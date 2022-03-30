using System.Net.Sockets;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        private bool IsClientConnected(System.Net.Sockets.TcpClient client)
        {
            if (!client.Connected)
            {
                return false;
            }

            if (client.Client.Poll(0, SelectMode.SelectWrite) && (!client.Client.Poll(0, SelectMode.SelectError)))
            {
                byte[] buffer = new byte[1];
                if (client.Client.Receive(buffer, SocketFlags.Peek) == 0)
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
