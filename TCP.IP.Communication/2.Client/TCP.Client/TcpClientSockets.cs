using System.Net.Sockets;

namespace TCP.IP.Communication.Client
{
    public partial class TcpClient
    {
        private bool PollSocket()
        {
            try
            {
                if (_client.Client == null || !_client.Client.Connected)
                {
                    return false;
                }

                /* pear to the documentation on Poll:
                 * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                 * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                 * -or- true if data is available for reading; 
                 * -or- true if the connection has been closed, reset, or terminated; 
                 * otherwise, returns false
                 */
                if (!_client.Client.Poll(0, SelectMode.SelectRead))
                {
                    return true;
                }

                var buff = new byte[1];
                var clientSentData = _client.Client.Receive(buff, SocketFlags.Peek) != 0;
                return clientSentData; //False here though Poll() succeeded means we had a disconnect!
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }
}
