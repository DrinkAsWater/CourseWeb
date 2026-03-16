namespace CourseApi.Response
{
    public class ErrorResponse
    {
        public string Message { get; set; }

        public string? Details { get; set; }

        public DateTime Timestamp { get; set; }

        public string Path { get; set; }

        public int StatusCode { get; set; }

        public ErrorResponse(string message, string path, int statusCode = 500)
        {
            Message = message;
            Path = path;
            StatusCode = statusCode;
            Timestamp = DateTime.Now;
        }



    }
}
