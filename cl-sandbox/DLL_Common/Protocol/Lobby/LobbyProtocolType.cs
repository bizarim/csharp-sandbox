namespace DLL_Common.Protocol.Lobby
{
    /// <summary>
    /// 로비 프로토콜 타입
    /// </summary>
    public enum LobbyProtocolType
    {
        ErrorResponse = 1,

        Request_KeepAlive,
        Response_KeepAlive,
        
    }
}