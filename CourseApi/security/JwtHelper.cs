using CourseApi.Response;
using CourseService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CourseApi.security
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;
        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public LoginResponse GenerateToken(UserModel user)
        {
            var jwtSettings = _configuration.GetSection("JwtTokenSettings");
            var issuerSigningKey = jwtSettings["IssuerSigningKey"]
             ?? throw new InvalidOperationException("IssuerSigningKey is not configured");
            var issuer = jwtSettings["Issuer"]
             ?? throw new InvalidOperationException("Issuer is not configured");
            var audience = jwtSettings["Audience"]
                ?? throw new InvalidOperationException("Audience is not configured");
            var expireUnitStr = jwtSettings["ExpireUnit"]
                ?? throw new InvalidOperationException("ExpireUnit is not configured");
            var expireInMin = int.Parse(expireUnitStr);
            // 驗證密鑰長度，HMAC-SHA256 建議至少 32 bytes
            if (Encoding.UTF8.GetBytes(issuerSigningKey).Length < 32)
                throw new ArgumentException("IssuerSigningKey must be at least 32 bytes for HMAC-SHA256");
            //產生對稱簽章金鑰
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey));
            //設置簽章憑證-指定使用 HMAC-SHA256 算法進行簽名
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //設置過期時間（使用 UTC 時間避免時區問題）
            var expiry = DateTime.UtcNow.AddMinutes(expireInMin);
            // 建立 JWT Token 的 Claims（聲明）集合，Claims 是 JWT Token 中用來儲存使用者資訊的Key-Value。
            // 這些聲明可以包含使用者的唯一識別碼、電子郵件、使用者名稱等資訊，這些資訊可以在後續的請求中用來識別和授權使用者。
            // JWT Token 是 Base64 編碼，不是加密，任何人都可以解碼查看 Claims 內容
            // 因此，不應在 Claims 中存放敏感資訊，如密碼,信用卡號等
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // JWT 標準聲明，代表「主體」,通常用來存放使用者的唯一識別碼 (User ID)
                new Claim(JwtRegisteredClaimNames.Email, user.Email), // 電子郵件
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID,可用於防止 Token 重複使用、實作 Token 撤銷機制
                new Claim(ClaimTypes.Name, user.UserName) // 使用者名稱
            };
            //產生 JWT Token 物件
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiry,
                signingCredentials: credentials
            );
            //將 JWT Token 物件 轉換成 字串
            var tokenHandler = new JwtSecurityTokenHandler();
            var encodedToken = tokenHandler.WriteToken(token);

            // 返回 Token 資訊
            return new LoginResponse
            {
                Token = encodedToken,
                UserName = user.UserName

            };
        }
    }
}
