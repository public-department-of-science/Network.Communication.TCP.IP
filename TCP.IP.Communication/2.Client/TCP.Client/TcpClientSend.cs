using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP.IP.Communication.Client
{
    public partial class TcpClient
    {
        /// <summary>
        /// Send data to the server.
        /// </summary>
        /// <param name="data">String containing data to send.</param>
        public void Send(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (!_isConnected)
            {
                throw new IOException("Not connected to the server; use Connect() first.");
            }
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            this.Send(bytes);
        }

        /// <summary>
        /// Send data to the server.
        /// </summary> 
        /// <param name="data">Byte array containing data to send.</param>
        public void Send(byte[] data)
        {
            if (data == null || data.Length < 1)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (!_isConnected)
            {
                throw new IOException("Not connected to the server; use Connect() first.");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Seek(0, SeekOrigin.Begin);
                SendInternal(data.Length, ms);
            }
        }

        /// <summary>
        /// Send data to the server.
        /// </summary>
        /// <param name="contentLength">The number of bytes to read from the source stream to send.</param>
        /// <param name="stream">Stream containing the data to send.</param>
        public void Send(long contentLength, Stream stream)
        {
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
            if (!_isConnected)
            {
                throw new IOException("Not connected to the server; use Connect() first.");
            }

            SendInternal(contentLength, stream);
        }

        /// <summary>
        /// Send data to the server asynchronously.
        /// </summary>
        /// <param name="data">String containing data to send.</param>
        /// <param name="token">Cancellation token for canceling the request.</param>
        public async Task SendAsync(string data, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (!_isConnected)
            {
                throw new IOException("Not connected to the server; use Connect() first.");
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
                await SendInternalAsync(bytes.Length, ms, token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Send data to the server asynchronously.
        /// </summary> 
        /// <param name="data">Byte array containing data to send.</param>
        /// <param name="token">Cancellation token for canceling the request.</param>
        public async Task SendAsync(byte[] data, CancellationToken token = default)
        {
            if (data == null || data.Length < 1)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (!_isConnected)
            {
                throw new IOException("Not connected to the server; use Connect() first.");
            }
            if (token == default(CancellationToken))
            {
                token = _token;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                await ms.WriteAsync(data, 0, data.Length, token).ConfigureAwait(false);
                ms.Seek(0, SeekOrigin.Begin);
                await SendInternalAsync(data.Length, ms, token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Send data to the server asynchronously.
        /// </summary>
        /// <param name="contentLength">The number of bytes to read from the source stream to send.</param>
        /// <param name="stream">Stream containing the data to send.</param>
        /// <param name="token">Cancellation token for canceling the request.</param>
        public async Task SendAsync(long contentLength, Stream stream, CancellationToken token = default)
        {
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
            if (!_isConnected)
            {
                throw new IOException("Not connected to the server; use Connect() first.");
            }
            if (token == default(CancellationToken))
            {
                token = _token;
            }

            await SendInternalAsync(contentLength, stream, token).ConfigureAwait(false);
        }

        private void SendInternal(long contentLength, Stream stream)
        {
            long bytesRemaining = contentLength;
            int bytesRead = 0;
            byte[] buffer = new byte[_settings.StreamBufferSize];

            try
            {
                _sendLock.Wait();

                while (bytesRemaining > 0)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        if (!_ssl)
                        {
                            _networkStream.Write(buffer, 0, bytesRead);
                        }
                        else
                        {
                            _sslStream.Write(buffer, 0, bytesRead);
                        }

                        bytesRemaining -= bytesRead;
                        _statistics.SentBytes += bytesRead;
                    }
                }

                if (!_ssl)
                {
                    _networkStream.Flush();
                }
                else
                {
                    _sslStream.Flush();
                }
            }
            finally
            {
                _sendLock.Release();
            }
        }

        private async Task SendInternalAsync(long contentLength, Stream stream, CancellationToken token)
        {
            try
            {
                long bytesRemaining = contentLength;
                int bytesRead = 0;
                byte[] buffer = new byte[_settings.StreamBufferSize];

                await _sendLock.WaitAsync(token).ConfigureAwait(false);

                while (bytesRemaining > 0)
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
                    if (bytesRead > 0)
                    {
                        if (!_ssl)
                        {
                            await _networkStream.WriteAsync(buffer, 0, bytesRead, token).ConfigureAwait(false);
                        }
                        else
                        {
                            await _sslStream.WriteAsync(buffer, 0, bytesRead, token).ConfigureAwait(false);
                        }

                        bytesRemaining -= bytesRead;
                        _statistics.SentBytes += bytesRead;
                    }
                }

                if (!_ssl)
                {
                    await _networkStream.FlushAsync(token).ConfigureAwait(false);
                }
                else
                {
                    await _sslStream.FlushAsync(token).ConfigureAwait(false);
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
                _sendLock.Release();
            }
        }

    }
}
