using System;
using System.Collections.Generic;
using System.Text;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        /// <summary>
        /// Retrieve a list of client IP:port connected to the server.
        /// </summary>
        /// <returns>IEnumerable of strings, each containing client IP:port.</returns>
        public IEnumerable<string> GetClients()
        {
            List<string> clients = new List<string>(_clients.Keys);
            return clients;
        }
    }
}
