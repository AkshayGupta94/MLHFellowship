using System;
namespace Tattel.WebApiClient.Models
{
    public class GPSLocation
    {
        public GPSLocation()
        {
        }

        public string LocationId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
