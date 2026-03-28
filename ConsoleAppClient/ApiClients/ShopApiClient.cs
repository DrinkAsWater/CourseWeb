using ConsoleAppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppClient.ApiClients
{
    public class ShopApiClient
    {
        private readonly HttpClient _httpClient;

        public ShopApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 取得所有訂單
        /// </summary>
        /// <returns>訂單清單</returns>
        public async Task<List<ShopOrderModel>?> GetOrdersAsync()
        {
            Console.WriteLine($"呼叫 API: GET {_httpClient.BaseAddress}api/shop");

            var response = await _httpClient.GetAsync("api/shop");

            if (response.IsSuccessStatusCode)
            {
                var orders = await response.Content.ReadFromJsonAsync<List<ShopOrderModel>>();
                Console.WriteLine($"✓ 成功取得 {orders?.Count ?? 0} 筆訂單資料");
                return orders;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"✗ 取得訂單失敗! 狀態碼: {response.StatusCode}");
                Console.WriteLine($"錯誤訊息: {errorContent}");
                return null;
            }
        }


        /// <summary>
        /// 建立訂單（下單）
        /// </summary>
        /// <param name="stuId">學生編號</param>
        /// <param name="scheduleId">課程排程編號</param>
        /// <returns>是否成功</returns>
        public async Task<bool> CreateOrderAsync(CreateOrderRequest createOrderRequest)
        {
            Console.WriteLine($"呼叫 API: POST {_httpClient.BaseAddress}api/shop");
            Console.WriteLine($"學生編號: {createOrderRequest.StuId}");
            Console.WriteLine($"課程編號: {createOrderRequest.ScheduleId}");

            var response = await _httpClient.PostAsJsonAsync("api/shop", createOrderRequest);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✓ 下單成功!");
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"✗ 下單失敗! 狀態碼: {response.StatusCode}");
                Console.WriteLine($"錯誤訊息: {errorContent}");
                return false;
            }
        }

    }
}
