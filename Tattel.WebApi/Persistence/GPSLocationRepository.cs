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
    public class GPSLocationRepository : IGPSLocationRepository
    {
        private readonly TattelContext context;

        public GPSLocationRepository()
        {
            context = new TattelContext();
        }

        public async Task<GPSLocation> Get(string LocationId)
        {
            var filter = Builders<GPSLocation>.Filter.Eq("_LocationId", LocationId);
            return await context.GPSLocations.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<GPSLocation>> Get()
        {
            var filter = Builders<GPSLocation>.Filter.Empty;
            return await context.GPSLocations.Find(filter).SortBy(i => i.LocationId).ToListAsync();
        }

        public async Task Add(GPSLocation GPSLocation)
        {
            var filter = Builders<GPSLocation>.Filter.Eq("_LocationId", GPSLocation.LocationId);
            var result = await context.GPSLocations.Find(filter).FirstOrDefaultAsync();

            if (result == null)
            {
                await context.GPSLocations.InsertOneAsync(GPSLocation);
            }
        }

        public async Task<UpdateResult> Update(GPSLocation GPSLocation)
        {
            var filter = Builders<GPSLocation>.Filter.Eq("_LocationId", GPSLocation.LocationId);

            var update = Builders<GPSLocation>.Update
                                 .Set(s => s.Longitude, GPSLocation.Longitude)
                                 .Set(s => s.Latitude, GPSLocation.Latitude);

            return await context.GPSLocations.UpdateOneAsync(filter, update);
        }

        public async Task<DeleteResult> Delete(string LocationId)
        {
            var filter = Builders<GPSLocation>.Filter.Eq("_LocationId", LocationId);
            return await context.GPSLocations.DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> Delete()
        {
            var filter = Builders<GPSLocation>.Filter.Empty;
            return await context.GPSLocations.DeleteManyAsync(filter);
        }
    }
}
