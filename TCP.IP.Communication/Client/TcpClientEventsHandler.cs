using System;
using TCP.EventArguments;

namespace TCP.Client
{
    /// <summary>
    /// Tcp client events handler.
    /// </summary>
    public class TcpClientEventsHandler
    {
        /// <summary>
        /// Event to call when the connection is established.
        /// </summary>
        public event EventHandler<ConnectionEventArgs> Connected;

        /// <summary>
        /// Event to call when the connection is destroyed.
        /// </summary>
        public event EventHandler<DisconnectionEventArgs> Disconnected;

        /// <summary>
        /// Event to call when byte data has become available from the server.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        internal void HandleConnected(object sender, ConnectionEventArgs args)
        {
            Connected?.Invoke(sender, args);
        }

        internal void HandleClientDisconnected(object sender, DisconnectionEventArgs args)
        {
            Disconnected?.Invoke(sender, args);
        }

        internal void HandleDataReceived(object sender, DataReceivedEventArgs args)
        {
            DataReceived?.Invoke(sender, args);
        }
    }
}
