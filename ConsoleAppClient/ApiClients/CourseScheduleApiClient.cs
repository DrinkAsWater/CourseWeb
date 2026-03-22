using ConsoleAppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppClient.ApiClients
{
    public class CourseScheduleApiClient
    {
        private readonly HttpClient _httpclient;
        public CourseScheduleApiClient(HttpClient client) {

            _httpclient = client;
        }
        public async Task<List<CourseScheduleModel>?> GetCourseAsync()
        {
        Console.WriteLine($"呼叫API: GET {_httpclient.BaseAddress}api/CourseSchedule");
            var response = await _httpclient.GetAsync("api/CourseSchedule");
            if (response.IsSuccessStatusCode)
            {
            var course = await response.Content.ReadFromJsonAsync<List<CourseScheduleModel>>();
                Console.WriteLine($"成功取得:{course?.Count ?? 0}筆課程資料");
                return course;
            }
            else
            {
                var errorContet = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"")
            }

        }
    }
}
