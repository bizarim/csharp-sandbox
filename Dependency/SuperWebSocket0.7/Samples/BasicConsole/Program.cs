using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;

namespace SuperWebSocket.Samples.BasicConsole
{
    class Program
    {
        public static int cnt = 0;
        public static object sync = new object();
        public static SuperSocket.SocketBase.Logging.ILog appServerLogger;
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start the WebSocketServer!");

            Console.ReadKey();
            Console.WriteLine();

            var appServer = new WebSocketServer();

            SuperSocket.SocketBase.Config.ServerConfig sc = new SuperSocket.SocketBase.Config.ServerConfig();
            sc.MaxConnectionNumber = 40000;
            sc.Port = 2012;


            //Setup the appServer
            if (!appServer.Setup(sc)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            appServerLogger = appServer.Logger;

            // 문자열로 오는 메시지에 대한 리시빙 완료 핸들러
            appServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(appServer_NewMessageReceived);
            appServer.NewDataReceived += new SessionHandler<WebSocketSession,byte[]>(appServer_NewDataReceived);
            
            // 새 연결이 왔다.
            appServer.NewSessionConnected += new SessionHandler<WebSocketSession>(appServer_NewSessionConnected);
            // 기존 연결이 끊겼다.
            appServer.SessionClosed += new SessionHandler<WebSocketSession,CloseReason>(appServer_SessionClosed);

            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine();
            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }

        static void appServer_NewMessageReceived(WebSocketSession session, string message)
        {
            //Send the received message back
            session.Send("Server: " + message);
            
        }

        static void appServer_NewDataReceived(WebSocketSession session, byte[] datas)
        {
            session.Send(datas, 0, datas.Length);
        }

        static void appServer_NewSessionConnected(WebSocketSession session)
        {
            // 웹방식에선 정수로된 managed socket id값 같은것을 식별자로 쓸수 없고, GUID처럼 긴문자열로 된 값을 식별자로 써야함
            lock (sync)
            {
                cnt++;
                Console.WriteLine("session add [{0}]", cnt);
            }
        }
        static void appServer_SessionClosed(WebSocketSession session, CloseReason closereason)
        {
            lock (sync)
            {
                cnt--;
                Console.WriteLine("session del [{0}]", cnt);
            }
        }
    }
}
