using AppLobbyServer.Server;

namespace AppLobbyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            LibServerCommon.ServerExecuter<LobbyServer>.Start(args);
        }
    }
}
