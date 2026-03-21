using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
