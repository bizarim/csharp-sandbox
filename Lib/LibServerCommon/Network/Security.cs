
namespace LibServerCommon.Network
{
    public interface IEncryptable
    {
        string Encrypt(string contents);
        string Decrypt(string contents);
    }

    public class Security : IEncryptable
    {
        public string Encrypt(string contents)
        {
            // todo
            return contents;
        }

        public string Decrypt(string contents)
        {
            // todo
            return contents;
        }
    }
}
