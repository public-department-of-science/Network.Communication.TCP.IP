using System;
using TCP.Enums;

namespace TCP.EventArguments
{
    /// <summary>
    /// Arguments for connection events.
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// The IP address and port number of the connected peer socket.
        /// </summary>
        public string IpPort { get; }

        public ConnectionStatus Status { get; }

        internal ConnectionEventArgs(string ipPort, ConnectionStatus status)
        {
            IpPort = ipPort;
            Status = status;
        }
    }
}