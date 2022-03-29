﻿namespace TCP.Enums
{
    /// <summary>
    /// Reason why a client disconnected.
    /// </summary>
    public enum ConnectionStatus
    {
        /// <summary>
        /// Impossible to get connection status.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Client connection timed out; server did not receive data within the timeout window.
        /// </summary>
        NoResponse_Timeout,

        /// <summary>
        /// Client connection established correctly
        /// </summary>
        ConnectToServerOK,
    }
}