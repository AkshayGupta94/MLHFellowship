using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using IdentityModel.Client;


namespace Tattel.Common 
{
    public static class AuthHelper
    {
        public static AuthServerResponse Authenticate(string baseAddress, string client, string secret, string username, string password, string scope)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return new AuthServerResponse
                {
                    ResponseCode = 1,
                    Message = "Authentication failed. Invalid username or password."
                };
            }

            var httpClient = new HttpClient();

            var discoveryDocRequest = new DiscoveryDocumentRequest
            {
                Address = baseAddress,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            };

            var disco = httpClient.GetDiscoveryDocumentAsync(discoveryDocRequest).Result;
            if (disco.IsError)
            {
                return new AuthServerResponse
                {
                    ResponseCode = 2,
                    Message = $"{disco.Error}"
                };
            }

            var tokenRequest = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = client,
                ClientSecret = secret,
                UserName = username,
                Password = password,
                Scope = scope
            };

            var tokenResponse = httpClient.RequestPasswordTokenAsync(tokenRequest).Result;
            if (tokenResponse.IsError)
            {
                if (tokenResponse.ErrorDescription == "invalid_username_or_password")
                {
                    return new AuthServerResponse
                    {
                        ResponseCode = 3,
                        Message = "Login failed. Invalid username or password."
                    };
                }
                else
                {
                    return new AuthServerResponse
                    {
                        ResponseCode = 4,
                        Message = $"{tokenResponse.Error}"
                    };
                }
            }

            var userInfoRequest = new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = tokenResponse.AccessToken
            };

            var userInfoResponse = httpClient.GetUserInfoAsync(userInfoRequest).Result;
            if (userInfoResponse.IsError)
            {
                return new AuthServerResponse
                {
                    ResponseCode = 5,
                    Message = $"{userInfoResponse.Error}"
                };
            }

            var user = new UserAccount
            {
                Id = username,
                AccessToken = tokenResponse.AccessToken,
                Claims = userInfoResponse.Claims
            };

            var firstName = user.Claims.Where(c => c.Type.ToLower() == "given_name").FirstOrDefault();
            if (firstName != null) user.FirstName = firstName.Value;

            var lastName = user.Claims.Where(c => c.Type.ToLower() == "family_name").FirstOrDefault();
            if (lastName != null) user.LastName = lastName.Value;

            var roleClaims = user.Claims.Where(c => c.Type.ToLower() == "role").ToList();
            foreach (var claim in roleClaims)
            {
                user.Roles.Add(claim.Value);
            }

            return new AuthServerResponse
            {
                ResponseCode = 0,
                Message = string.Empty,
                UserAccount = user
            };
        }
        public static bool UserIsInspector(UserAccount user)
        {
            var claim = user.Claims.FirstOrDefault(c => c.Type.ToLower() == "role" && c.Value.ToLower() == "inspector");
            return claim != null;
        }
        public static bool UserIsSupervisor(UserAccount user)
        {
            var claim = user.Claims.FirstOrDefault(c => c.Type.ToLower() == "role" && c.Value.ToLower() == "supervisor");
            return claim != null;
        }

        public static bool UserIsCentreManager(UserAccount user)
        {
            var claim = user.Claims.FirstOrDefault(c => c.Type.ToLower() == "role" && c.Value.ToLower() == "centre manager");
            return claim != null;
        }
        public static bool UserIsCandidate(UserAccount user)
        {
            var claim = user.Claims.FirstOrDefault(c => c.Type.ToLower() == "role" && c.Value.ToLower() == "candidate");
            return claim != null;
        }
        public static bool UserIsExaminer(UserAccount user)
        {
            var claim = user.Claims.FirstOrDefault(c => c.Type.ToLower() == "role" && c.Value.ToLower() == "examiner");
            return claim != null;
        }
    }
}

