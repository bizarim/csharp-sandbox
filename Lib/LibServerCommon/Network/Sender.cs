using System;
using System.Text;

namespace LibServerCommon.Network
{
    public class Sender
    {
        public void ToSuccess(SuperWebSocket.WebSocketSession session, int type, string json) {

            json = new Security().Decrypt(json);

            byte[] _sendBuffer = new byte[Buffer.HEADER_SIZE + Buffer.SEND_BUFFER_SIZE];
            byte[] _data = Encoding.UTF8.GetBytes(json);
            BitConverter.GetBytes(type).CopyTo(_sendBuffer, Buffer.PACKET_TYPE_OFFSET);

            _data.CopyTo(_sendBuffer, Buffer.HEADER_SIZE);
            session.Send(_sendBuffer, 0, Buffer.HEADER_SIZE + _data.Length);
        }

        public void ToError()
        {
            // todo
        }

        public void ToException()
        {
            // todo
        }
    }
}
