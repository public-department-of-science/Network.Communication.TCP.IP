using System;
using System.Net.Sockets;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        private void EnableKeepalives()
        {
            try
            {
#if NETCOREAPP || NET5_0

                _listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                _listener.Server.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, _keepalive.TcpKeepAliveTime);
                _listener.Server.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, _keepalive.TcpKeepAliveInterval);
                _listener.Server.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, _keepalive.TcpKeepAliveRetryCount);

#elif NETFRAMEWORK

            byte[] keepAlive = new byte[12];

            // Turn keepalive on
            Buffer.BlockCopy(BitConverter.GetBytes((uint)1), 0, keepAlive, 0, 4);

            // Set TCP keepalive time
            Buffer.BlockCopy(BitConverter.GetBytes((uint)_keepalive.TcpKeepAliveTime), 0, keepAlive, 4, 4);

            // Set TCP keepalive interval
            Buffer.BlockCopy(BitConverter.GetBytes((uint)_keepalive.TcpKeepAliveInterval), 0, keepAlive, 8, 4);

            // Set keepalive settings on the underlying Socket
            _listener.Server.IOControl(IOControlCode.KeepAliveValues, keepAlive, null);

#elif NETSTANDARD

#endif
            }
            catch (Exception)
            {
                Logger?.Invoke($"{_header}keepalives not supported on this platform, disabled");
            }
        }

        private void EnableKeepalives(System.Net.Sockets.TcpClient client)
        {
            try
            {
#if NETCOREAPP || NET5_0

                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, _keepalive.TcpKeepAliveTime);
                client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, _keepalive.TcpKeepAliveInterval);
                client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, _keepalive.TcpKeepAliveRetryCount);

#elif NETFRAMEWORK

                byte[] keepAlive = new byte[12];

                // Turn keepalive on
                Buffer.BlockCopy(BitConverter.GetBytes((uint)1), 0, keepAlive, 0, 4);

                // Set TCP keepalive time
                Buffer.BlockCopy(BitConverter.GetBytes((uint)_keepalive.TcpKeepAliveTime), 0, keepAlive, 4, 4);

                // Set TCP keepalive interval
                Buffer.BlockCopy(BitConverter.GetBytes((uint)_keepalive.TcpKeepAliveInterval), 0, keepAlive, 8, 4);

                // Set keepalive settings on the underlying Socket
                client.Client.IOControl(IOControlCode.KeepAliveValues, keepAlive, null);

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
