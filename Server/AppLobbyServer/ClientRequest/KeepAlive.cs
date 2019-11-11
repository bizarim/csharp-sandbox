using System;

namespace AppLobbyServer.ClientRequest
{
    public class KeepAlive : ClientRequest
    {
        LibCommon.Protocol.Lobby.KeepAliveRequest param = null;
        LibCommon.Protocol.Lobby.KeepAliveResponse rtParam = new LibCommon.Protocol.Lobby.KeepAliveResponse();

        public override void MakeParameter(string json)
        {
            param = Newtonsoft.Json.JsonConvert.DeserializeObject<LibCommon.Protocol.Lobby.KeepAliveRequest>(json);
            if (null == param) throw new Exception("KeepAlive InvalidParameter");
        }

        public override void Process(SuperWebSocket.WebSocketSession session, int reqpn)
        {
            var rt = ProcessEvent();
            if (LibCommon.Enums.ErrorType.Success != rt)
            {
                SendCommonError(session, reqpn, rt);
            }
            else
            {
                SendResponse(session, (int)LibCommon.Protocol.Lobby.LobbyProtocolType.KeppAliveResponse, rtParam);
            }
        }

        protected LibCommon.Enums.ErrorType ProcessEvent()
        {
            //if (false == isValidSessionKey(param.uid, param.sKey)) return LibCommon.Enums.ErrorType.ExpireSession;

            return LibCommon.Enums.ErrorType.Success;
        }
    }
}
