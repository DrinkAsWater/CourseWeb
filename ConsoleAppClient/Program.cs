using ConsoleAppClient.ApiClients;

namespace ConsoleAppClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== API 呼叫測試 ===\n");

            // 建立共用的 HttpClient（整個程式只建立一次）
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7224/")
            };


            #region 測試 : 取得課程 API

            // 測試 : 取得課程 API
            Console.WriteLine("【測試 : 取得課程清單 API】");
            var courseApi = new CourseScheduleApiClient(httpClient);
            var courses = await courseApi.GetCourseAsync();

            if (courses != null && courses.Count > 0)
            {
                Console.WriteLine($"\n共取得 {courses.Count} 筆課程:");
                foreach (var course in courses)
                {
                    Console.WriteLine($"  - {course.Code}: {course.Name}");
                    Console.WriteLine($"    講師: {course.TeacherName}");
                    Console.WriteLine($"    時間: {course.Sdate} ~ {course.Edate}");
                    Console.WriteLine($"    地點: {course.Location}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("\n=================================\n");

            #endregion


        }
    }
}

    

