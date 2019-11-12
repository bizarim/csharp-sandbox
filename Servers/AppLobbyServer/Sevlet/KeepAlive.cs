using System;

namespace AppLobbyServer.Sevlet
{
    public class KeepAlive : ClientRequest
    {
        private LibCommon.Protocol.Lobby.KeepAliveRequest param = null;
        private LibCommon.Protocol.Lobby.KeepAliveResponse rtParam = new LibCommon.Protocol.Lobby.KeepAliveResponse();

        public override void MakeParameter(string json)
        {
            //Assembly encryptionAssembly = Assembly.LoadFrom("NbCommon.dll");
            //string typeName = string.Format("NbCommon.DataTables.{0}", fileName.Split('.')[0]);
            //Type requestType = encryptionAssembly.GetType(typeName);
            //if (null == requestType)
            //{
            //    throw new Exception("-1");
            //}

            //var dt = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstr, requestType);

            param = Newtonsoft.Json.JsonConvert.DeserializeObject<LibCommon.Protocol.Lobby.KeepAliveRequest>(json);
            if (null == param) throw new Exception("KeepAlive InvalidParameter");
        }

        public override void Process(SuperWebSocket.WebSocketSession session, int reqpn)
        {
            var rt = ProcessEvent();
            if (LibCommon.Enums.ErrorCode.Success != rt)
            {
                SendCommonError(session, reqpn, rt);
            }
            else
            {
                SendResponse(session, (int)LibCommon.Protocol.Lobby.LobbyProtocolType.KeppAliveResponse, rtParam);
            }
        }

        protected LibCommon.Enums.ErrorCode ProcessEvent()
        {
            //if (false == isValidSessionKey(param.uid, param.sKey)) return LibCommon.Enums.ErrorType.ExpireSession;

            return LibCommon.Enums.ErrorCode.Success;
        }
    }
}
