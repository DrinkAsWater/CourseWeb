using ConsoleAppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static ConsoleAppClient.Models.UserInfoUpdateModel;

namespace ConsoleAppClient.ApiClients
{
    public class UserApiClient
    {
        private readonly HttpClient _httpClient;
        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> UpdateMemberInfoAsync(UserInfoRequest userInfoRequest)
        {
            Console.WriteLine($"呼叫 API: PUT {_httpClient.BaseAddress}api/user/info");
            Console.WriteLine($"使用者名稱: {userInfoRequest.Name}");
            Console.WriteLine($"使用者名稱: {userInfoRequest.Mobile}");

            var response = await _httpClient.PutAsJsonAsync("api/user/info", userInfoRequest);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✓更新會員成功!");
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"✗更新會員失敗! 狀態碼: {response.StatusCode}");
                Console.WriteLine($"錯誤訊息: {errorContent}");
                return false;
            }
        }


        }
    }

