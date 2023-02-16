using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Tattel.WebApiClient
{
    public static class HttpConnectionFactory
    {
        private static HttpClient _client = null;
        private static readonly string _baseUrl = null;

        public static HttpClient Create()
        {
            var baseUrl = "https://testtattelwebapi.azurewebsites.net";

            if (_client == null || _baseUrl != baseUrl)
            {
                _client = new HttpClient
                {
                    BaseAddress = new Uri($"{baseUrl}/api/"),
                    Timeout = TimeSpan.FromMinutes(10),
                };

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            }

            return _client;
        }
    }
}
