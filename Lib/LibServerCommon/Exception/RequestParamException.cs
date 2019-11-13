
namespace LibServerCommon.Exception
{
    public class RequestParamException : System.Exception
    {
        public RequestParamException()
        {
        }

        public RequestParamException(string message)
            : base(message)
        {
        }

        public RequestParamException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
