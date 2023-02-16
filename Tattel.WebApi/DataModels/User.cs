using System;
namespace Tattel.WebApi.DataModels
{
    public class User
    {
        public User()
        {
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string ProfilePicPath { get; set; }
        public string Bio { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Token { get; set; }
        public bool IsOnline { get; set; }
        public string DateOfBirth { get; set; }
        public string Role { get; set; }
        public string Company { get; set; }
        public string Sector { get; set; }
        public string EmailId { get; set; }
        public int UserType { get; set; } // 1: normal, 2: linkdien
        public string LinkdienToken { get; set; }
    }
}