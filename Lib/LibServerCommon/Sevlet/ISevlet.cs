namespace LibServerCommon.Sevlet
{
    public interface ISevlet
    {
        void Pre();
        void Execute();
        void Post();
    }
}
