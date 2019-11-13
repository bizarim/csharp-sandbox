using System;
using System.Text;

namespace LibServerCommon.Network
{
    public class Buffer
    {
        // size
        public static readonly int RECV_BUFFER_SIZE = 10240;   // buffer size = 20480
        public static readonly int SEND_BUFFER_SIZE = 819200;  // buffer size = 819200, 0.8192 mb
        public static readonly int HEADER_SIZE = 8;            // header (8byte) = size (4byte) + type (4byte)

        // offset
        public static readonly int PACKET_SIZE_OFFSET = 0;      // 0 위치 읽는다.
        public static readonly int PACKET_TYPE_OFFSET = 4;      // 4 위치 읽는다.

        public bool IsInit { get; protected set; }
        public int Version { get; protected set; }
        public int ProtocolType { get; protected set; }
        public string Contents { get; set; }

        public Buffer(byte[] buffer)
        {
            try
            {
                this.Version = BitConverter.ToInt32(buffer, PACKET_SIZE_OFFSET);
                this.ProtocolType = BitConverter.ToInt32(buffer, PACKET_SIZE_OFFSET + PACKET_TYPE_OFFSET);
                this.Contents = Encoding.UTF8.GetString(buffer, PACKET_TYPE_OFFSET + PACKET_TYPE_OFFSET, buffer.Length - (PACKET_TYPE_OFFSET + PACKET_TYPE_OFFSET));
                this.IsInit = true;
            }
            catch
            {
                this.IsInit = false;
            }
        }
    }
}
