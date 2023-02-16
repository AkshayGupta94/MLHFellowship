using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;

using Tattel.WebApiClient.Models;

namespace Tattel.WebApiClient
{
    public class ConversationResource
    {
        HttpClient client;

        public ConversationResource(string baseUrl)
        {
            client = HttpConnectionFactory.Create();
            //client.SetBearerToken(accessToken);
        }

        public Conversation GetConversation(string id)
        {
            Conversation conversation = null;

            var url = $"conversation/{id}";
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                conversation = JsonConvert.DeserializeObject<Conversation>(json);
            }

            return conversation;
        }


        public List<Conversation> GetConversations()
        {
            List<Conversation> states = null;

            var url = $"conversation";
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                states = JsonConvert.DeserializeObject<List<Conversation>>(json);
            }

            return states;
        }

        public bool AddConversation(Conversation conversation)
        {
            var url = $"conversation";
            var content = JsonConvert.SerializeObject(conversation, Formatting.None);
            var response = client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json")).Result;

            return response.IsSuccessStatusCode;
        }

        public bool UpdateConversation(Conversation conversation)
        {
            var url = $"conversation";
            var content = JsonConvert.SerializeObject(conversation, Formatting.None);
            var response = client.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json")).Result;

            return response.IsSuccessStatusCode;
        }

        public bool DeleteConversation(string id)
        {
            var url = $"conversation/{id}";
            var response = client.DeleteAsync(url).Result;

            return response.IsSuccessStatusCode;
        }

        public bool DeleteConversations()
        {
            var url = $"conversation";
            var response = client.DeleteAsync(url).Result;

            return response.IsSuccessStatusCode;
        }
    }
}
