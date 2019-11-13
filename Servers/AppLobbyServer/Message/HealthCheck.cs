using LibCommon.Protocol.Lobby;

namespace AppLobbyServer.Message
{
    public class HealthCheck : LibServerCommon.Message.AbsMessage
    {
        private HealthCheckRequest reqParam = null;
        private HealthCheckResponse resParam = new HealthCheckResponse();

        protected override void Pre()
        {
            this.reqParam = Newtonsoft.Json.JsonConvert.DeserializeObject<HealthCheckRequest>(buffer.Contents);
            if (null == reqParam)
            {
                throw new LibServerCommon.Exception.RequestParamException("Error HealthCheckRequest Deserialize");
            }
        }

        protected override void Execute()
        {
            resParam.code = 0;
            sender.ToSuccess(
                this.session,
                (int)LobbyProtocolType.HealthCheckResponse,
                Newtonsoft.Json.JsonConvert.SerializeObject(resParam)
            );
        }

        protected override void Post()
        {
            // custom
        }

        protected override void PostRelease()
        {
            reqParam = null;
            resParam = null;
        }
    }
}
