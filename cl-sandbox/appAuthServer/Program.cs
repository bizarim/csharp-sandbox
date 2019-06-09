namespace appAuthServer
{
    class Program
    {
        static void Main(string[] args)
        {
            DLL_SvrCommon.ServerExecuter<WebAuthServer>.Start(args);
        }
    }
}
