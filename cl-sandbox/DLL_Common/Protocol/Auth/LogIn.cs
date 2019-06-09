namespace DLL_Common.Protocol.Auth
{
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

    public partial class PT_AUTH_LOGIN_RQ
    {
        /// <summary>
        /// 인증타입(계정유형)
        /// 1: guest
        /// 2: facebook
        /// 3: ios game center
        /// 4: google play
        /// </summary>
        public int cdType;
        /// <summary>
        /// 인증유형에 따른 세부 인증정보 JSON 스트링을 Base64 로 인코딩
        /// </summary>
        public string cdToken;
        public string appVer;
    }

    public partial class PT_AUTH_LOGIN_RS : Response
    {
        /// <summary>
        /// 인증타입(계정유형)
        /// 1: guest
        /// 2: facebook
        /// 3: ios game center
        /// 4: google play
        /// </summary>
        public int cdType;

        /// <summary>
        /// 계정당 부여되는 고유번호 (아주아주 중요함) 계정에 대해선 변경되지 않음
        /// </summary>
        public long UUID;

        /// <summary>
        /// 인증세션값
        /// 이값은 모든 요청시마다 던져줘야한다.
        /// 이값을 통해 중복로그인등의 세션의 만료여부를 체크하기 때문이다.
        /// </summary>
        public string sKey;

    }

}
