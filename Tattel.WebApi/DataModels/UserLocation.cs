using System;
namespace Tattel.WebApi.DataModels
{
    public class UserLocation
    {
        public UserLocation()
        {
        }

        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public GPSLocation GPSLocation { get; set; }
    }
}
