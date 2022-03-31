using System;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        private void UpdateClientLastSeen(string ipPort)
        {
            if (_clientsLastSeen.ContainsKey(ipPort))
            {
                _clientsLastSeen.TryRemove(ipPort, out _);
            }

            _clientsLastSeen.TryAdd(ipPort, DateTime.UtcNow);
        }
    }
}
