using SuperSocket.SocketBase;
using SuperWebSocket;

namespace AppLobbyServer.Server
{
    public class LobbyHandler : LibServerCommon.Server.IServerHandler
    {

        public void NewDataReceived(WebSocketSession session, byte[] datas)
        {
            var msg = Dispatcher.DispatcherMessage.Create<LibServerCommon.Message.IMessage>(session, datas);
        }

        public void NewMessageReceived(WebSocketSession session, string message)
        {
            session.Close();
        }

        public void NewSessionConnected(WebSocketSession session)
        {
            session.AppServer.Logger.InfoFormat("Connected [{0}],[{1}],[{2}]", session.AppServer.SessionCount, session.SessionID, session.RemoteEndPoint.ToString());
        }

        public void SessionClosed(WebSocketSession session, CloseReason closereason)
        {
        }
    }
}
