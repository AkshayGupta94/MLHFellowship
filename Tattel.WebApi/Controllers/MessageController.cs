using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Tattel.WebApi.DataModels;
using Tattel.WebApi.Interfaces;


namespace Tattel.WebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]

    public class MessageController : Controller
    {
        private readonly IMessageRepository repository;

        public MessageController(IMessageRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        [ProducesResponseType(200, Type = typeof(Message))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string senderId, string receiverId)
        {
            try
            {
                var message = await repository.Get(senderId, receiverId);
                if (message == null)
                {
                    return NotFound();
                }

                return Ok(message);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Message>))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var messages = await repository.Get();
                return Ok(messages);
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
        public async Task<IActionResult> Add([FromBody] Message message)
        {
            if (message == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                await repository.Add(message);
                return CreatedAtAction(nameof(Get), new { senderId = message.SenderUserId }, message);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update([FromBody] Message message)
        {
            if (message == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                await repository.Update(message);
                return NoContent();
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string senderId, string receiverId)
        {
            try
            {
                await repository.Delete(senderId, receiverId);
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
