using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;

namespace TCP.Client
{
    internal class ClientMetadata : IDisposable
    {
        private System.Net.Sockets.TcpClient tcpClient;
        private NetworkStream networkStream;
        private SslStream sslStream;
        private string requestedPort = string.Empty;

        internal System.Net.Sockets.TcpClient Client => tcpClient;

        internal NetworkStream NetworkStream => networkStream;

        internal SslStream SslStream
        {
            get { return sslStream; }
            set { sslStream = value; }
        }

        internal string IpPort
        {
            get { return requestedPort; }
        }

        internal SemaphoreSlim sendLock = new SemaphoreSlim(1, 1);
        internal SemaphoreSlim receiveLock = new SemaphoreSlim(1, 1);

        internal CancellationTokenSource TokenSource { get; set; }

        internal CancellationToken Token { get; set; }

        internal ClientMetadata(System.Net.Sockets.TcpClient tcpClient)
        {
            if (tcpClient == null)
            {
                throw new ArgumentNullException(nameof(tcpClient));
            }

            this.tcpClient = tcpClient;
            
            networkStream = tcpClient.GetStream();
            requestedPort = tcpClient.Client.RemoteEndPoint.ToString();

            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;
        }

        public void Dispose()
        {
            if (TokenSource != null)
            {
                if (!TokenSource.IsCancellationRequested)
                {
                    TokenSource.Cancel();
                    TokenSource.Dispose();
                }
            }

            if (sslStream != null)
            {
                sslStream.Close();
            }

            if (networkStream != null)
            {
                networkStream.Close();
            }

            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient.Dispose();
            }

            sendLock.Dispose();
            receiveLock.Dispose();
        }
    }
}
