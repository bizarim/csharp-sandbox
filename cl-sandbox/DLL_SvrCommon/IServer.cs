namespace DLL_SvrCommon
{
    public interface IServer
    {
        bool LoadConfig(string[] args);
        bool LoadData();

        bool Initialize();
        void Uninitialize();

        void Start();
        void Stop();

    }
}
