using DLL_Common.Enums;
using System;
using System.Net;

namespace appAuthServer.ClientRequest
{
    /// <summary>
    /// 로그인 요청
    /// </summary>
    public partial class LogIn : ClientRequest
    {
        DLL_Common.Protocol.Auth.PT_AUTH_LOGIN_RQ param;
        DLL_Common.Protocol.Auth.PT_AUTH_LOGIN_RS rtParam = new DLL_Common.Protocol.Auth.PT_AUTH_LOGIN_RS();

        protected override void MakeParameter(string json)
        {
            param = Newtonsoft.Json.JsonConvert.DeserializeObject<DLL_Common.Protocol.Auth.PT_AUTH_LOGIN_RQ>(json);
            if (null == param) throw new Exception("LogIn Protocol_InvalidParameter");
        }

        protected override void Process(HttpListenerContext context)
        {
            eErrorCode err = eErrorCode.Success;
            rtParam.rt = (int)err;
            rtParam.errMsg = err.ToString();

            Send(context, rtParam);
        }
    }
}
