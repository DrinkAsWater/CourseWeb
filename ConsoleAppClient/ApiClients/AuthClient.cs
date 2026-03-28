using ConsoleAppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppClient.ApiClients
{
    public class AuthClient
    {
        private readonly HttpClient _httpClient;
        public AuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        /// <summary>
        /// 呼叫登入 API
        /// </summary>
        /// <param name="userName">使用者名稱</param>
        /// <param name="password">密碼</param>
        /// <returns>登入回應</returns>
        public async Task<LoginResponse?> LoginAsync(string userName, string password)
        {

            var loginRequest = new LoginRequest
            {
                UserName = userName,
                Password = password
            };

            Console.WriteLine($"呼叫 API: POST {_httpClient.BaseAddress}api/auth/login");
            Console.WriteLine($"使用者名稱: {userName}");

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                Console.WriteLine("✓ 登入成功!");
                return loginResponse;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"✗ 登入失敗! 狀態碼: {response.StatusCode}");
                Console.WriteLine($"錯誤訊息: {errorContent}");
                return null;
            }
        }
    }
}
