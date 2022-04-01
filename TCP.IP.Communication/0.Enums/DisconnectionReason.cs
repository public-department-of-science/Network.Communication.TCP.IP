namespace TCP.Enums
{
    public enum DisconnectionReason
    {
        Undefined = 0,
        DisconnectOK_ByClient,
        NoResponse_Timeout,
        KickedOut_ByServer,
    }
}