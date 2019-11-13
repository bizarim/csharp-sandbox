

namespace AppLobbyServer.Dispatcher
{
    public class DispatcherMessage : LibServerCommon.Dispatcher.IDispatcher
    {
        private static LibServerCommon.Dispatcher.IocContainer container = new LibServerCommon.Dispatcher.IocContainer();

        public static void Register<T>(int pn)
        {
            container.Register(pn, typeof(T));
        }

        public static T Create<T>(SuperWebSocket.WebSocketSession ws, byte[] datas) where T : LibServerCommon.Message.IMessage
        {
            var buffer = new LibServerCommon.Network.Converter().Deserialize(datas);
            if (false == buffer.IsInit) {
                // buffer deserialize error
                LibServerCommon.Message.IMessage err = new Message.BufferError();
                return (T)err;
            }

            var msg = container.Resolve<T>(buffer.ProtocolType);
            if (null == msg)
            {
                // no match protocol error
                LibServerCommon.Message.IMessage err = new Message.ProtocolError();
                return (T)err;
            }

            // success
            msg.Initialize(ws, buffer);
            return msg;
        }
    }
}
