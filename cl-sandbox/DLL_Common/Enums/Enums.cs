namespace DLL_Common.Enums
{
    /// <summary>
    /// 인증타입(계정유형)
    /// </summary>
    public enum eCDType
    {
        Guest           = 1,
        GooglePlay      = 2,
        IOSGameCenter   = 3,
        Facebook        = 4,
    }

    /// <summary>
    /// OS 타입
    /// </summary>
    public enum eOSType
    {
        Android     = 1,
        IOS         = 2,
        Windows     = 3,
    }

    /// <summary>
    /// 마켓 타입
    /// </summary>
    public enum eMarketType
    {
        GooglePlay  = 100,
        IOS         = 200,
    }
}
