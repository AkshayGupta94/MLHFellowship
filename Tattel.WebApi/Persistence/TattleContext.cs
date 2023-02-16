using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;

using MongoDB.Bson;
using MongoDB.Driver;

using Tattel.WebApi.DataModels;
using Microsoft.Extensions.Options;

namespace Tattel.WebApi.Persistence
{
    public class TattelContext
    {
        private readonly IMongoDatabase database;
        private ConfigurationRepository _configuration { get; }

        public TattelContext()
        {
            _configuration = new ConfigurationRepository();
            var client = new MongoClient(_configuration.DatabaseConnectionString);
            if(client != null) database = client.GetDatabase(_configuration.Database);
        }

        public void TestConnection()
        {
            var client = new MongoClient(DbSettings.ConnectionString);
            client.ListDatabases();
        }

        public IMongoCollection<Conversation> Conversations
        {
            get
            {
                return database.GetCollection<Conversation>("Conversations");
            }
        }

        public IMongoCollection<User> Users
        {
            get
            {
                return database.GetCollection<User>("Users");
            }
        }

        public IMongoCollection<GPSLocation> GPSLocations
        {
            get
            {
                return database.GetCollection<GPSLocation>("GPS-Locations");
            }
        }

        public IMongoCollection<Message> Messages
        {
            get
            {
                return database.GetCollection<Message>("Messages");
            }
        }

        public IMongoCollection<UserLocation> UserLocations
        {
            get
            {
                return database.GetCollection<UserLocation>("User-Locations");
            }
        }

        public IMongoCollection<UserStatus> UserStatuses
        {
            get
            {
                return database.GetCollection<UserStatus>("User-Status");
            }
        } 
        public IMongoCollection<MeetingRecord> MeetingRecords
        {
            get
            {
                return database.GetCollection<MeetingRecord>("Meeting-Records");
            }
        } 
        public IMongoCollection<UserInterest> UserInterests
        {
            get
            {
                return database.GetCollection<UserInterest>("User-Interest");
            }
        }
        public IMongoCollection<Verification_Otp> Verification_Otp
        {
            get
            {
                return database.GetCollection<Verification_Otp>("Verification-Otp");
            }
        }
    }
}
