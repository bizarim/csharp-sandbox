using DLL_Common.Protocol.Lobby;
using DLL_SvrCommon;
using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Text;

namespace appLobbyServer
{
    public partial class LobbyServer : IServer
    {
        void appServer_NewDataReceived(WebSocketSession session, byte[] datas)
        {
            int ver = -1;
            int pn = -1;

            try
            {
                //ver
                ver = BitConverter.ToInt32(datas, 0);
                // 프로토콜타입
                pn = BitConverter.ToInt32(datas, 0 + 4);
                // data
                string datajson = Encoding.UTF8.GetString(datas, 4 + 4, datas.Length - (4 + 4));
                // log
                session.AppServer.Logger.InfoFormat("{0} {1} {2}", session.SessionID, pn, datajson);

                ClientRequest.ClientRequest cr = null;

                switch ((LobbyProtocolType)pn)
                {
                    // todo protocol dispatcher
                    case LobbyProtocolType.Request_KeepAlive: cr = new ClientRequest.KeepAlive(); break;
                }

                if (null != cr)
                {
                    cr.MakeParameter(datajson);
                    session.AppServer.Logger.InfoFormat("{0} {1}", session.SessionID, cr.GetType().Name);
                    cr.Process(session, pn);
                    cr.PostCenterControlLogging();
                }
                else
                {
                    // N/A Protocol response
                    session.AppServer.Logger.InfoFormat("{0} {1} {2}", session.SessionID, "N/A", pn);

                    SendErrorResponse(session, new CommonErrorResponse
                    {
                        reqn = pn,
                        rt = (int)DLL_Common.Enums.eErrorCode.NAProtocol
                    });
                }
            }
            catch (Exception ex)
            {
                session.AppServer.Logger.InfoFormat("{0} exception {1}", session.SessionID, ex.Message, ex.StackTrace);

                SendErrorResponse(session, new CommonErrorResponse {
                    reqn = pn,
                    rt = (int)DLL_Common.Enums.eErrorCode.Exception,
                    errMsg = ex.Message
                });
            }
        }

        void appServer_NewMessageReceived(WebSocketSession session, string message)
        {
            // Text방식은 사용하지 않는다.
            session.Close();
        }

        void appServer_NewSessionConnected(WebSocketSession session)
        {
            // 웹방식에선 정수로된 managed socket id값 같은것을 식별자로 쓸수 없고, GUID처럼 긴문자열로 된 값을 식별자로 써야함
            session.AppServer.Logger.InfoFormat("Connected [{0}],[{1}],[{2}]", session.AppServer.SessionCount, session.SessionID, session.RemoteEndPoint.ToString());
        }

        void appServer_SessionClosed(WebSocketSession session, CloseReason closereason)
        {
        }

        void SendErrorResponse(WebSocketSession session, CommonErrorResponse er)
        {
            byte[] buffer = new byte[4 + 4 + 10240];
            byte[] _data = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(er));
            BitConverter.GetBytes((int)DLL_Common.Protocol.Lobby.LobbyProtocolType.ErrorResponse).CopyTo(buffer, 0 + 4);
            _data.CopyTo(buffer, 4 + 4);
            session.Send(buffer, 0, 4 + 4 + _data.Length);
        }
    }
}
