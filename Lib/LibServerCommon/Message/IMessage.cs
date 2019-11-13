namespace LibServerCommon.Message
{

    public interface IMessage
    {
        void Initialize(SuperWebSocket.WebSocketSession ws, Network.Buffer buffer);
        void Process();
        void Release();
    }

    public abstract class AbsMessage : IMessage
    {
        public string GUID = System.Guid.NewGuid().ToString();
        protected Network.Sender sender = new Network.Sender();
        protected Network.Buffer buffer;
        protected SuperWebSocket.WebSocketSession session;

        public void Initialize(SuperWebSocket.WebSocketSession ws, Network.Buffer buffer)
        {
            this.session = ws;
            this.buffer = buffer;
        }

        protected abstract void Pre();
        protected abstract void Execute();
        protected abstract void Post();
        protected abstract void PostRelease();

        private void PreRelease()
        {
            sender = null;
            buffer = null;
            session = null;
        }

        public void Process()
        {
            Pre();
            Execute();
            Post();
        }

        public void Release() {
            PreRelease();
            PostRelease();
        }
    }
}
