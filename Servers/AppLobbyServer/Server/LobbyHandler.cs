using LibCommon.Protocol.Lobby;
using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Text;

namespace AppLobbyServer.Server
{
    public interface IServerHandler
    {
        void NewDataReceived(WebSocketSession session, byte[] datas);
        void NewMessageReceived(WebSocketSession session, string message);
        void NewSessionConnected(WebSocketSession session);
        void SessionClosed(WebSocketSession session, CloseReason closereason);
    }

    public class Buffer
    {
        // size
        public static readonly int BUFFER_SIZE = 10240;        // buffer size = 10240
        public static readonly int HEADER_SIZE = 8;            // header (8byte) = size (4byte) + type (4byte)

        // offset
        private static readonly int PACKET_SIZE_OFFSET = 0;      // 0 위치 읽는다.
        private static readonly int PACKET_TYPE_OFFSET = 4;      // 4 위치 읽는다.

        public int Version { get; protected set; }
        public int ProtocolType { get; protected set; }
        public string Contents { get; set; }
        public bool IsInit { get; protected set; }

        public Buffer(byte[] buffer)
        {
            try
            {
                this.Version = BitConverter.ToInt32(buffer, 0);
                this.ProtocolType = BitConverter.ToInt32(buffer, 0 + 4);
                this.Contents = Encoding.UTF8.GetString(buffer, 4 + 4, buffer.Length - (4 + 4));
                this.IsInit = true;
            }
            catch
            {
                this.IsInit = false;
            }
        }
    }

    public class Security
    {
        public void Encrypt() { }
        public string Decrypt(string contents) { return contents; }
    }

    public class Parser
    {
        public void Serialize() { }
        public Buffer Deserialize(byte[] datas)
        {
            var buffer = new Buffer(datas);
            buffer.Contents = new Security().Decrypt(buffer.Contents);
            return buffer;
        }
    }

    public class Message
    {
        public void Test() { }
        public Message(Buffer buffer) { }
    }

    public class DispatcherMessage
    {
        private static IocContainer container = new IocContainer();

        public static void Register<T>(int pn)
        {
            container.Register(pn, typeof(T));
        }

        public static object Resolve(int pn)
        {
            return (Sevlet.ClientRequest)container.Resolve(pn);
        }

        public static Sevlet.ClientRequest Create(byte[] datas)
        {
            // todo
            var buffer = new Parser().Deserialize(datas);

            // todo
            // try catch exception msg
            // null object msg
            // not return to null object
            return (Sevlet.ClientRequest)container.Resolve(buffer.ProtocolType);

            // return new Message(buffer);
        }
    }

    public class LobbyHandler : IServerHandler
    {

        public void NewDataReceived(WebSocketSession session, byte[] datas)
        {

            try
            {
                Sevlet.ClientRequest msg = DispatcherMessage.Create(datas);

                // todo
  
                if (null != msg)
                {
                    session.AppServer.Logger.InfoFormat("{0} {1}", session.SessionID, msg.GetType().Name);
                    //msg.Process(session);
                    //msg.PostCenterControlLogging();
                }
                else
                {
                    // N/A Protocol response
                    // session.AppServer.Logger.InfoFormat("{0} {1} {2}", session.SessionID, "N/A", pn);

                    //SendErrorResponse(session, new CommonErrorResponse
                    //{
                    //    reqn = pn,
                    //    rt = (int)LibCommon.Enums.ErrorType.NAProtocol
                    //});
                }
            }
            catch (Exception ex)
            {
                session.AppServer.Logger.InfoFormat("{0} exception {1}", session.SessionID, ex.Message, ex.StackTrace);

                //SendErrorResponse(session, new CommonErrorResponse
                //{
                //    reqn = pn,
                //    rt = (int)DLL_Common.Enums.eErrorCode.Exception,
                //    errMsg = ex.Message
                //});
            }
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
