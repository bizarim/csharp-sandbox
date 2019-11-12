using LibServerCommon.Server;
using System;
using SuperSocket.SocketBase;
using SuperWebSocket;
using System.Reflection;

namespace AppLobbyServer.Server
{
    public class LobbyServer : IServer
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(LobbyServer));

        public LobbyServerConfig config = new LobbyServerConfig();

        public WebSocketServer webSocketServer = new WebSocketServer();

        public bool Initialize() {
            // register ioc
            DispatcherMessage.Register<Sevlet.KeepAlive>((int)LibCommon.Protocol.Lobby.LobbyProtocolType.KeppAliveRequest);
            return true;
        }

        public bool LoadConfig(string[] args) { return true; }

        public bool LoadData() { return true; }

        public void Start()
        {

            if (false == this.webSocketServer.Setup(new SuperSocket.SocketBase.Config.ServerConfig
            {
                MaxConnectionNumber = 40000,
                Port = 10230,
                IdleSessionTimeOut = 60 * 5,
                ClearIdleSession = true,
                ClearIdleSessionInterval = 10
            })) // Setup with listening port
            {
                logger.Error("Failed to setup!");
                Console.ReadKey();
                System.Threading.Thread.Sleep(5000);
                return;
            }

            // handler
            var handler = new LobbyHandler();
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

            logger.Info("LobbyServer Started");

            // 우아하지 않게
            while (true) System.Threading.Thread.Sleep(1000);
        }

        public void Stop() { }

        public void Uninitialize() { }
    }
}
