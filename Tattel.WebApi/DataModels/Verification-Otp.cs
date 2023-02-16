using MongoDB.Bson;

namespace Tattel.WebApi.DataModels
{
    public class Verification_Otp
    {
        public BsonValue _id { get; set; }
        public string UserEmail { get; set; }
        public string Otp { get; set; }
    }
}
