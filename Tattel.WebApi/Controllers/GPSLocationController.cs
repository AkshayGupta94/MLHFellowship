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
    public class GPSLocationController : Controller
    {
        private readonly IGPSLocationRepository repository;

        public GPSLocationController(IGPSLocationRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("{id}", Name = "GetGPSLocation")]
        [ProducesResponseType(200, Type = typeof(GPSLocation))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var gpsLocation = await repository.Get(id);
                if (gpsLocation == null)
                {
                    return NotFound();
                }

                return Ok(gpsLocation);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<GPSLocation>))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var gpsLocations = await repository.Get();
                return Ok(gpsLocations);
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
        public async Task<IActionResult> Add([FromBody] GPSLocation gpsLocation)
        {
            if (gpsLocation == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                await repository.Add(gpsLocation);
                return CreatedAtAction(nameof(Get), new { locationId = gpsLocation.LocationId }, gpsLocation);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update([FromBody] GPSLocation gpsLocation)
        {
            if (gpsLocation == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                await repository.Update(gpsLocation);
                return NoContent();
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string locationId)
        {
            try
            {
                await repository.Delete(locationId);
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
