using System;
using TCP.Client;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        public void DisconnectAllClients()
        {
            foreach (var clientInfo in _clients)
            {
                DisconnectClient(clientInfo.Key);
            }
        }

        /// <summary>
        /// Disconnects the specified client.
        /// </summary>
        /// <param name="ipPort">IP:port of the client.</param>
        public void DisconnectClient(string ipPort)
        {
            if (string.IsNullOrWhiteSpace(ipPort))
            {
                throw new ArgumentNullException(nameof(ipPort));
            }

            if (!_clients.TryGetValue(ipPort, out ClientMetadata client))
            {
                Logger?.Invoke($"{_header}unable to find client: {ipPort}");
            }
            else
            {
                if (!_clientsTimedout.ContainsKey(ipPort))
                {
                    Logger?.Invoke($"{_header}kicking: {ipPort}");
                    _clientsKicked.TryAdd(ipPort, DateTime.UtcNow);
                }
            }

            if (client != null)
            {
                if (!client.TokenSource.IsCancellationRequested)
                {
                    client.TokenSource.Cancel();
                    Logger?.Invoke($"{_header}requesting disposal of: {ipPort}");
                }

                client.Dispose();
            }
        }
    }
}
