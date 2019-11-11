namespace LibCommon.Protocol.Base
{
    public class Response
    {
        /// <summary>
        /// HTTP 방식은 요청-응답 방식이라 실패 프로토콜을 따로 만들수 없으니
        /// 0: 성공
        /// 1~????: 각종 에러코드
        /// </summary>
        public int code;

        /// <summary>
        /// 실패했을때 세부 에러정보
        /// </summary>
        public string msg;

    }
}
