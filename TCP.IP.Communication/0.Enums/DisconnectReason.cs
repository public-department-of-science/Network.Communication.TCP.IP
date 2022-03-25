namespace TCP.Enums
{
    /// <summary>
    /// Reason why a client disconnected.
    /// </summary>
    public enum DisconnectReason
    {
        /// <summary>
        /// Impossible to get right disconnection reason.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Normal disconnection.
        /// </summary>
        DisconnectedByClient,

        /// <summary>
        /// Client connection was intentionally terminated programmatically or by the server.
        /// </summary>
        KickedByServer,

        /// <summary>
        /// Client connection timed out; server did not receive data within the timeout window.
        /// </summary>
        NoResponseTimeout,
    }
}