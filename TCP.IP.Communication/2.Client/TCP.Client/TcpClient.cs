using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCP.Client;
using TCP.Common;
using TCP.Enums;
using TCP.EventArguments;
using TCP.Settings;
using TCP.Statistic;

namespace TCP.IP.Communication.Client
{
    /// <summary>
    /// Tcp client with SSL support.  
    /// Set the Connected, Disconnected, and DataReceived events.  
    /// Once set, use Connect() to connect to the server.
    /// </summary>
    public partial class TcpClient : IDisposable
    {
        private readonly string _header = "[Tcp.Client] ";
        private TcpClientSettings _settings = new TcpClientSettings();
        private TcpClientEventsHandler _events = new TcpClientEventsHandler();
        private TcpKeepAliveSettings _keepalive = new TcpKeepAliveSettings();
        private TcpStatistic _statistics = new TcpStatistic();

        private string _serverIp = string.Empty;
        private int _serverPort = 0;
        private readonly IPAddress _ipAddress;
        private System.Net.Sockets.TcpClient _client;
        private NetworkStream _networkStream;

        private bool _ssl = false;
        private string _pfxCertFilename = string.Empty;
        private string _pfxPassword = string.Empty;
        private SslStream _sslStream;
        private X509Certificate2 _sslCert;
        private X509Certificate2Collection _sslCertCollection;

        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);
        private bool _isConnected = false;

        private Task _dataReceiver;
        private Task _idleServerMonitor;
        private Task _connectionMonitor;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private CancellationToken _token;

        private DateTime _lastActivity = DateTime.UtcNow;
        private bool _isTimeout = false;

        /// <summary>
        /// Method to invoke to send a log message.
        /// </summary>
        public Action<string> Logger;

        /// <summary>
        /// Instantiates the TCP client without SSL. Set the Connected, Disconnected, and DataReceived callbacks. Once set, use Connect() to connect to the server.
        /// </summary>
        /// <param name="ipPort">The IP:port of the server.</param> 
        public TcpClient(string ipPort)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }

            IpAddressAndPortParser.ParseIpPort(ipPort, out _serverIp, out _serverPort);
            if (_serverPort < 0)
            {
                throw new ArgumentException("Port must be zero or greater.");
            }
            if (string.IsNullOrWhiteSpace(_serverIp))
            {
                throw new ArgumentNullException("Server IP or hostname must not be null.");
            }

            if (!IPAddress.TryParse(_serverIp, out _ipAddress))
            {
                _ipAddress = Dns.GetHostEntry(_serverIp).AddressList[0];
                _serverIp = _ipAddress.ToString();
            }
        }

        /// <summary>
        /// Instantiates the TCP client. Set the Connected, Disconnected, and DataReceived callbacks. Once set, use Connect() to connect to the server.
        /// </summary>
        /// <param name="ipPort">The IP:port of the server.</param> 
        /// <param name="ssl">Enable or disable SSL.</param>
        /// <param name="pfxCertFilename">The filename of the PFX certificate file.</param>
        /// <param name="pfxPassword">The password to the PFX certificate file.</param>
        public TcpClient(string ipPort, bool ssl, string pfxCertFilename, string pfxPassword) : this(ipPort)
        {
            _ssl = ssl;
            _pfxCertFilename = pfxCertFilename;
            _pfxPassword = pfxPassword;
        }

        /// <summary>
        /// Instantiates the TCP client without SSL. Set the Connected, Disconnected, and DataReceived callbacks. Once set, use Connect() to connect to the server.
        /// </summary>
        /// <param name="serverIpOrHostname">The server IP address or hostname.</param>
        /// <param name="port">The TCP port on which to connect.</param>
        public TcpClient(string serverIpOrHostname, int port)
        {
            if (string.IsNullOrWhiteSpace(serverIpOrHostname))
            {
                throw new ArgumentNullException(nameof(serverIpOrHostname));
            }
            if (port < 0)
            {
                throw new ArgumentException("Port must be zero or greater.");
            }

            _serverIp = serverIpOrHostname;
            _serverPort = port;

            if (!IPAddress.TryParse(_serverIp, out _ipAddress))
            {
                _ipAddress = Dns.GetHostEntry(serverIpOrHostname).AddressList[0];
                _serverIp = _ipAddress.ToString();
            }
        }

        /// <summary>
        /// Instantiates the TCP client.  Set the Connected, Disconnected, and DataReceived callbacks.  Once set, use Connect() to connect to the server.
        /// </summary>
        /// <param name="serverIpOrHostname">The server IP address or hostname.</param>
        /// <param name="port">The TCP port on which to connect.</param>
        /// <param name="ssl">Enable or disable SSL.</param>
        /// <param name="pfxCertFilename">The filename of the PFX certificate file.</param>
        /// <param name="pfxPassword">The password to the PFX certificate file.</param>
        public TcpClient(string serverIpOrHostname, int port, bool ssl, string pfxCertFilename, string pfxPassword) : this(serverIpOrHostname, port)
        {
            _ssl = ssl;
            _pfxCertFilename = pfxCertFilename;
            _pfxPassword = pfxPassword;
        }

        /// <summary>
        /// Dispose of the TCP client.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of the TCP client.
        /// </summary>
        /// <param name="disposing">Dispose of resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isConnected = false;

                if (_tokenSource != null)
                {
                    if (!_tokenSource.IsCancellationRequested)
                    {
                        _tokenSource.Cancel();
                        _tokenSource.Dispose();
                    }
                }

                if (_sslStream != null)
                {
                    _sslStream.Close();
                    _sslStream.Dispose();
                }

                if (_networkStream != null)
                {
                    _networkStream.Close();
                    _networkStream.Dispose();
                }

                if (_client != null)
                {
                    _client.Close();
                    _client.Dispose();
                }

                Logger?.Invoke($"{_header}dispose complete");
            }
        }

        private void InitializeClient(bool ssl, string pfxCertFilename, string pfxPassword)
        {
            _ssl = ssl;
            _pfxCertFilename = pfxCertFilename;
            _pfxPassword = pfxPassword;
            _client = new System.Net.Sockets.TcpClient();
            _sslStream = null;
            _sslCert = null;
            _sslCertCollection = null;

            if (_ssl)
            {
                if (string.IsNullOrWhiteSpace(pfxPassword))
                {
                    _sslCert = new X509Certificate2(pfxCertFilename);
                }
                else
                {
                    _sslCert = new X509Certificate2(pfxCertFilename, pfxPassword);
                }

                _sslCertCollection = new X509Certificate2Collection
                {
                    _sslCert
                };
            }
        }
    }
}