using CourseApi.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace CourseApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly JsonSerializerOptions _jsonOptions;

        public GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IWebHostEnvironment env,
    IOptions<JsonOptions> jsonOptions)
        {
            // 注入 RequestDelegate 以繼續執行後續的中介軟體
            _next = next;
            // 注入 ILogger 以記錄錯誤日誌(實現持久化到檔案或其他儲存媒介)
            _logger = logger;
            // 注入 IWebHostEnvironment 以判斷當前環境
            _env = env;
            // 使用系統全域的 JSON 序列化設定
            _jsonOptions = jsonOptions.Value.JsonSerializerOptions;
        }
        // 中介軟體的核心方法，捕捉例外並處理
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 繼續執行後續的中介軟體
                await _next(context);
            }
            catch (Exception ex)
            {
                // 記錄錯誤日誌
                _logger.LogError(ex, "發生未處理的例外: {Message}", ex.ToString());

                // 處理例外並回傳統一的錯誤回應
                await HandleExceptionAsync(context, ex);
            }
        }
              // 處理例外並回傳統一的錯誤回應
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // 設定回應的 Content-Type 為 JSON
            context.Response.ContentType = "application/json";

            // 設定 HTTP 狀態碼為 500
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // 建立錯誤回應物件
            var errorResponse = new ErrorResponse(
                message: "伺服器發生錯誤，請稍後再試",
                path: context.Request.Path.ToString(),
                statusCode: context.Response.StatusCode
            );

            // 如果是開發環境，加入詳細的錯誤資訊
            if (_env.IsDevelopment())
            {
                errorResponse.Details = $"{exception.Message}\n\nStackTrace:\n{exception.StackTrace}";
            }

            // 使用全域設定的 JSON 序列化選項
            var jsonResponse = JsonSerializer.Serialize(errorResponse, _jsonOptions);

            // 寫入回應
            await context.Response.WriteAsync(jsonResponse);
        }
    }

}
