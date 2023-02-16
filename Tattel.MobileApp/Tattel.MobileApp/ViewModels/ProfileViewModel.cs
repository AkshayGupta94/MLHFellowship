using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Tattel.WebApiClient.Model;
using Tattel.WebApiClient.Models;
using Xamarin.Forms;

namespace Tattel.MobileApp.ViewModels
{
    public class ProfileViewModel
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public int? Type { get; set; }
        public string Sector { get; set; }
        public string Designation { get; set; }
        public string Organisation { get; set; }
        public string EmailAddress { get; set; }
        public string LinkedInToken { get; set; }
        public List<string> Interests { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Interest { get; set; }
        public Image LinkedInProfileImage { get; set; }
        public MediaFile ProfileImage { get; set; }
        public string ProfileImageLink { get; set; }
        public string Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Age { get; set; }
        public ProfileViewModel()
        {

        }
        public User GetUser()
        {
            User user = new User
            {
                EmailId= this.EmailAddress,
                Name = this.Name,
                Sector = this.Sector,
                UserType=this.Type,
                Company= this.Organisation,
                Role= this.Designation,
                ProfilePicPath = this.ProfileImageLink,
                Id = this.Id?.ToString(),
                PhoneNumber = this.PhoneNumber,
                Bio = this.Bio,
                Country = this.Country,
                Interests = this.Interest,
                DateOfBirth = this.DateOfBirth.Ticks.ToString()
        };
            return user;
        }
      
        public ProfileViewModel(WebApiClient.Model.User user)//For showing profile
        {
            this.EmailAddress = user.EmailId;
            this.Name = user.Name;
            this.Sector = user.Sector;
            this.ProfileImageLink = user.ProfilePicPath;
            this.Id = user.Id;
            this.Type = user.UserType;
            this.Organisation = user.Company;
            this.Designation = user.Role;

            #region Unused properties
            this.Country = user.Country;
            this.Bio = user.Bio;
            this.PhoneNumber = user.PhoneNumber;
            #endregion
            long t = 0;
            if (long.TryParse(user.DateOfBirth, out t))
            {
                this.DateOfBirth = new DateTime(t);
            }
        }
    }
}
