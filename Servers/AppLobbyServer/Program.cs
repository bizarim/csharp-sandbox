
namespace AppLobbyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            LibServerCommon.Server.ServerExecuter<Server.LobbyServer>.Start(args);
        }
    }
}
