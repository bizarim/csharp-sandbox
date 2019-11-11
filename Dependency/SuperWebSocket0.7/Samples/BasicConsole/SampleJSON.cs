using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperWebSocket.Samples.BasicConsole
{
    public interface BusinessLogicItem
    {
        void Process();

    }
    public partial class LogInResult : BusinessLogicItem
    {
        public long uid;
        public int result;
        public string user_token;

        public void Process()
        {
        }

    }

    public partial class LogInRequest
    {
        public string token;
        public int auth_type;
    }

    class SampleJSON
    {
        public void Test_MSGHandle(string receivedMessage)
        {
            // receivedMessage는 "cmdname§jsondata" 형태로 들어가 있음
            // 실제 리시빙처리가 완료되고 난뒤 호출

            // 구분자는 되도록이면 잘쓰지 않는 유니코드를 쓰는게 좋다
            //      -||- 이렇게 문자열 단위로 split이 가능하다면 문자가 아닌 문자열 단위로 청크(토큰) 구분을 하는게 좋다.
            string[] tokens = receivedMessage.Split('§');
            if (2 != tokens.Length)
            {
                // invalid protocol expression
                return;
            }

            // cmdname 에 대응하는 클래스의 인스턴스로 생성해서 쓴다.
            object msgObj = null;
            switch (tokens[0])
            {
                case "LogInResult": msgObj = Newtonsoft.Json.JsonConvert.DeserializeObject<LogInResult>(tokens[1]); break;
            }

            //if(null != msgObj) some_businesslogic_queue.Enqueue(msgObj);
        }

        public void Test_LogInRequest(LogInRequest param)
        {
            string strdata = string.Format("LogInRequest§{0}", Newtonsoft.Json.JsonConvert.SerializeObject(param));

            //some_websocketcommunicator.Send(strdata);
        }

    }
}
