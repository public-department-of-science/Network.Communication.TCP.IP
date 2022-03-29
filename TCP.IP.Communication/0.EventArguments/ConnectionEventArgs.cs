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

        /// <summary>
        /// The reason for the disconnection, if any.
        /// </summary>
        public ConnectionStatus Reason { get; } = ConnectionStatus.Undefined;

        internal ConnectionEventArgs(string ipPort, ConnectionStatus reason = ConnectionStatus.Undefined)
        {
            IpPort = ipPort;
            Reason = reason;
        }
    }
}