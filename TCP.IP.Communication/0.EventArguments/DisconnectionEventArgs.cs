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
        public DisconnectionReason Reason { get; }

        internal DisconnectionEventArgs(string ipPort, DisconnectionReason reason )
        {
            IpPort = ipPort;
            Reason = reason;
        }
    }
}