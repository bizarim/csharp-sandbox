using System;
using System.Net;

namespace appAuthServer.ClientRequest
{
    /// <summary>
    /// 클라이언트에서 오는 요청 처리
    /// </summary>
    public abstract class ClientRequest
    {
        /// <summary>
        /// 요청의 고유 번호 생성
        /// </summary>
        public string GUID = Guid.NewGuid().ToString();

        /// <summary>
        /// 메시지 parse
        /// </summary>
        /// <param name="json"></param>
        protected abstract void MakeParameter(string json);
        /// <summary>
        /// 처리
        /// </summary>
        /// <param name="context"></param>
        protected abstract void Process(HttpListenerContext context);

        /// <summary>
        /// 예외 처리
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        public virtual void ProcessException(Exception ex, HttpListenerContext context)
        {
            DLL_Common.Protocol.Auth.Response rh = new DLL_Common.Protocol.Auth.Response();
            rh.rt = (int)DLL_Common.Enums.eErrorCode.Exception;
            rh.errMsg = ex.Message;

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(rh));
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        /// <summary>
        /// 요청 처리
        /// </summary>
        /// <param name="reqdata"></param>
        /// <param name="context"></param>
        public void ProcessRequest(string reqdata, HttpListenerContext context)
        {
            try
            {
                MakeParameter(reqdata);     // parse
                Process(context);           // process
            }
            catch (System.OutOfMemoryException)
            {
                // 메모리 부족은 무조건 서버를 죽여서 새로 올라오게 하는것이 맞다.
                Environment.Exit(100);
            }
            catch (Exception ex)
            {
                ProcessException(ex, context);  // 예외처리
            }
        }

        /// <summary>
        /// 로그 처리
        /// </summary>
        public void PostCenterControlLogging()
        {
            // todo logging
        }

        /// <summary>
        /// 메시지 보내기
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ro"></param>
        protected void Send(HttpListenerContext context, object ro)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(ro);
            WebAuthServer.theLogger.DebugFormat("Send {0} {1} {2}", GetType().Name, context.Request.RemoteEndPoint.ToString(), json);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();

        }
    }
}
