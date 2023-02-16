using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Tattel.WebApi.Persistence;
using Tattel.WebApi.DataModels;
using Tattel.WebApi.Interfaces;
using Hangfire;
using Tattel.WebApi.Services;
using Tattel.WebApi.DTO;
using Newtonsoft.Json;

namespace Tattel.WebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public class ConversationController : Controller
    {
        private readonly IConversationRepository repository;
        private readonly IUserRepository userRepository;
        public ConversationController(IConversationRepository repository, IUserRepository userRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
        }

        [HttpPost("AddProposal")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddProposal(string senderId, string receiverId)
        {
            if (string.IsNullOrWhiteSpace(senderId) || string.IsNullOrWhiteSpace(receiverId)) return BadRequest();

            try
            {
                if (await repository.IsConversationsExist(senderId, receiverId))
                    return Conflict("Proposal already exist");

                var conversation = new Conversation()
                {
                    CreatedAt = DateTime.UtcNow,
                    Id = Guid.NewGuid().ToString(),
                    SendUserId = senderId,
                    RecipientUserId = receiverId,
                    Messages = new List<Message>(),
                    IsAccepted = null
                };
                await repository.Add(conversation);
                BackgroundJob.Schedule(() => NotificationService.send("New Proposal", "You have a new proposal", receiverId, "create",""), DateTime.UtcNow);
                return Ok();
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut("UpdateProposalResponse")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProposalResponse(string proposalId, bool response)
        {
            if (string.IsNullOrWhiteSpace(proposalId)) return BadRequest();
            try
            {
                if(await repository.UpdateResponse(proposalId, response))
                {
                    if(!response)
                        return NoContent();
                    var prop = await repository.GetProposalById(proposalId);
                    try
                    {
                        BackgroundJob.Schedule(() => NotificationService.send( "Your proposal has been accepted", "Proposal Accepted", prop.SendUserId, "accept", proposalId), DateTime.UtcNow);
                        BackgroundJob.Schedule(() => NotificationService.send( "5 Minutes Left", "Alert", prop.SendUserId, "alert", proposalId), DateTime.UtcNow.AddMinutes(15));
                        BackgroundJob.Schedule(() => NotificationService.send( "5 Minutes Left", "Alert", prop.RecipientUserId, "alert", proposalId), DateTime.UtcNow.AddMinutes(15));
                        BackgroundJob.Schedule(() => NotificationService.send( "20 minutes time finished", "Chat Ended", prop.SendUserId, "over", proposalId), DateTime.UtcNow.AddMinutes(20));
                        BackgroundJob.Schedule(() => NotificationService.send( "20 minutes time finished","Chat Ended", prop.SendUserId, "over", proposalId), DateTime.UtcNow.AddMinutes(20));
                    }
                    catch (Exception) { }
                    return NoContent();
                }
                return BadRequest("Server did something wrong");
            }
            catch(Exception w)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("AddMessage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddMessage(string proposalId, [FromBody] Message conversation)
        {
            if (conversation == null || ModelState.IsValid == false) return BadRequest();

            try
            {
                if(await repository.AddMessage(conversation, proposalId))
                {
                    var sender = await userRepository.Get(conversation.SenderUserId);
                    BackgroundJob.Schedule(() => NotificationService.send(conversation.MessageText, sender.Name, conversation.ReceiverUserId, "message",JsonConvert.SerializeObject(conversation)), DateTime.UtcNow);
                    return Ok();
                }
                return BadRequest("Server did something wrong");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetMessages")]
        [ProducesResponseType(200, Type = typeof(List<Message>))]
        public async Task<IActionResult> GetMessages(string proposalId ,int page = 1)
        {
            try
            {
                var messages = await repository.GetAllMessages(proposalId);
                if(messages != null && messages.Count > 0)
                {
                    messages = messages.OrderBy(_ => _.TimeStamp).ToList();
                    return Ok(messages.Skip(page-1 * 50).Take(50).ToList());
                }
                return Ok(new List<Message>());
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("GetProposals")]
        [ProducesResponseType(200, Type = typeof(List<ProposalDto>))]
        public async Task<IActionResult> GetProposals(string userId)
        {
            try
            {
                var proposals = await repository.GetAllProposals(userId);
                if(proposals != null)
                    return Ok(proposals);               
                return Ok(new List<ProposalDto>());
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        [HttpPost("SendMeetProposal")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult SendMeetProposal(string senderId, string recipientId)
        {
            if (string.IsNullOrWhiteSpace(senderId) || string.IsNullOrWhiteSpace(recipientId)) return BadRequest();
            try
            {
                    BackgroundJob.Schedule(() => NotificationService.send("You have received a meeting proposal", "New Meeting Proposal", recipientId, "meet-proposal", senderId), DateTime.UtcNow);
                    return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AcceptMeetProposal")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AcceptMeetProposal(string proposalSenderId, string proposalRecipientId)
        {
            if (string.IsNullOrWhiteSpace(proposalSenderId) || string.IsNullOrWhiteSpace(proposalRecipientId)) return BadRequest();
            try
            {
                if(!string.IsNullOrWhiteSpace(await repository.AddNewMeeting(proposalSenderId, proposalRecipientId)))
                {
                    BackgroundJob.Schedule(() => NotificationService.send("You meeting proposal has been accepted", "Meeting Proposal Accepted", proposalSenderId, "meet-accept", proposalRecipientId), DateTime.UtcNow);
                    return Ok();
                }
                throw new Exception("Internal Server Error Occurred");
              }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
