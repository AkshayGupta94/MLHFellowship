using System;
using System.Collections.Generic;
using System.Text;

namespace Tattel.MobileApp.Services
{
    public class WebService
    {
        static string baseUrl = "https://testtattelwebapi.azurewebsites.net";
        public WebApiClient.Api.UserApi UserService = new WebApiClient.Api.UserApi(baseUrl);
        public WebApiClient.Api.MessageApi MessageService = new WebApiClient.Api.MessageApi(baseUrl);
        public WebApiClient.Api.ConversationApi ConversationService = new WebApiClient.Api.ConversationApi(baseUrl);
        public WebApiClient.Api.GPSLocationApi GPSService = new WebApiClient.Api.GPSLocationApi(baseUrl);
        public WebApiClient.Api.UserLocationApi UserLocationService = new WebApiClient.Api.UserLocationApi(baseUrl);


    }
}
