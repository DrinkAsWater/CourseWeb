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
                BaseAddress = new Uri("https://localhost:7283/")
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



            // 測試 1: 登入 API
            Console.WriteLine("【測試 1: 登入 API】");
            var authApi = new AuthClient(httpClient);
            var loginResponse = await authApi.LoginAsync("shop@email.com", "M13579kk");

            if (loginResponse != null)
            {
                Console.WriteLine($"Token: {loginResponse.Token}");
                Console.WriteLine($"使用者: {loginResponse.Username}");

                // 登入成功後，可以設定 Token 給後續 API 使用
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResponse.Token);
            }

            Console.WriteLine("\n=================================\n");



            // 測試 2: 取得課程訂單 API
            Console.WriteLine("【測試 2: 取得課程訂單 API】");
            var shopApi = new ShopApiClient(httpClient);
            var orders = await shopApi.GetOrdersAsync();

            if (orders != null && orders.Count > 0)
            {
                Console.WriteLine($"\n共取得 {orders.Count} 筆訂單:");
                foreach (var order in orders)
                {
                    Console.WriteLine($"  - 訂單編號: {order.Id}");
                    Console.WriteLine($"    課程: {order.CourseName}");
                    Console.WriteLine($"    講師: {order.TeacherName}");
                    Console.WriteLine($"    日期: {order.CourseStartDate} ~ {order.CourseEndDate}");
                    Console.WriteLine($"    時間: {order.Times}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("\n=================================\n");

    


            

            // 測試 3: 下單 API
            Console.WriteLine("【測試 3: 下單 API】");

            // 這裡需要提供學生ID+課程排程ID，請根據實際情況修改
            Guid stuId = Guid.Parse("CB14171D-448D-4902-BCD2-A6E4D58400B5");
            Guid courseScheduleId = Guid.Parse("2F632057-B75D-470C-A489-C079C51F2621");

            var createOrderSuccess = await shopApi.CreateOrderAsync(
                new Models.CreateOrderRequest()
                {
                    StuId = stuId,
                    ScheduleId = courseScheduleId
                });

            if (createOrderSuccess)
            {
                Console.WriteLine("✓ 課程訂單建立完成!");
            }


            Console.WriteLine("\n=================================\n");

            //測試4.刪除訂單api
            Console.WriteLine("【測試 3: 下單 API】");

            ////使用測試2取得一筆訂單來刪除
            ////bool deleteSuccess = await shopApi.DeleteOrderAsync(Guid.Parse(""));

            //if (deleteSuccess) {
            //    Console.WriteLine("刪除訂單成功");
            //}

            // 測試 5: 更新會員api
            Console.WriteLine("【測試 5: 更新會員 API】");

           
           
            var userApi = new UserApiClient(httpClient);
            bool updateresult = await userApi.UpdateMemberInfoAsync(new Models.UserInfoUpdateModel.UserInfoRequest()
            {
                Name = "",
                Mobile = "123456789"
            });
            

            if (updateresult)
            {
                Console.WriteLine("✓ 更新完成!");
            }



            #endregion 




        }
    }
}

