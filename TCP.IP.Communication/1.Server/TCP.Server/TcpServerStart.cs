using System;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using TCP.Client.Metadata;
using TCP.Statistic;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        /// <summary>
        /// Start accepting connections.
        /// </summary>
        public void Start()
        {
            if (_isListening)
            {
                throw new InvalidOperationException("TcpServer is already running.");
            }

            _listener = new TcpListener(_ipAddress, _port);

            _listener.Start();
            _isListening = true;

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _listenerTokenSource = new CancellationTokenSource();
            _listenerToken = _listenerTokenSource.Token;

            _statistics = new TcpStatistic();

            if (_idleClientMonitor == null)
            {
                _idleClientMonitor = Task.Run(() => IdleClientMonitor(), _token);
            }

            _acceptConnections = Task.Run(() => AcceptConnections(), _listenerToken);
        }

        /// <summary>
        /// Start accepting connections.
        /// </summary>
        /// <returns>Task.</returns>
        public Task StartAsync()
        {
            if (_isListening)
            {
                throw new InvalidOperationException("TcpServer is already running.");
            }
            _isListening = true;


            if (_keepalive.EnableTcpKeepAlives)
            {
                EnableKeepalives();
            }

            _listener = new TcpListener(_ipAddress, _port);
            _listener.Start();

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _listenerTokenSource = new CancellationTokenSource();
            _listenerToken = _listenerTokenSource.Token;

            _statistics = new TcpStatistic();

            if (_idleClientMonitor == null)
            {
                _idleClientMonitor = Task.Run(() => IdleClientMonitor(), _token);
            }

            _acceptConnections = Task.Run(() => AcceptConnections(), _listenerToken);
            return _acceptConnections;
        }

        private async Task<bool> StartTls(ClientMetadata client)
        {
            try
            {
                await client.SslStream.AuthenticateAsServerAsync(_sslCertificate, _settings.MutuallyAuthenticate,
                    SslProtocols.Tls12,
                    !_settings.AcceptInvalidCertificates)
                    .ConfigureAwait(false);

                if (!client.SslStream.IsEncrypted)
                {
                    Logger?.Invoke($"{_header}client {client.IpPort} not encrypted, disconnecting");
                    client.Dispose();
                    return false;
                }

                if (!client.SslStream.IsAuthenticated)
                {
                    Logger?.Invoke($"{_header}client {client.IpPort} not SSL/TLS authenticated, disconnecting");
                    client.Dispose();
                    return false;
                }

                if (_settings.MutuallyAuthenticate && !client.SslStream.IsMutuallyAuthenticated)
                {
                    Logger?.Invoke($"{_header}client {client.IpPort} failed mutual authentication, disconnecting");
                    client.Dispose();
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger?.Invoke($"{_header}client {client.IpPort} SSL/TLS exception: {Environment.NewLine}{e}");
                client.Dispose();
                return false;
            }

            return true;
        }
    }
}
