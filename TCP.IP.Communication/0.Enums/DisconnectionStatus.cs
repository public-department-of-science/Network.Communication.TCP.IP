namespace TCP.Enums
{
    /// <summary>
    /// Reason why a client disconnected.
    /// </summary>
    public enum DisconnectionStatus
    {
        /// <summary>
        /// Impossible to get disconnection reason.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Normal disconnection.
        /// </summary>
        DisconnectOK_ByClient,

        /// <summary>
        /// Client connection timed out; server did not receive data within the timeout window.
        /// </summary>
        NoResponse_Timeout,

        /// <summary>
        /// Client connection was intentionally terminated programmatically or by the server.
        /// </summary>
        KickedOut_ByServer,
    }
}