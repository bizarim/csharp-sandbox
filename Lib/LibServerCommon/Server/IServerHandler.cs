using SuperSocket.SocketBase;
using SuperWebSocket;

namespace LibServerCommon.Server
{
    public interface IServerHandler
    {
        void NewDataReceived(WebSocketSession session, byte[] datas);
        void NewMessageReceived(WebSocketSession session, string message);
        void NewSessionConnected(WebSocketSession session);
        void SessionClosed(WebSocketSession session, CloseReason closereason);
    }
}
