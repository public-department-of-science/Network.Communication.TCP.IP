using System;
using System.Threading.Tasks;

namespace TCP.IP.Communication.Client
{
    public partial class TcpClient
    {
        private async Task IdleServerMonitor()
        {
            while (!_token.IsCancellationRequested)
            {
                await Task.Delay(_settings.IdleServerEvaluationIntervalMs, _token).ConfigureAwait(false);
                if (_settings.IdleServerTimeoutMs == 0)
                {
                    continue;
                }

                DateTime timeoutTime = _lastActivity.AddMilliseconds(_settings.IdleServerTimeoutMs);
                if (DateTime.UtcNow > timeoutTime)
                {
                    Logger?.Invoke($"{_header}disconnecting from {ServerIpPort} due to timeout");
                    _isConnected = false;
                    _isTimeout = true;
                    _tokenSource.Cancel(); // DataReceiver will fire events including dispose
                }
            }
        }

        private async Task ConnectedMonitor()
        {
            while (!_token.IsCancellationRequested)
            {
                await Task.Delay(_settings.ConnectionLostEvaluationIntervalMs, _token).ConfigureAwait(false);
                if (!_isConnected)
                {
                    continue; //Just monitor connected clients
                }

                if (!PollSocket())
                {
                    Logger?.Invoke($"{_header}disconnecting from {ServerIpPort} due to connection lost");
                    _isConnected = false;
                    _tokenSource.Cancel(); // DataReceiver will fire events including dispose
                }
            }
        }
    }
}
