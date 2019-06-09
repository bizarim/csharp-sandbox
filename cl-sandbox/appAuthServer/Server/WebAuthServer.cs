using System;
using DLL_SvrCommon;
using log4net;
using System.Collections.Generic;
using System.Net;
using System.Threading;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "./Config/log4net.config", Watch = true)]

namespace appAuthServer
{
    public class WebAuthServer : IServer
    {
        public static AuthServerConfig Config = new AuthServerConfig();
        public static ILog theLogger = LogManager.GetLogger("Auth");

        private HttpListener listener;
        // private string baseFolder = string.Empty;

        public void Uninitialize()
        {
            // todo
        }
        public bool Initialize()
        {
            // todo
            return true;
        }

        public bool LoadConfig(string[] args)
        {
            try
            {
                using (var io = new System.IO.StreamReader("config.json"))
                {
                    string str = io.ReadToEnd();
                    Config = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthServerConfig>(str);
                }
            }
            catch //(Exception ex)
            {
                Console.WriteLine("config 파일을 찾을수 없습니다");
                Thread.Sleep(1000);
                return false;
            }
            return true;
        }

        public bool LoadData() { return true; }

        public void Start()
        {
            // netsh http add urlacl url=http://*:8081/ user=username
            //  + 가 아니라 * 를 명시해줘야함, 둘을 동시에 쓰면 클라측에 503 에러가 통보됨
            List<string> uriList = new List<string>();
            uriList.Add(Config.url);

            listener = new HttpListener();
            foreach (var n in uriList)
            {
                listener.Prefixes.Add(n);
            }

            listener.Start();
            
            while (true)
            {
                try
                {
                    HttpListenerContext request = listener.GetContext();
                    ThreadPool.QueueUserWorkItem(ProcessRequest, request);
                }
                catch (HttpListenerException) { break; }
                catch (InvalidOperationException) { break; }
                catch
                {
                    // todo 로그
                }
            }

            Console.WriteLine("AuthServer Started");
        }

        public void Stop()
        {
            listener.Stop();
        }

        /// <summary>
        /// 워커쓰레딩으로 처리, 1커넥션당 1개, 실제 활성화는 ThreadPool 기법에 의해 관리
        /// </summary>
        /// <param name="listenerContext"></param>
        void ProcessRequest(object listenerContext)
        {
            var context = (HttpListenerContext)listenerContext;
            var request = context.Request;
            context.Response.ContentType = "application/json";

            try
            {

                using (var sr = new System.IO.StreamReader(request.InputStream))
                {
                    string reqdata = sr.ReadToEnd();

                    ClientRequest.ClientRequest cmd = null;

                    switch (request.RawUrl)
                    {
                        // todo protocol dispatcher
                        case "/LogIn": cmd = new ClientRequest.LogIn(); break;
                    }

                    if (null != cmd)
                    {
                        cmd.ProcessRequest(reqdata, context);
                        cmd.PostCenterControlLogging();
                    }
                    else
                    {

                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        context.Response.OutputStream.Close();
                    }

                }

            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.OutputStream.Close();
            }
        }
    }

}
