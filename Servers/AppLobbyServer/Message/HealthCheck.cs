
namespace AppLobbyServer.Message
{
    public class HealthCheck : LibServerCommon.Message.AbsMessage
    {
        private LibCommon.Protocol.Lobby.HealthCheckRequest reqParam = null;
        private LibCommon.Protocol.Lobby.HealthCheckResponse resParam = new LibCommon.Protocol.Lobby.HealthCheckResponse();

        protected override void Pre()
        {
            this.reqParam = Newtonsoft.Json.JsonConvert.DeserializeObject<LibCommon.Protocol.Lobby.HealthCheckRequest>(buffer.Contents);
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
                (int)LibCommon.Protocol.Lobby.LobbyProtocolType.HealthCheckResponse,
                Newtonsoft.Json.JsonConvert.SerializeObject(resParam)
            );
        }

        protected override void Post()
        {
            // custom
        }

        protected override void Release()
        {
            reqParam = null;
            resParam = null;
        }
    }
}
