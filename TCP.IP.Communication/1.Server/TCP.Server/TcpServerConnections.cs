using System;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using TCP.Client.Metadata;
using TCP.EventArguments;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        private async Task AcceptConnections()
        {
            while (!_listenerToken.IsCancellationRequested)
            {
                ClientMetadata client = null;
                try
                {
                    System.Net.Sockets.TcpClient tcpClient = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    string clientIp = tcpClient.Client.RemoteEndPoint.ToString();

                    client = new ClientMetadata(tcpClient);
                    if (_ssl)
                    {
                        if (_settings.AcceptInvalidCertificates)
                        {
                            client.SslStream = new SslStream(client.NetworkStream, false, new RemoteCertificateValidationCallback(AcceptCertificate));
                        }
                        else
                        {
                            client.SslStream = new SslStream(client.NetworkStream, false);
                        }

                        bool success = await StartTls(client).ConfigureAwait(false);
                        if (!success)
                        {
                            client.Dispose();
                            continue;
                        }
                    }

                    _clients.TryAdd(clientIp, client);
                    _clientsLastSeen.TryAdd(clientIp, DateTime.UtcNow);
                    Logger?.Invoke($"{_header}starting data receiver for: {clientIp}");
                    _events.HandleClientConnected(this, new ConnectionEventArgs(clientIp));

                    if (_keepalive.EnableTcpKeepAlives)
                    {
                        EnableKeepalives(tcpClient);
                    }

                    CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(client.Token, _token);
                    Task unawaited = Task.Run(() => DataReceiver(client), linkedCts.Token);
                }
                catch (Exception ex)
                {
                    if (ex is TaskCanceledException
                        || ex is OperationCanceledException
                        || ex is ObjectDisposedException
                        || ex is InvalidOperationException)
                    {
                        _isListening = false;
                        if (client != null)
                        {
                            client.Dispose();
                        }

                        Logger?.Invoke($"{_header}stopped listening");
                        break;
                    }
                    else
                    {
                        if (client != null)
                        {
                            client.Dispose();
                        }

                        Logger?.Invoke($"{_header}exception while awaiting connections: {ex}");
                        continue;
                    }
                }
            }

            _isListening = false;
        }

        /// <summary>
        /// Stop accepting new connections.
        /// </summary>
        public void AbortNewConnections()
        {
            if (!_isListening)
            {
                throw new InvalidOperationException("TcpServer is not running.");
            }

            _isListening = false;
            _listener.Stop();
            _listenerTokenSource.Cancel();
            _acceptConnections.Wait();
            _acceptConnections = null;


            Logger?.Invoke($"{_header}stopped");
        }
    }
}
