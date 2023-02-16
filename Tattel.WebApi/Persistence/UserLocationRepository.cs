using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

using Tattel.WebApi.DataModels;
using Tattel.WebApi.Interfaces;

namespace Tattel.WebApi.Persistence
{
    
    public class UserLocationRepository : IUserLocationRepository
    {
        private readonly TattelContext context;

        public UserLocationRepository()
        {
            context = new TattelContext();
        }

        public async Task<UserLocation> Get(string userId)
        {
            var filter = Builders<UserLocation>.Filter.Eq("_userId", userId);
            return await context.UserLocations.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<UserLocation>> Get()
        {
            var filter = Builders<UserLocation>.Filter.Empty;
            return await context.UserLocations.Find(filter).SortBy(i => i.UserId).ToListAsync();
        }

        public async Task Add(UserLocation userLocation)
        {
            var filter = Builders<UserLocation>.Filter.Eq("_userId", userLocation.UserId);
            var result = await context.UserLocations.Find(filter).FirstOrDefaultAsync();

            if (result == null)
            {
                await context.UserLocations.InsertOneAsync(userLocation);
            }
        }

        public async Task<UpdateResult> Update(UserLocation userLocation)
        {
            var filter = Builders<UserLocation>.Filter.Eq("_userId", userLocation.UserId);

            var update = Builders<UserLocation>.Update
                                 .Set(s => s.GPSLocation, userLocation.GPSLocation)
                                 .Set(s => s.Timestamp, userLocation.Timestamp);

            return await context.UserLocations.UpdateOneAsync(filter, update);
        }

        public async Task<DeleteResult> Delete(string id)
        {
            var filter = Builders<UserLocation>.Filter.Eq("_userId", id);
            return await context.UserLocations.DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> Delete()
        {
            var filter = Builders<UserLocation>.Filter.Empty;
            return await context.UserLocations.DeleteManyAsync(filter);
        }
    }
}
