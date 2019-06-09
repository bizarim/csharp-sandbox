
namespace appLobbyServer
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            DLL_SvrCommon.ServerExecuter<LobbyServer>.Start(args);
        }
    }
}
