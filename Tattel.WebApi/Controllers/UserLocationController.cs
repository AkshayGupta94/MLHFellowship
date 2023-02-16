using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Tattel.WebApi.Persistence;
using Tattel.WebApi.DataModels;
using Tattel.WebApi.Interfaces;

namespace Tattel.WebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public class UserLocationController : Controller
    {
        private readonly IUserLocationRepository repository;

        public UserLocationController(IUserLocationRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("{id}", Name = "GetUserLocationLocation")]
        [ProducesResponseType(200, Type = typeof(UserLocation))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var userLocation = await repository.Get(id);
                if (userLocation == null)
                {
                    return NotFound();
                }

                return Ok(userLocation);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<UserLocation>))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userLocations = await repository.Get();
                return Ok(userLocations);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Add([FromBody] UserLocation userLocation)
        {
            if (userLocation == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                await repository.Add(userLocation);
                return CreatedAtAction(nameof(Get), new { userId = userLocation.UserId }, userLocation);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update([FromBody] UserLocation userLocation)
        {
            if (userLocation == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                await repository.Update(userLocation);
                return NoContent();
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await repository.Delete(id);
                return NoContent();
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                await repository.Delete();
                return NoContent();
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
