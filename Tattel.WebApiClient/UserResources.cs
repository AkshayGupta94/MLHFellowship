using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using IdentityModel.Client;
using Newtonsoft.Json;
using Tattel.WebApiClient.Model;
using Tattel.WebApiClient.Models;

namespace Tattel.WebApiClient
{
    public class UserResources
    {
        HttpClient client;

        public UserResources()
        {
            client = HttpConnectionFactory.Create();
        }


        public User GetUser(string id)
        {
            User user = null;

            var url = $"user/Get/{id}";
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<User>(json);
            }

            return user;
        }


        public List<User> GetUsers()
        {
            List<User> states = null;

            var url = $"user/GetAll";
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                states = JsonConvert.DeserializeObject<List<User>>(json);
            }

            return states;
        }

        public async Task<User> AddUser(User user)
        {
            var url = $"user/Add";
            var content = JsonConvert.SerializeObject(user, Formatting.None);
            var response = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
            var json = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<User>(json);
            return null;
        }

        public bool UpdateUser(User user)
        {
            var url = $"user/Update";
            var content = JsonConvert.SerializeObject(user, Formatting.None);
            var response = client.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json")).Result;

            return response.IsSuccessStatusCode;
        }

        public bool DeleteUser(string id)
        {
            var url = $"user/Delete/{id}";
            var response = client.DeleteAsync(url).Result;

            return response.IsSuccessStatusCode;
        }

        public bool DeleteUsers()
        {
            var url = $"user/DeleteAll";
            var response = client.DeleteAsync(url).Result;

            return response.IsSuccessStatusCode;
        }


        public async Task<string> UploadImage(Stream stream, string fileName)
        {
            string[] strs = fileName.Split('/');
            if (strs.Length > 0)
                fileName = strs[strs.Length - 1];
            var url = $"user/UploadImage";

            try
            {
                HttpContent fileStreamContent = new StreamContent(stream);
                fileStreamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileStreamContent);
                    var response = client.PostAsync(url, formData).Result;
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}


