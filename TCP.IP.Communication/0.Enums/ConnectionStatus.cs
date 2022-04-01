namespace TCP.Enums
{
    public enum ConnectionStatus
    {
        Undefined = 0,
        NoResponse_Timeout,
        ConnectToServerOK,
        ConnectToServerOK_AfterRetries,
    }
}