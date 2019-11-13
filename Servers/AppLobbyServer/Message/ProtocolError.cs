using LibCommon.Protocol.Lobby;

namespace AppLobbyServer.Message
{
    public class ProtocolError : LibServerCommon.Message.AbsMessage
    {
        protected override void Pre()
        {
            // none
        }

        protected override void Execute()
        {
            var resParam = new LibCommon.Protocol.Error.ErrorResponse
            {
                code = 0
            };

            sender.ToSuccess(
                this.session,
                (int)LobbyProtocolType.Exception,
                Newtonsoft.Json.JsonConvert.SerializeObject(resParam)
            );
        }

        protected override void Post()
        {
            // none
        }
        protected override void PostRelease()
        {
            // none
        }
    }
}
