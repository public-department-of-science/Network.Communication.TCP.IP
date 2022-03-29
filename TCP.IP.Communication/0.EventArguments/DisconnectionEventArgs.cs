using System;
using TCP.Enums;

namespace TCP.EventArguments
{
    /// <summary>
    /// Arguments for connection events.
    /// </summary>
    public class DisconnectionEventArgs : EventArgs
    {
        /// <summary>
        /// The IP address and port number of the connected peer socket.
        /// </summary>
        public string IpPort { get; }

        /// <summary>
        /// The reason for the disconnection, if any.
        /// </summary>
        public DisconnectionStatus Reason { get; }

        internal DisconnectionEventArgs(string ipPort, DisconnectionStatus reason = DisconnectionStatus.Undefined)
        {
            IpPort = ipPort;
            Reason = reason;
        }
    }
}