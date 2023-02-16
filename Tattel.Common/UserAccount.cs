using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Tattel.Common
{
    public class UserAccount
    {
        public UserAccount()
        {
            Roles = new List<string>();
            Claims = new List<Claim>();
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Names
        {
            get { return $"{FirstName} {LastName}"; }
            set { Names = value; }
        }
        public List<string> Roles { get; set; }
        public string AccessToken { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
