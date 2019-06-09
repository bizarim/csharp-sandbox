namespace DLL_Common.Protocol.Lobby
{
    public partial class Request
    {
        public long uid;
        public string sKey;
    }

    public class Response
    {
        /// <summary>
        /// HTTP 방식은 요청-응답 방식이라 실패 프로토콜을 따로 만들수 없으니
        /// 0: 성공
        /// 1~????: 각종 에러코드
        /// </summary>
        public int rt;

        /// <summary>
        /// 실패했을때 세부 에러정보
        /// </summary>
        public string errMsg;

    }

    public class CommonErrorResponse
    {
        /// <summary>
        /// RQ number
        /// </summary>
        public int reqn;
        /// <summary>
        /// HTTP 방식은 요청-응답 방식이라 실패 프로토콜을 따로 만들수 없으니
        /// 0: 성공
        /// 1~????: 각종 에러코드
        /// </summary>
        public int rt;

        /// <summary>
        /// 실패했을때 세부 에러정보
        /// </summary>
        public string errMsg;

    }
}

namespace DLL_Common.Protocol.Lobby
{

    public partial class PT_LOBBY_KEEPALIVE_RQ : Request
    {
    }

    public partial class PT_LOBBY_KEEPALIVE_RS : Response
    {
    }
}
