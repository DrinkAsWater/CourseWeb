using ConsoleAppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppClient.ApiClients
{
    public class CourseScheduleApiClient
    {
        private readonly HttpClient _httpClient;
        public CourseScheduleApiClient(HttpClient httpClient) {

            _httpClient = httpClient;
        }
        public async Task<List<CourseScheduleModel>?> GetCourseAsync()
        {
            Console.WriteLine($"呼叫 API: GET {_httpClient.BaseAddress}api/CourseSchedule");

            var response = await _httpClient.GetAsync("api/CourseSchedule");

            if (response.IsSuccessStatusCode)
            {
                var courses = await response.Content.ReadFromJsonAsync<List<CourseScheduleModel>>();
                Console.WriteLine($"✓ 成功取得 {courses?.Count ?? 0} 筆課程資料");
                return courses;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"✗ 取得課程失敗! 狀態碼: {response.StatusCode}");
                Console.WriteLine($"錯誤訊息: {errorContent}");

                return new List<CourseScheduleModel>(); // 回傳空清單，避免 null 參考問題

                //return null;
            }

        }
    }
}
