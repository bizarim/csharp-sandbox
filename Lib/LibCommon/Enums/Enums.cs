
namespace LibCommon.Enums
{
    // 인증(Authentication), 권한부여(Authorization)

    /// <summary>
    /// 인증타입(계정유형)
    /// </summary>
    public enum CredentialType
    {
        Guest = 1,
        GooglePlay = 2,
        IOSGameCenter = 3,
        Facebook = 4,
    }
}
