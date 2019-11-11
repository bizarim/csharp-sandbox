using LibServerCommon;
using System;
using SuperSocket.SocketBase;
using SuperWebSocket;

namespace AppLobbyServer.Server
{
    public partial class LobbyServer : IServer
    {
        public static SuperSocket.SocketBase.Logging.ILog theLogger;

        public static LobbyServerConfig Config = new LobbyServerConfig();

        public WebSocketServer theWebSocketServer = null;

        public bool Initialize()
        {
            throw new NotImplementedException();
        }

        public bool LoadConfig(string[] args)
        {
            try
            {
                using (var io = new System.IO.StreamReader("config.json"))
                {
                    string str = io.ReadToEnd();
                    Config = Newtonsoft.Json.JsonConvert.DeserializeObject<LobbyServerConfig>(str);
                }
            }
            catch //(Exception ex)
            {
                Console.WriteLine("config 파일을 찾을수 없습니다");
                System.Threading.Thread.Sleep(1000);
                return false;
            }

            Player.PlayerShardDBInfo.strConnMainDb = Config.strConnMainDb;

            theWebSocketServer = new WebSocketServer();


            return true;
        }

        public bool LoadData()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            SuperSocket.SocketBase.Config.ServerConfig sc = new SuperSocket.SocketBase.Config.ServerConfig();
            sc.MaxConnectionNumber = 40000;
            sc.Port = Config.port;
            sc.IdleSessionTimeOut = 60 * 5;
            sc.ClearIdleSession = true;
            sc.ClearIdleSessionInterval = 10;

            //Setup the appServer
            if (!theWebSocketServer.Setup(sc)) //Setup with listening port
            {
                // startup error

                Console.WriteLine("Failed to setup!");
                System.Threading.Thread.Sleep(5000);
                return;
            }
            // 이 변수는 Setup() 호출뒤에 생성됨
            theLogger = theWebSocketServer.Logger;

            //appServer.NewSessionConnected += new SessionHandler<WebSocketSession>()
            // 문자열로 오는 메시지에 대한 리시빙 완료 핸들러
            theWebSocketServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(appServer_NewMessageReceived);
            theWebSocketServer.NewDataReceived += new SessionHandler<WebSocketSession, byte[]>(appServer_NewDataReceived);

            // 새 연결이 왔다.
            theWebSocketServer.NewSessionConnected += new SessionHandler<WebSocketSession>(appServer_NewSessionConnected);
            // 기존 연결이 끊겼다.
            theWebSocketServer.SessionClosed += new SessionHandler<WebSocketSession, CloseReason>(appServer_SessionClosed);

            Console.WriteLine();

            //Try to start the appServer
            if (!theWebSocketServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("LobbyServer Started");
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Uninitialize()
        {
            throw new NotImplementedException();
        }
    }
}
