namespace TCP.IP.Communication.Client
{
    public partial class TcpClient
    {
        /// <summary>
        /// Disconnect from the server.
        /// </summary>
        public void Disconnect()
        {
            if (!IsConnected)
            {
                Logger?.Invoke($"{_header}already disconnected");
                return;
            }

            Logger?.Invoke($"{_header}disconnecting from {ServerIpPort}");

            _tokenSource.Cancel();
            // _dataReceiver.Wait();
            _client.Close();
            _isConnected = false;
        }
    }
}
