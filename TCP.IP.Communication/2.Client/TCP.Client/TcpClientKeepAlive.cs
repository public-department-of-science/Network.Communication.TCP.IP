using System;
using System.Net.Sockets;

namespace TCP.IP.Communication.Client
{
    public partial class TcpClient
    {
        private void EnableKeepalives()
        {
            try
            {
#if NETCOREAPP || NET5_0

                _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                _client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, _keepalive.TcpKeepAliveTime);
                _client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, _keepalive.TcpKeepAliveInterval);
                _client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, _keepalive.TcpKeepAliveRetryCount);

#elif NETFRAMEWORK

                byte[] keepAlive = new byte[12];

                // Turn keepalive on
                Buffer.BlockCopy(BitConverter.GetBytes((uint)1), 0, keepAlive, 0, 4);

                // Set TCP keepalive time
                Buffer.BlockCopy(BitConverter.GetBytes((uint)_keepalive.TcpKeepAliveTime), 0, keepAlive, 4, 4);

                // Set TCP keepalive interval
                Buffer.BlockCopy(BitConverter.GetBytes((uint)_keepalive.TcpKeepAliveInterval), 0, keepAlive, 8, 4);

                // Set keepalive settings on the underlying Socket
                _client.Client.IOControl(IOControlCode.KeepAliveValues, keepAlive, null);

#elif NETSTANDARD

#endif
            }
            catch (Exception)
            {
                Logger?.Invoke($"{_header}keepalives not supported on this platform, disabled");
                _keepalive.EnableTcpKeepAlives = false;
            }
        }
    }
}
