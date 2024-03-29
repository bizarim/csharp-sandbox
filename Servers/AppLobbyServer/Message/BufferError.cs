﻿
namespace AppLobbyServer.Message
{
    public class BufferError : LibServerCommon.Message.AbsMessage
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
                session,
                (int)LibCommon.Protocol.Lobby.LobbyProtocolType.ErrorResonse,
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
