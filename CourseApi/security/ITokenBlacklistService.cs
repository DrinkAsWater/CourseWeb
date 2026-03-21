namespace CourseApi.security
{
    public interface ITokenBlacklistService
    {
        void AddToBlacklist(string jti);

        bool IsBlacklisted(string jti);
    }
}
