using System;
using TCP.EventArguments;

namespace TCP.Server
{
    /// <summary>
    /// Tcp server events.
    /// </summary>
    public class TcpServerEventsHandler
    {
        /// <summary>
        /// Event to call when a client connects.
        /// </summary>
        public event EventHandler<ConnectionEventArgs> ClientConnected;

        /// <summary>
        /// Event to call when a client disconnects.
        /// </summary>
        public event EventHandler<DisconnectionEventArgs> ClientDisconnected;

        /// <summary>
        /// Event to call when byte data has become available from the client.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        internal void HandleClientConnected(object sender, ConnectionEventArgs args)
        {
            ClientConnected?.Invoke(sender, args);
        }

        internal void HandleClientDisconnected(object sender, DisconnectionEventArgs args)
        {
            ClientDisconnected?.Invoke(sender, args);
        }

        internal void HandleDataReceived(object sender, DataReceivedEventArgs args)
        {
            DataReceived?.Invoke(sender, args);
        }
    }
}
