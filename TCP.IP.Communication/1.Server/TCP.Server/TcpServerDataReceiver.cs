using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TCP.Client.Metadata;
using TCP.Enums;
using TCP.EventArguments;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        private async Task DataReceiver(ClientMetadata client)
        {
            string ipPort = client.IpPort;
            Logger?.Invoke($"{_header}data receiver started for client {ipPort}");

            CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_token, client.Token);
            while (true)
            {
                try
                {
                    if (!IsClientConnected(client.TcpClient))
                    {
                        Logger?.Invoke($"{_header}client {ipPort} disconnected");
                        break;
                    }

                    if (client.Token.IsCancellationRequested)
                    {
                        Logger?.Invoke($"{_header}cancellation requested (data receiver for client {ipPort})");
                        break;
                    }

                    byte[] data = await DataReadAsync(client, linkedCts.Token).ConfigureAwait(false);
                    if (data == null)
                    {
                        await Task.Delay(10, linkedCts.Token).ConfigureAwait(false);
                        continue;
                    }

                    _ = Task.Run(() => _events.HandleDataReceived(this, new DataReceivedEventArgs(ipPort, data)), linkedCts.Token);
                    _statistics.ReceivedBytes += data.Length;
                    UpdateClientLastSeen(client.IpPort);
                }
                catch (IOException)
                {
                    Logger?.Invoke($"{_header}data receiver canceled, peer disconnected [{ipPort}]");
                    break;
                }
                catch (SocketException)
                {
                    Logger?.Invoke($"{_header}data receiver canceled, peer disconnected [{ipPort}]");
                    break;
                }
                catch (TaskCanceledException)
                {
                    Logger?.Invoke($"{_header}data receiver task canceled [{ipPort}]");
                    break;
                }
                catch (ObjectDisposedException)
                {
                    Logger?.Invoke($"{_header}data receiver canceled due to disposal [{ipPort}]");
                    break;
                }
                catch (Exception e)
                {
                    Logger?.Invoke($"{_header}data receiver exception [{ipPort}]:{ Environment.NewLine}{e}{Environment.NewLine}");
                    break;
                }
            }

            Logger?.Invoke($"{_header}data receiver terminated for client {ipPort}");

            if (_clientsKicked.ContainsKey(ipPort))
            {
                _events.HandleClientDisconnected(this, new DisconnectionEventArgs(ipPort, DisconnectionStatus.KickedOut_ByServer));
            }
            else if (_clientsTimedout.ContainsKey(client.IpPort))
            {
                _events.HandleClientDisconnected(this, new DisconnectionEventArgs(ipPort, DisconnectionStatus.NoResponse_Timeout));
            }
            else
            {
                _events.HandleClientDisconnected(this, new DisconnectionEventArgs(ipPort, DisconnectionStatus.DisconnectOK_ByClient));
            }

            _clients.TryRemove(ipPort, out _);
            _clientsLastSeen.TryRemove(ipPort, out _);
            _clientsKicked.TryRemove(ipPort, out _);
            _clientsTimedout.TryRemove(ipPort, out _);

            if (client != null)
            {
                client.Dispose();
            }
        }

        private async Task<byte[]> DataReadAsync(ClientMetadata client, CancellationToken token)
        {
            byte[] buffer = new byte[_settings.StreamBufferSize];
            int read = 0;

            if (!_ssl)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    while (true)
                    {
                        read = await client.NetworkStream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);

                        if (read > 0)
                        {
                            await ms.WriteAsync(buffer, 0, read, token).ConfigureAwait(false);
                            return ms.ToArray();
                        }
                        else
                        {
                            throw new SocketException();
                        }
                    }
                }
            }
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    while (true)
                    {
                        read = await client.SslStream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);

                        if (read > 0)
                        {
                            await ms.WriteAsync(buffer, 0, read, token).ConfigureAwait(false);
                            return ms.ToArray();
                        }
                        else
                        {
                            throw new SocketException();
                        }
                    }
                }
            }
        }
    }
}
