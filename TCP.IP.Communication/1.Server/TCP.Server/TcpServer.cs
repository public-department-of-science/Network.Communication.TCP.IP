using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using TCP.Client;
using TCP.Common;
using TCP.Server.EventsHandler;
using TCP.Server.Settings;
using TCP.Settings;
using TCP.Statistic;

namespace TCP.IP.Communication.Server
{
    /// <summary>
    /// Tcp server with SSL support.  
    /// Set the ClientConnected, ClientDisconnected, and DataReceived events.  
    /// Once set, use Start() to begin listening for connections.
    /// </summary>
    public partial class TcpServer : IDisposable
    {
        /// <summary>
        /// Method to invoke to send a log message.
        /// </summary>
        public Action<string> Logger = null;

        private readonly string _header = "[Tcp.Server] ";
        private TcpServerSettings _settings = new TcpServerSettings();
        private TcpServerEventsHandler _events = new TcpServerEventsHandler();
        private TcpKeepAliveSettings _keepalive = new TcpKeepAliveSettings();
        private TcpStatistic _statistics = new TcpStatistic();

        private readonly string _listenerIp = null;
        private readonly IPAddress _ipAddress = null;
        private readonly int _port = 0;
        private readonly bool _ssl = false;

        private readonly X509Certificate2 _sslCertificate = null;
        private readonly X509Certificate2Collection _sslCertificateCollection = null;

        private readonly ConcurrentDictionary<string, ClientMetadata> _clients = new ConcurrentDictionary<string, ClientMetadata>();
        private readonly ConcurrentDictionary<string, DateTime> _clientsLastSeen = new ConcurrentDictionary<string, DateTime>();
        private readonly ConcurrentDictionary<string, DateTime> _clientsKicked = new ConcurrentDictionary<string, DateTime>();
        private readonly ConcurrentDictionary<string, DateTime> _clientsTimedout = new ConcurrentDictionary<string, DateTime>();

        private TcpListener _listener = null;
        private bool _isListening = false;

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private CancellationToken _token;
        private CancellationTokenSource _listenerTokenSource = new CancellationTokenSource();
        private CancellationToken _listenerToken;
        private Task _acceptConnections = null;
        private Task _idleClientMonitor = null;

        /// <summary>
        /// Instantiates the TCP server without SSL.  Set the ClientConnected, ClientDisconnected, and DataReceived callbacks.  Once set, use Start() to begin listening for connections.
        /// </summary>
        /// <param name="ipPort">The IP:port of the server.</param> 
        public TcpServer(string ipPort)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }

            IpAddressAndPortParser.ParseIpPort(ipPort, out _listenerIp, out _port);
            if (_port < 0)
            {
                throw new ArgumentException("Port must be zero or greater.");
            }

            if (string.IsNullOrWhiteSpace(_listenerIp))
            {
                _ipAddress = IPAddress.Loopback;
                _listenerIp = _ipAddress.ToString();
            }
            else if (_listenerIp == "*" || _listenerIp == "+")
            {
                _ipAddress = IPAddress.Any;
            }
            else
            {
                if (!IPAddress.TryParse(_listenerIp, out _ipAddress))
                {
                    _ipAddress = Dns.GetHostEntry(_listenerIp).AddressList[0];
                    _listenerIp = _ipAddress.ToString();
                }
            }

