using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Tattel.WebApi.DataModels;
using Tattel.WebApi.Interfaces;
using Tattel.WebApi.Middleware;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Tattel.WebApi.Services;
using System.Net;

namespace Tattel.WebApi.Controllers
{
    //[Authorize] //Secures the "api/user" endpoint by Identity Server 4 so that it can't be publicly accessed without a valid token
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public class UserController : Controller
    {
        private readonly IUserRepository repository;

        public UserController(IUserRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string id)
        {
            try     
            {
                var user = await repository.Get(id);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200,Type = typeof(User))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            if (user == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                if(!string.IsNullOrWhiteSpace(user.LinkdienToken))
                {
                    user.EmailId = await LinkedinService.GetUserEmail(user.LinkdienToken);
                    var linkedInProfileResponse = await LinkedinService.GetUserProfile(user.LinkdienToken);
                    if (linkedInProfileResponse == null)
                        return StatusCode(StatusCodes.Status401Unauthorized, "LinkedIn token expired");
                    user.Name = linkedInProfileResponse.firstName.localized.en_US + " " + linkedInProfileResponse.lastName.localized.en_US;
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead(linkedInProfileResponse.profilePicture.DisplayImage.elements[0].identifiers[0].identifier);
                    user.ProfilePicPath = await repository.UploadImageToStorage(stream, Guid.NewGuid().ToString());
                    user.UserType =2;
                }
                string id = await repository.Add(user);
                if(!string.IsNullOrWhiteSpace(id))
                    return Ok(await repository.Get(id));
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (user == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                await repository.Update(user);
                return NoContent();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        [HttpPut("update-location")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateLocation(string userId, double latitude, double longitude)
        {
            if (userId == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                var res = await repository.UpdateLocation(userId, latitude, longitude);
                if(res.IsAcknowledged)
                    return NoContent();
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(string),200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                Stream fileStream = null;
                if (file.Length > 0)
                {
                    using (fileStream = new FileStream(file.FileName, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        fileStream.Seek(0, SeekOrigin.Begin);
                        var url =  await repository.UploadImageToStorage(fileStream, file.FileName);
                        return Ok(url);
                    }
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetNearbyUsers")]
        [ProducesResponseType(200, Type = typeof(List<User>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetNearbyUsers(string userId, int size = 10, int range = 1)
        {
            try
            {
                var user = await repository.GetNearbyUsers(userId,size,range);
                if (user == null)
                {
                    return Ok(new List<User>());
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut("update-token")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateToken(string userId, string token)
        {
            if (userId == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                var res = await repository.UpdateToken(userId, token);
                if (res.IsAcknowledged)
                    return NoContent();
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut("update-status")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateStatus(string userId, bool isOnline)
        {
            if (userId == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                var res = await repository.UpdateUserStatus(userId, isOnline);
                if (res.IsAcknowledged)
                    return NoContent();
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{email}", Name = "GetUserByEmail")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await repository.GetUserByEmail(email);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet(Name = "GetUserIntrests")]
        [ProducesResponseType(200, Type = typeof(List<string>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserIntrests(string searchText = null)
        {
            try
            {
                var userIntrest = await repository.GetUserInterests();
                if (userIntrest == null)
                {
                    return NotFound();
                }
                if(string.IsNullOrWhiteSpace(searchText))
                    return Ok(userIntrest.Select(_ => _.InterestName).ToList());
                else
                    return Ok(userIntrest.Where(_ => _.InterestName.Contains(searchText,StringComparison.OrdinalIgnoreCase)).Select(_ => _.InterestName).ToList());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpGet(Name = "Add-Intrest")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddInterests()
        {
            try
            {
                await repository.AddUserInterests();
                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SendVerifyOtp(string email)
        {
            try
            {
                string otp = new Random().Next(100000, 999999).ToString();
                await repository.AddUserOtp(email.ToLower(), otp);
                await SendGridService.SendEmail("Verification OTP", "Your verification code is " + otp,  email);
                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet(Name = "VerifyUser")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> VerifyUser(string email,string verificationCode)
        {
            try
            {
                var otp = await repository.GetUserOtp(email.ToLower());
                if (otp != null && otp.Otp.Equals(verificationCode, StringComparison.OrdinalIgnoreCase))
                {
                    await repository.GetUserOtp(email.ToLower(),true);
                    return Ok(true);
                }
                return Ok(false);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
