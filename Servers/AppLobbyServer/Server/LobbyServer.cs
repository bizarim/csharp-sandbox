using System;
using SuperSocket.SocketBase;
using SuperWebSocket;

namespace AppLobbyServer.Server
{
    public class LobbyServer : LibServerCommon.Server.IServer
    {
        // logger
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(LobbyServer));

        // handler
        protected LibServerCommon.Server.IServerHandler handler = new LobbyHandler();

        // was
        public WebSocketServer webSocketServer = new WebSocketServer();

        // ExecutorMessage
        public static Executor.ExecutorMessage executorMessage = new Executor.ExecutorMessage();

        public bool Initialize()
        {
            // register ioc
            Dispatcher.DispatcherMessage.Register<Message.HealthCheck>((int)LibCommon.Protocol.Lobby.LobbyProtocolType.HealthCheckRequest);
            return true;
        }

        public void Start()
        {
            // todo config 작업

            var sc = new SuperSocket.SocketBase.Config.ServerConfig
            {
                MaxConnectionNumber = 40000,
                Port = 10230,
                IdleSessionTimeOut = 60 * 5,
                ClearIdleSession = true,
                ClearIdleSessionInterval = 10
            };

            if (false == this.webSocketServer.Setup(sc)) // Setup with listening port
            {
                logger.Error("Failed to setup!");
                Console.ReadKey();
                System.Threading.Thread.Sleep(5000);
                return;
            }

            // handler
            this.webSocketServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(handler.NewMessageReceived);
            this.webSocketServer.NewDataReceived += new SessionHandler<WebSocketSession, byte[]>(handler.NewDataReceived);
            this.webSocketServer.NewSessionConnected += new SessionHandler<WebSocketSession>(handler.NewSessionConnected);
            this.webSocketServer.SessionClosed += new SessionHandler<WebSocketSession, CloseReason>(handler.SessionClosed);

            // Try to start the appServer
            if (false == this.webSocketServer.Start())
            {
                logger.Error("Failed to start!");
                Console.ReadKey();
                return;
            }

            executorMessage.Start();

            logger.Info("AppLobbyServer Started");
            Console.WriteLine("AppLobbyServer Started");

            // 우아하지 않게
            while (true) System.Threading.Thread.Sleep(1000);
        }

        public bool LoadConfig(string[] args)
        {
            // custom
            return true;
        }

        public bool LoadData()
        {
            // custom
            return true;
        }

        public void Stop()
        {
            // custom
        }

        public void Uninitialize()
        {
            // custom
        }
    }
}
