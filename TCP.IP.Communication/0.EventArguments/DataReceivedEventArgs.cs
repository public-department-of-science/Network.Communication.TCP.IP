using System;

namespace TCP.EventArguments
{
    /// <summary>
    /// Arguments for data received from connected endpoints.
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// The IP address and port number of the connected endpoint.
        /// </summary>
        public string IpPort { get; }

        /// <summary>
        /// The data received from the client.
        /// </summary>
        public byte[] Data { get; }
  
        internal DataReceivedEventArgs(string ipPort, byte[] data)
        {
            IpPort = ipPort;
            Data = data;
        }
    }
}