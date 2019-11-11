using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