            _isListening = false;
            _token = _tokenSource.Token;
        }

        /// <summary>
        /// Instantiates the TCP server without SSL.  Set the ClientConnected, ClientDisconnected, and DataReceived callbacks.  Once set, use Start() to begin listening for connections.
        /// </summary>
        /// <param name="listenerIp">The listener IP address or hostname.</param>
        /// <param name="port">The TCP port on which to listen.</param>
        public TcpServer(string listenerIp, int port)
        {
            if (port < 0)
            {
                throw new ArgumentException("Port must be zero or greater.");
            }

            _listenerIp = listenerIp;
            _port = port;

            if (string.IsNullOrWhiteSpace(_listenerIp))
            {
                _ipAddress = IPAddress.Loopback;
                _listenerIp = _ipAddress.ToString();
            }
            else if (_listenerIp == "*" || _listenerIp == "+")
            {
                _ipAddress = IPAddress.Any;
                _listenerIp = listenerIp;
            }
            else
            {
                if (!IPAddress.TryParse(_listenerIp, out _ipAddress))
                {
                    _ipAddress = Dns.GetHostEntry(listenerIp).AddressList[0];
                    _listenerIp = _ipAddress.ToString();
                }
            }

            _isListening = false;
            _token = _tokenSource.Token;
        }

        /// <summary>
        /// Instantiates the TCP server.  Set the ClientConnected, ClientDisconnected, and DataReceived callbacks.  Once set, use Start() to begin listening for connections.
        /// </summary>
        /// <param name="ipPort">The IP:port of the server.</param> 
        /// <param name="ssl">Enable or disable SSL.</param>
        /// <param name="pfxCertFilename">The filename of the PFX certificate file.</param>
        /// <param name="pfxPassword">The password to the PFX certificate file.</param>
        public TcpServer(string ipPort, bool ssl, string pfxCertFilename, string pfxPassword)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }

            IpAddressAndPortParser.ParseIpPort(ipPort, out _listenerIp, out _port);
            if (_port < 0)
            {
                throw new ArgumentException("Port must be zero or greater.");
            }

            if (string.IsNullOrWhiteSpace(_listenerIp))
            {
                _ipAddress = IPAddress.Loopback;
                _listenerIp = _ipAddress.ToString();
            }
            else if (_listenerIp == "*" || _listenerIp == "+")
            {
                _ipAddress = IPAddress.Any;
            }
            else
            {
                if (!IPAddress.TryParse(_listenerIp, out _ipAddress))
                {
                    _ipAddress = Dns.GetHostEntry(_listenerIp).AddressList[0];
                    _listenerIp = _ipAddress.ToString();
                }
            }

            _ssl = ssl;
            _isListening = false;
            _token = _tokenSource.Token;

            if (_ssl)
            {
                if (string.IsNullOrWhiteSpace(pfxPassword))
                {
                    _sslCertificate = new X509Certificate2(pfxCertFilename);
                }
                else
                {
                    _sslCertificate = new X509Certificate2(pfxCertFilename, pfxPassword);
                }

                _sslCertificateCollection = new X509Certificate2Collection
                {
                    _sslCertificate
                };
            }
        }

        /// <summary>
        /// Instantiates the TCP server.  Set the ClientConnected, ClientDisconnected, and DataReceived callbacks.  Once set, use Start() to begin listening for connections.
        /// </summary>
        /// <param name="listenerIp">The listener IP address or hostname.</param>
        /// <param name="port">The TCP port on which to listen.</param>
        /// <param name="ssl">Enable or disable SSL.</param>
        /// <param name="pfxCertFilename">The filename of the PFX certificate file.</param>
        /// <param name="pfxPassword">The password to the PFX certificate file.</param>
        public TcpServer(string listenerIp, int port, bool ssl, string pfxCertFilename, string pfxPassword)
        {
            if (port < 0) throw new ArgumentException("Port must be zero or greater.");

            _listenerIp = listenerIp;
            _port = port;

            if (string.IsNullOrWhiteSpace(_listenerIp))
            {
                _ipAddress = IPAddress.Loopback;
                _listenerIp = _ipAddress.ToString();
            }
            else if (_listenerIp == "*" || _listenerIp == "+")
            {
                _ipAddress = IPAddress.Any;
            }
            else
            {
                if (!IPAddress.TryParse(_listenerIp, out _ipAddress))
                {
                    _ipAddress = Dns.GetHostEntry(listenerIp).AddressList[0];
                    _listenerIp = _ipAddress.ToString();
                }
            }

            _ssl = ssl;
            _isListening = false;
            _token = _tokenSource.Token;

            if (_ssl)
            {
                if (string.IsNullOrWhiteSpace(pfxPassword))
                {
                    _sslCertificate = new X509Certificate2(pfxCertFilename);
                }
                else
                {
                    _sslCertificate = new X509Certificate2(pfxCertFilename, pfxPassword);
                }

                _sslCertificateCollection = new X509Certificate2Collection
                {
                    _sslCertificate
                };
            }
        }

        /// <summary>
        /// Dispose of the TCP server.
        /// </summary>
        /// <param name="disposing">Dispose of resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (_clients != null && _clients.Count > 0)
                    {
                        foreach (KeyValuePair<string, ClientMetadata> curr in _clients)
                        {
                            curr.Value.Dispose();
                            Logger?.Invoke($"{_header}disconnected client: {curr.Key}");
                        }
                    }

                    if (_tokenSource != null)
                    {
                        if (!_tokenSource.IsCancellationRequested)
                        {
                            _tokenSource.Cancel();
                        }

                        _tokenSource.Dispose();
                    }

                    if (_listener != null && _listener.Server != null)
                    {
                        _listener.Server.Close();
                        _listener.Server.Dispose();
                    }

                    if (_listener != null)
                    {
                        _listener.Stop();
                    }
                }
                catch (Exception e)
                {
                    Logger?.Invoke($"{_header}dispose exception:{Environment.NewLine}{e}{Environment.NewLine}");
                }

                _isListening = false;

                Logger?.Invoke($"{_header}disposed");
            }
        }

        /// <summary>
        /// Dispose of the TCP server.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
