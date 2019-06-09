using System;

namespace appLobbyServer.ClientRequest
{
    /// <summary>
    /// Heart Beat
    /// </summary>
    public partial class KeepAlive : ClientRequest
    {
        DLL_Common.Protocol.Lobby.PT_LOBBY_KEEPALIVE_RQ param = null;
        DLL_Common.Protocol.Lobby.PT_LOBBY_KEEPALIVE_RS rtParam = new DLL_Common.Protocol.Lobby.PT_LOBBY_KEEPALIVE_RS();
        
        public override void MakeParameter(string json)
        {
            param = Newtonsoft.Json.JsonConvert.DeserializeObject<DLL_Common.Protocol.Lobby.PT_LOBBY_KEEPALIVE_RQ>(json);
            if (null == param) throw new Exception("KeepAlive InvalidParameter");
        }

        public override void Process(SuperWebSocket.WebSocketSession session, int reqpn)
        {
            var rt = ProcessEvent();
            if (DLL_Common.Enums.eErrorCode.Success != rt)
            {
                SendCommonError(session, reqpn, rt);
            }
            else
            {
                SendResponse(session, (int)DLL_Common.Protocol.Lobby.LobbyProtocolType.Response_KeepAlive, rtParam);
            }
        }

        protected DLL_Common.Enums.eErrorCode ProcessEvent()
        {
            if (false == isValidSessionKey(param.uid, param.sKey)) return DLL_Common.Enums.eErrorCode.ExpireSession;

            return DLL_Common.Enums.eErrorCode.Success;
        }
    }
}
