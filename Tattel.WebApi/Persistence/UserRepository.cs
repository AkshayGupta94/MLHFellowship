using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using MongoDB.Driver;

using Tattel.WebApi.DataModels;
using Tattel.WebApi.Interfaces;

using Azure.Storage.Blobs;
using Azure.Storage;
using Azure.Storage.Sas;
using Azure.Storage.Blobs.Specialized;
using System.IO;
using System.Net.Http;
using MongoDB.Bson;

namespace Tattel.WebApi.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly TattelContext context;

        private readonly IConfigurationRepository _configuration;


        public UserRepository()
        {
            context = new TattelContext();
            _configuration = new ConfigurationRepository();
        }

        public async Task<User> Get(string id)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id, id);
            return await context.Users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<User>> Get()
        {
            try
            {
                var filter = Builders<User>.Filter.Empty;
                return await context.Users.Find(filter).SortBy(i => i.Id).ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> Add(User user)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(i => i.EmailId, user.EmailId);
                var result = await context.Users.Find(filter).FirstOrDefaultAsync();

                if (result == null)
                {
                    user.Id = Guid.NewGuid().ToString();
                    await context.Users.InsertOneAsync(user);
                    return user.Id;
                }
                return result.Id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<UpdateResult> Update(User user)
        {
            var filter = Builders<User>.Filter.Eq(i => i.EmailId, user.EmailId);

            var update = Builders<User>.Update
                                 .Set(s => s.Name, user.Name)
                                 .Set(s => s.ProfilePicPath, user.ProfilePicPath)
                                 .Set(s => s.Bio, user.Bio)
                                 .Set(s => s.PhoneNumber, user.PhoneNumber)
                                 .Set(s => s.City, user.City)
                                 .Set(s => s.Company, user.Company)
                                 .Set(s => s.Country, user.Country)
                                 .Set(s => s.Role, user.Role)
                                 .Set(s => s.Sector, user.Sector)
                                 .Set(s => s.DateOfBirth, user.DateOfBirth);

            return await context.Users.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> UpdateLocation(string userId, double latitude, double longitude)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id, userId);

            var update = Builders<User>.Update
                                 .Set(s => s.Latitude, latitude)
                                 .Set(s => s.Longitude, longitude);
            return await context.Users.UpdateOneAsync(filter, update);
        }

        public async Task<DeleteResult> Delete(string id)
        {
            var filter = Builders<User>.Filter.Eq("_id", id);
            return await context.Users.DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> Delete()
        {
            var filter = Builders<User>.Filter.Empty;
            return await context.Users.DeleteManyAsync(filter);
        }


        /// <summary>
        /// API Method to allow user to Upload their Profile picture to Azure Blob Storage
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> UploadImageToStorage(Stream stream, string fileName)
        {
            try
            {
                //Upload image to container
                var imageName = DateTime.UtcNow.ToString("MMddyyyyhhmmsstt") + "_" + fileName;
                var a = await _configuration.Container.UploadBlobAsync(imageName, stream);
                return _configuration.Container.Uri.AbsoluteUri + "/" + imageName;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<User>> GetNearbyUsers(string userId, int count, int range)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id, userId);
            var onlineFilter = Builders<User>.Filter.Eq(i => i.IsOnline, true);

            var user = await context.Users.Find(filter & onlineFilter).FirstOrDefaultAsync();
            if (user == null)
                return null;

            var nearbyFilter = Builders<User>.Filter.Where(_ => (_.Latitude >= user.Latitude - 0.008983 * range && _.Latitude <= user.Latitude + 0.008983 * range)
                                                        && _.Longitude >= user.Longitude - 0.015060 * range && _.Longitude <= user.Longitude + 0.015060 * range
                                                        && !string.Equals(_.Id, userId));

            return await context.Users.Find(nearbyFilter).ToListAsync();
        }

        public async Task<UpdateResult> UpdateToken(string userId, string token)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id, userId);

            var update = Builders<User>.Update
                                 .Set(s => s.Token, token);
            return await context.Users.UpdateOneAsync(filter, update);
        }
        public async Task<string> GetToken(string userId)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id, userId);

            var user = await context.Users.Find(filter).FirstOrDefaultAsync();
            if (user == null)
                throw new Exception("user not found");
            return user.Token;
        }
        public async Task<UpdateResult> UpdateUserStatus(string userId, bool isOnline)
        {
            var filter = Builders<User>.Filter.Eq(i => i.Id, userId);

            var update = Builders<User>.Update
                                 .Set(s => s.IsOnline, isOnline);
            return await context.Users.UpdateOneAsync(filter, update);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(i => i.EmailId, email);
            return await context.Users.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<List<UserInterest>> GetUserInterests()
        {
            return await context.UserInterests.Find(_ => true).ToListAsync();
        }
        public async Task AddUserInterests()
        {
            try
            {
                //context.UserInterests.DeleteMany(Builders<UserInterest>.Filter.Empty);
                List<string> final = new List<string>();
                List<int> error = new List<int>();
                var httpClient = new HttpClient();
                List<string> errorfinal = new List<string>();
                for (int i = 1; i < 189; i++)
                {
                    string data = "";
                    try
                    {
                        data = await httpClient.GetStringAsync("https://www.allpastimes.com/hobby" + i);
                        var a = data.Split("<meta name=\"description\" content=\"");
                        var da = a[1].Remove(a[1].IndexOf("\"/>"));
                        da = da.Trim();
                        da = da.Replace("&amp;", ",");
                        da = da.Replace(", ", ",");
                        da = da.Replace(" ,", ",");
                        var ada = da.Split(",");
                        final.AddRange(ada);
                    }
                    catch (Exception) { error.Add(i); continue; }
                }

                List<UserInterest> ls = new List<UserInterest>();
                foreach (string s in final)
                {
                    try
                    {
                        UserInterest a = new UserInterest { InterestName = char.ToUpper(s[0]) + s.Substring(1), _id = new BsonBinaryData(Guid.NewGuid(), GuidRepresentation.Standard) };
                        ls.Add(a);
                    }
                    catch (Exception) { errorfinal.Add(s); continue; }
                }
                await context.UserInterests.InsertManyAsync(ls);
            }
            catch (Exception e)
            {

            }
        }

        public async Task AddJobIndustry()
        {
            try
            {
                string text = File.ReadAllText("C:\\Users\\garv\\source\\other_repos\\tattel\\Tattel.MobileApp\\Tattel.MobileApp\\temp.txt");

                var a1 = text.Split("<span style=\"color: #000000; \">");
                List<string> final = new List<string>();
                for (int j = 1;j < a1.Length; j++)
                {
                    string da = a1[j].Remove(a1[j].IndexOf('<'));
                    da = da.Replace("&amp;", "&");
                    da = da.Replace(" in the UK", "");
                    final.Add(da);
                }
                final.Sort();
                List<UserInterest> ls = new List<UserInterest>();
                foreach (string s in final)
                {
                    try
                    {
                        UserInterest a = new UserInterest { InterestName = char.ToUpper(s[0]) + s.Substring(1), _id = new BsonBinaryData(Guid.NewGuid(), GuidRepresentation.Standard) };
                        ls.Add(a);
                    }
                    catch (Exception) { continue; }
                }
                await context.UserInterests.InsertManyAsync(ls);
            }
            catch (Exception e)
            {

            }
        }
        public async Task<string> AddUserOtp(string userEmail, string otp)
        {
            var data = new Verification_Otp();
            data._id = new BsonBinaryData(Guid.NewGuid(), GuidRepresentation.Standard);
            data.UserEmail = userEmail;
            data.Otp = otp;
            var filter = Builders<Verification_Otp>.Filter.Eq(i => i.UserEmail, userEmail);
            var existingData = await context.Verification_Otp.Find(filter).FirstOrDefaultAsync();
            if (existingData != null)
                await context.Verification_Otp.DeleteManyAsync(filter);
            await context.Verification_Otp.InsertOneAsync(data);
            return data._id.ToString();
        }
        public async Task<Verification_Otp> GetUserOtp(string userEmail, bool isDelete = false)
        {
            var filter = Builders<Verification_Otp>.Filter.Eq(i => i.UserEmail, userEmail);
            var data = await context.Verification_Otp.Find(filter).FirstOrDefaultAsync();
            if (data != null && isDelete)
                await context.Verification_Otp.DeleteManyAsync(filter);
            return data;
        }
    }
}

