using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MongoDB.Driver;
using Tattel.WebApi.DataModels;


namespace Tattel.WebApi.Interfaces
{
    public interface IGPSLocationRepository
    {
        Task<GPSLocation> Get(string id);
        Task<List<GPSLocation>> Get();
        Task Add(GPSLocation gpsLocation);
        Task<UpdateResult> Update(GPSLocation gpsLocation);
        Task<DeleteResult> Delete(string id);
        Task<DeleteResult> Delete();
    }
}
