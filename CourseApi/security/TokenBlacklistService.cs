namespace CourseApi.security
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private static readonly HashSet<string> _blacklist = new()
        {
          "test-jti-001",
          "test-jti-002"
        };

        private static readonly object _lock = new();
        public void AddToBlacklist(string jti)
        {
            lock(_lock)
            {
                _blacklist.Add(jti);
            }
        }

        public bool IsBlacklisted(string jti)
        {
            lock (_lock)
            {
                return _blacklist.Contains(jti);
            }
        }
    }
}
