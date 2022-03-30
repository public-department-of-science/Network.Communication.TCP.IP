using System;
using System.IO;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        /// <summary>
        /// Send data to the specified client by IP:port.
        /// </summary>
        /// <param name="ipPort">The client IP:port string.</param>
        /// <param name="contentLength">The number of bytes to read from the source stream to send.</param>
        /// <param name="stream">Stream containing the data to send.</param>
        public void NotifyAllClients(long contentLength, Stream stream)
        {
            var clients = GetClients();
            foreach (var clientIp in clients)
            {
                if (string.IsNullOrWhiteSpace(clientIp))
                {
                    throw new ArgumentNullException(nameof(clientIp));
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

                SendInternal(clientIp, contentLength, stream);
            }
        }
    }
}
