namespace TCP.Enums
{
    /// <summary>
    /// Reason why a client disconnected.
    /// </summary>
    public enum ConnectionStatus
    {
        /// <summary>
        /// Impossible to get right disconnection reason.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Normal disconnection.
        /// </summary>
        DisconnectOK_ByClient,

        /// <summary>
        /// Client connection was intentionally terminated programmatically or by the server.
        /// </summary>
        KickedOut_ByServer,

        /// <summary>
        /// Client connection timed out; server did not receive data within the timeout window.
        /// </summary>
        NoResponse_Timeout,

        /// <summary>
        /// Client connection established
        /// </summary>
        ConnectToServerOK,
    }
}