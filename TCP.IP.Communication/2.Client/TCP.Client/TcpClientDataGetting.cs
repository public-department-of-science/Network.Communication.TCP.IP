using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TCP.Enums;
using TCP.EventArguments;

namespace TCP.IP.Communication.Client
{
    public partial class TcpClient
    {
        private async Task DataReceiver(CancellationToken token)
        {
            Stream outerStream = null;
            if (!_ssl)
            {
                outerStream = _networkStream;
            }
            else
            {
                outerStream = _sslStream;
            }

            while (!token.IsCancellationRequested && _client != null && _client.Connected)
            {
                try
                {
                    await DataReadAsync(token)
                        .ContinueWith(async task =>
                    {
                        if (task.IsCanceled)
                        {
                            return null;
                        }

                        byte[] data = task.Result;
                        if (data != null)
                        {
                            _lastActivity = DateTime.UtcNow;
                            _events.HandleDataReceived(this, new DataReceivedEventArgs(ServerIpPort, data));
                            _statistics.ReceivedBytes += data.Length;

                            return data;
                        }
                        else
                        {
                            await Task.Delay(100).ConfigureAwait(false);
                            return null;
                        }

                    }, token).ContinueWith(task => { }).ConfigureAwait(false);
                }
                catch (AggregateException)
                {
                    Logger?.Invoke($"{_header}data receiver canceled, disconnected");
                    break;
                }
                catch (IOException)
                {
                    Logger?.Invoke($"{_header}data receiver canceled, disconnected");
                    break;
                }
                catch (SocketException)
                {
                    Logger?.Invoke($"{_header}data receiver canceled, disconnected");
                    break;
                }
                catch (TaskCanceledException)
                {
                    Logger?.Invoke($"{_header}data receiver task canceled, disconnected");
                    break;
                }
                catch (OperationCanceledException)
                {
                    Logger?.Invoke($"{_header}data receiver operation canceled, disconnected");
                    break;
                }
                catch (ObjectDisposedException)
                {
                    Logger?.Invoke($"{_header}data receiver canceled due to disposal, disconnected");
                    break;
                }
                catch (Exception e)
                {
                    Logger?.Invoke($"{_header}data receiver exception:{Environment.NewLine}{e}{Environment.NewLine}");
                    break;
                }
            }

            Logger?.Invoke($"{_header}disconnection detected");

            _isConnected = false;

            if (!_isTimeout)
            {
                _events.HandleClientDisconnected(this, new DisconnectionEventArgs(ServerIpPort, DisconnectionReason.DisconnectOK_ByClient));
            }
            else
            {
                _events.HandleClientDisconnected(this, new DisconnectionEventArgs(ServerIpPort, DisconnectionReason.NoResponse_Timeout));
            }

            Dispose();
        }

        private async Task<byte[]> DataReadAsync(CancellationToken token)
        {
            byte[] buffer = new byte[_settings.StreamBufferSize];
            int read = 0;

            try
            {
                if (!_ssl)
                {
                    read = await _networkStream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
                }
                else
                {
                    read = await _sslStream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
                }

                if (read > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(buffer, 0, read);
                        return ms.ToArray();
                    }
                }
                else
                {
                    throw new SocketException();
                }
            }
            catch (IOException)
            {
                // thrown if ReadTimeout (ms) is exceeded
                // see https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.networkstream.readtimeout?view=net-6.0
                // and https://github.com/dotnet/runtime/issues/24093
                return null;
            }
        }
    }
}
