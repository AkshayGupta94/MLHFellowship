using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tattel.WebApi.Middleware
{
    public class RequestResponseData
    {
        public long ResponseTime { get; internal set; }
        public int StatusCode { get; internal set; }
        public string Path { get; internal set; }
        public string Method { get; internal set; }
        public string ClientIp { get; internal set; }
        public DateTime Timestamp { get; internal set; }
        public string UserId { get; internal set; }
    }
}
