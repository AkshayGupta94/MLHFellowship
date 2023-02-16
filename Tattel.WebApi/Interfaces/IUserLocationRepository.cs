using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using MongoDB.Driver;
using Tattel.WebApi.DataModels;

namespace Tattel.WebApi.Interfaces
{
    public interface IUserLocationRepository
    {
        Task<UserLocation> Get(string id);
        Task<List<UserLocation>> Get();
        Task Add(UserLocation userLocation);
        Task<UpdateResult> Update(UserLocation userLocation);
        Task<DeleteResult> Delete(string id);
        Task<DeleteResult> Delete();
    }
}
