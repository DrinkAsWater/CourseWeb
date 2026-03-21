namespace CourseApi.security
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private static readonly HashSet<string> _blacklist = new()
        {
          "test-jti-001",
          "test-jti-002",
          "6a4a3d9e-45d1-43bf-bbf6-37800b76298d"
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
