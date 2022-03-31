using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TCP.IP.Communication.Server
{
    public partial class TcpServer
    {
        private async Task IdleClientMonitor()
        {
            while (!_token.IsCancellationRequested)
            {
                await Task.Delay(_settings.IdleClientEvaluationIntervalMs, _token).ConfigureAwait(false);
                if (_settings.IdleClientTimeoutMs == 0)
                {
                    continue;
                }

                try
                {
                    DateTime idleTimestamp = DateTime.UtcNow.AddMilliseconds(-1.0 * _settings.IdleClientTimeoutMs);
                    foreach (KeyValuePair<string, DateTime> curr in _clientsLastSeen)
                    {
                        if (curr.Value < idleTimestamp)
                        {
                            _clientsTimedout.TryAdd(curr.Key, DateTime.UtcNow);
                            Logger?.Invoke($"{_header} disconnecting {curr.Key} due to timeout");
                            DisconnectClient(curr.Key);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger?.Invoke($"{_header}monitor exception: {e}");
                }
            }
        }
    }
}
