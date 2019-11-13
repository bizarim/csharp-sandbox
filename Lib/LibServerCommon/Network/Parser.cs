namespace LibServerCommon.Network
{
    public interface IConvertable
    {
        void Serialize();
        Buffer Deserialize(byte[] datas);
    }

    public class Converter : IConvertable
    {
        public void Serialize()
        {
            // todo
        }

        public Buffer Deserialize(byte[] datas)
        {
            var buffer = new Buffer(datas);
            buffer.Contents = new Security().Decrypt(buffer.Contents);
            return buffer;
        }
    }
}
