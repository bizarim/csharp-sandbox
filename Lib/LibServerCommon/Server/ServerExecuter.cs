namespace LibServerCommon.Server
{
    public static class ServerExecuter<T> where T : IServer, new()
    {
        public static void Start(string[] args)
        {
            T server = new T();

            if (false == server.LoadConfig(args))
                return;

            if (false == server.LoadData())
                return;

            if (false == server.Initialize())
                return;

            server.Start();
        }
    }
}
