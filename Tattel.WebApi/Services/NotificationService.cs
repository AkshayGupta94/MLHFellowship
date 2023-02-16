using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tattel.WebApi.Interfaces;
using Tattel.WebApi.Persistence;

namespace Tattel.WebApi.Services
{
    public static class NotificationService
    {
        private static IUserRepository repository;
        private static IUserRepository _repository
        {
            get
            {
                return repository ?? (repository = new UserRepository());
            }
        }

        public static async Task send(string message, string title, string userId, string type, string extraData)
        {
            try
            {
                string userToken = await _repository.GetToken(userId);
                if (string.IsNullOrWhiteSpace(userToken))
                    return;
                //TODO: create a constant class and write that to it
                string applicationID = "AAAA_BySX0E:APA91bEg7c_rV01MIoGM00l8vO1Rjc09URKkOptbRKy_eeik8_31IA3YmoBe0uu3v0OXgUyCB6I449hYEUgsgUAgvgtCgSm4GKRjAwjGAEc1fKs3lwYYaa0sq9YCk-EPj2iowGFr7YFG";
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    // send to subscribed topic "Chats"                 
                    token = userToken,
                    notification = new
                    {
                        body = message,
                        title = title
                    },
                    data = new
                    {
                        type = "Notification",
                        category = "Show",
                        notificationType = type,
                        timestamp = DateTime.UtcNow.Ticks.ToString(),
                        extras = extraData
                    },
                    to = userToken
                };


                var json = JsonConvert.SerializeObject(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
    }
}
