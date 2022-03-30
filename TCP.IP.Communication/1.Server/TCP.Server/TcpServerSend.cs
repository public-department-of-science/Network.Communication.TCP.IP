using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCP.Client;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        /// <summary>
        /// Send data to the specified client by IP:port.
        /// </summary>
        /// <param name="ipPort">The client IP:port string.</param>
        /// <param name="data">String containing data to send.</param>
        public void Send(string ipPort, string data)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                SendInternal(ipPort, bytes.Length, ms);
            }
        }

        /// <summary>
        /// Send data to the specified client by IP:port.
        /// </summary>
        /// <param name="ipPort">The client IP:port string.</param>
        /// <param name="data">Byte array containing data to send.</param>
        public void Send(string ipPort, byte[] data)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }
            if (data == null || data.Length < 1)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Seek(0, SeekOrigin.Begin);
                SendInternal(ipPort, data.Length, ms);
            }
        }

        /// <summary>
        /// Send data to the specified client by IP:port.
        /// </summary>
        /// <param name="ipPort">The client IP:port string.</param>
        /// <param name="contentLength">The number of bytes to read from the source stream to send.</param>
        /// <param name="stream">Stream containing the data to send.</param>
        public void Send(string ipPort, long contentLength, Stream stream)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }
            if (contentLength < 1)
            {
                return;
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead)
            {
                throw new InvalidOperationException("Cannot read from supplied stream.");
            }

            SendInternal(ipPort, contentLength, stream);
        }

        /// <summary>
        /// Send data to the specified client by IP:port asynchronously.
        /// </summary>
        /// <param name="ipPort">The client IP:port string.</param>
        /// <param name="data">String containing data to send.</param>
        /// <param name="token">Cancellation token for canceling the request.</param>
        public async Task SendAsync(string ipPort, string data, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (token == default(CancellationToken))
            {
                token = _token;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            using (MemoryStream ms = new MemoryStream())
            {
                await ms.WriteAsync(bytes, 0, bytes.Length, token).ConfigureAwait(false);
                ms.Seek(0, SeekOrigin.Begin);
                await SendInternalAsync(ipPort, bytes.Length, ms, token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Send data to the specified client by IP:port asynchronously.
        /// </summary>
        /// <param name="ipPort">The client IP:port string.</param>
        /// <param name="data">Byte array containing data to send.</param>
        /// <param name="token">Cancellation token for canceling the request.</param>
        public async Task SendAsync(string ipPort, byte[] data, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }
            if (data == null || data.Length < 1)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (token == default(CancellationToken))
            {
                token = _token;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                await ms.WriteAsync(data, 0, data.Length, token).ConfigureAwait(false);
                ms.Seek(0, SeekOrigin.Begin);
                await SendInternalAsync(ipPort, data.Length, ms, token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Send data to the specified client by IP:port asynchronously.
        /// </summary>
        /// <param name="ipPort">The client IP:port string.</param>
        /// <param name="contentLength">The number of bytes to read from the source stream to send.</param>
        /// <param name="stream">Stream containing the data to send.</param>
        /// <param name="token">Cancellation token for canceling the request.</param>
        public async Task SendAsync(string ipPort, long contentLength, Stream stream, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }
            if (contentLength < 1)
            {
                return;
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead)
            {
                throw new InvalidOperationException("Cannot read from supplied stream.");
            }
            if (token == default(CancellationToken))
            {
                token = _token;
            }

            await SendInternalAsync(ipPort, contentLength, stream, token).ConfigureAwait(false);
        }

        private void SendInternal(string ipPort, long contentLength, Stream stream)
        {
            if (!_clients.TryGetValue(ipPort, out ClientMetadata client))
            {
                return;
            }
            if (client == null)
            {
                return;
            }

            long bytesRemaining = contentLength;
            int bytesRead = 0;
            byte[] buffer = new byte[_settings.StreamBufferSize];

            try
            {
                client.sendLock.Wait();

                while (bytesRemaining > 0)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        if (!_ssl)
                        {
                            client.NetworkStream.Write(buffer, 0, bytesRead);
                        }
                        else
                        {
                            client.SslStream.Write(buffer, 0, bytesRead);
                        }

                        bytesRemaining -= bytesRead;
                        _statistics.SentBytes += bytesRead;
                    }
                }

                if (!_ssl)
                {
                    client.NetworkStream.Flush();
                }
                else
                {
                    client.SslStream.Flush();
                }
            }
            finally
            {
                if (client != null) client.sendLock.Release();
            }
        }

        private async Task SendInternalAsync(string ipPort, long contentLength, Stream stream, CancellationToken token)
        {
            ClientMetadata client = null;

            try
            {
                if (!_clients.TryGetValue(ipPort, out client))
                {
                    return;
                }
                if (client == null)
                {
                    return;
                }

                long bytesRemaining = contentLength;
                int bytesRead = 0;
                byte[] buffer = new byte[_settings.StreamBufferSize];

                await client.sendLock.WaitAsync(token).ConfigureAwait(false);

                while (bytesRemaining > 0)
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
                    if (bytesRead > 0)
                    {
                        if (!_ssl)
                        {
                            await client.NetworkStream.WriteAsync(buffer, 0, bytesRead, token).ConfigureAwait(false);
                        }
                        else
                        {
                            await client.SslStream.WriteAsync(buffer, 0, bytesRead, token).ConfigureAwait(false);
                        }

                        bytesRemaining -= bytesRead;
                        _statistics.SentBytes += bytesRead;
                    }
                }

                if (!_ssl)
                {
                    await client.NetworkStream.FlushAsync(token).ConfigureAwait(false);
                }
                else
                {
                    await client.SslStream.FlushAsync(token).ConfigureAwait(false);
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (client != null)
                {
                    client.sendLock.Release();
                }
            }
        }
    }
}
