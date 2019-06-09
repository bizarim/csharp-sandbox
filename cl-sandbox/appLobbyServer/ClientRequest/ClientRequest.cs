using SuperWebSocket;
using System;
using System.Text;

namespace appLobbyServer.ClientRequest
{
    /// <summary>
    /// 클라이언트에서 오는 요청 처리
    /// </summary>
    public partial class ClientRequest
    {
        /// <summary>
        /// 요청의 고유 번호 생성
        /// </summary>
        public string GUID = Guid.NewGuid().ToString();

        /// <summary>
        /// 에러코드
        /// </summary>
        protected DLL_Common.Enums.eErrorCode rtcode = DLL_Common.Enums.eErrorCode.Success;

        /// <summary>
        /// 메시지 parse
        /// </summary>
        /// <param name="json"></param>
        public virtual void MakeParameter(string json)
        {
        }
        /// <summary>
        /// 처리
        /// </summary>
        /// <param name="context"></param>
        public virtual void Process(WebSocketSession session, int reqpn)
        {
        }

        /// <summary>
        /// 에러 response 응답
        /// </summary>
        /// <param name="session"></param>
        /// <param name="reqpn"></param>
        /// <param name="rt"></param>
        protected void SendCommonError(WebSocketSession session, int reqpn, DLL_Common.Enums.eErrorCode rt)
        {
            rtcode = rt;

            DLL_Common.Protocol.Lobby.CommonErrorResponse er = new DLL_Common.Protocol.Lobby.CommonErrorResponse();
            er.reqn = reqpn;
            er.rt = (int)rt;

            byte[] buffer = new byte[4 + 4 + 1024];
            byte[] _data = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(er));
            BitConverter.GetBytes((int)DLL_Common.Protocol.Lobby.LobbyProtocolType.ErrorResponse).CopyTo(buffer, 0 + 4);
            _data.CopyTo(buffer, 4 + 4);

            session.Send(buffer, 0, 4 + 4 + _data.Length);
        }

        /// <summary>
        /// reponse 응답
        /// </summary>
        /// <param name="session"></param>
        /// <param name="pn"></param>
        /// <param name="resultparam"></param>
        protected void SendResponse(WebSocketSession session, int pn, object resultparam)
        {
            byte[] buffer = new byte[4 + 4 + 2048000];
            byte[] _data = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(resultparam));

            BitConverter.GetBytes(pn).CopyTo(buffer, 0 + 4);
            _data.CopyTo(buffer, 4 + 4);

            session.Send(buffer, 0, 4 + 4 + _data.Length);
        }


        // 로비에서 유저에게 notice 보내기
        protected int SendNotice(WebSocketSession session, int type, object param)
        {
            byte[] _sendBuffer = new byte[4 + 4 + 1024000];
            byte[] _data = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(param));
            BitConverter.GetBytes(type).CopyTo(_sendBuffer, 0 + 4);
            _data.CopyTo(_sendBuffer, 4 + 4);

            try
            {
                if (null == session) return 0;
                session.Send(_sendBuffer, 0, 4 + 4 + _data.Length);
                LobbyServer.theLogger.DebugFormat("Send {0} {1}", type, session.SessionID);
            }
            catch (Exception ex)
            {
                LobbyServer.theLogger.Error(string.Format("SendNotice exception {0}", ex.ToString()));
            }

            return 0;
        }

        /// <summary>
        /// 세션키 검사
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        protected bool isValidSessionKey(long uid, string sKey)
        {
            return false;
        }


        /// <summary>
        /// 로그 남기기
        /// </summary>
        public void PostCenterControlLogging()
        {
        }
    }
}
