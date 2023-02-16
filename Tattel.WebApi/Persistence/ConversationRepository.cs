using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

using Tattel.WebApi.DataModels;
using Tattel.WebApi.Interfaces;
using Tattel.WebApi.DTO;

namespace Tattel.WebApi.Persistence
{
  
    public class ConversationRepository : IConversationRepository
    {
        private readonly TattelContext context;
        public ConversationRepository()
        {
            context = new TattelContext();
        }

        public async Task Add(Conversation conversation)
        {
            var filter = Builders<Conversation>.Filter.Eq("_id", conversation.Id);
            var result = await context.Conversations.Find(filter).FirstOrDefaultAsync();

            if (result == null)
            {
                await context.Conversations.InsertOneAsync(conversation);
            }
        }
        public async Task<bool> IsConversationsExist(string senderId, string receiverId)
        {
           if(await context.Conversations.Find(x => (Equals(x.SendUserId, senderId) && Equals(x.RecipientUserId, receiverId)) || (Equals(x.RecipientUserId, senderId) 
                    && Equals(x.SendUserId, receiverId))).FirstOrDefaultAsync() != null)
                return true;

            return false;
        }

        public async Task<bool> UpdateResponse(string proposalId, bool response)
        {
           var res =  await context.Conversations.UpdateOneAsync(
                Builders<Conversation>.Filter.Eq(_ => _.Id,proposalId),
                Builders<Conversation>.Update.Set(_ => _.IsAccepted, response).
                                               Set(_ => _.InitiatedAt, DateTime.UtcNow));
            if (res.IsAcknowledged && res.ModifiedCount > 0)
                return true;
            return false;
        }

        public async Task<bool> AddMessage(Message message, string proposalId)
        {
            var filter = Builders<Conversation>.Filter.Eq(_ => _.Id, proposalId);
            var result = await context.Conversations.Find(filter).FirstOrDefaultAsync();

            if(result != null && result.InitiatedAt.AddMinutes(20) >= DateTime.UtcNow)
            {
                message.TimeStamp = DateTime.UtcNow;
                result.Messages.Add(message);
                var res = await context.Conversations.UpdateOneAsync(Builders<Conversation>.Filter.Eq(_ => _.Id, proposalId), 
                                                                     Builders<Conversation>.Update.Set(_ => _.Messages, result.Messages));
                if (res.IsAcknowledged && res.ModifiedCount > 0)
                    return true;
                return false;
            }
            if (result == null)
            {
                throw new Exception("No Proposal exist");
            }
            throw new Exception("20 Minutes Times is over");
        }

        public async Task<List<Message>> GetAllMessages(string proposalId)
        {
            var filter = Builders<Conversation>.Filter.Eq(_ => _.Id, proposalId);
            var result = await context.Conversations.Find(filter).FirstOrDefaultAsync();
            if (result != null)
                return result.Messages;
            return null;
        }

        public async Task<List<ProposalDto>> GetAllProposals(string userId)
        {
            var result = context.Conversations.AsQueryable().Where(_ => _.RecipientUserId == userId || _.SendUserId == userId).ToList();
                        
            List<string> ids = result.Where(_ => _.RecipientUserId != userId).Select(x => x.RecipientUserId).ToList();
            ids.AddRange(result.Where(_ => _.SendUserId != userId).Select(x => x.SendUserId).ToList());
            ids.Add(userId);

            var recepient = context.Users.AsQueryable().Where(_ => ids.Contains(_.Id)).ToList();
            if (result != null)
                return result.Select(x => Transform(x, recepient)).ToList();
            return null;
        }

        public async Task<Conversation> GetProposalById(string proposalId)
        {
            var filter = Builders<Conversation>.Filter.Eq(_ => _.Id, proposalId);
            return await context.Conversations.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<string> AddNewMeeting(string senderId, string recipientId)
        {
            var record = new MeetingRecord
            {
                CreatedDate = DateTime.UtcNow,
                RecipientId = recipientId,
                SenderId = senderId,
                Id = Guid.NewGuid().ToString()
            };
            await context.MeetingRecords.InsertOneAsync(record);
            return record.Id;
        }
        private ProposalDto Transform (Conversation c, List<User> u)
        {
            return new ProposalDto
            {
                CreatedAt = c.CreatedAt,
                Id = c.Id,
                InitiatedAt = c.InitiatedAt,
                IsAccepted = c.IsAccepted,
                Messages = c.Messages,
                RecipientUserId = c.RecipientUserId,
                SendUserId = c.SendUserId,
                SenderName = u.Where(_ => _.Id == c.SendUserId).FirstOrDefault() == null ? "" : u.Where(_ => _.Id == c.SendUserId).FirstOrDefault().Name,
                SenderProfilePic = u.Where(_ => _.Id == c.SendUserId).FirstOrDefault() == null ? "" : u.Where(_ => _.Id == c.SendUserId).FirstOrDefault().ProfilePicPath,
                SenderInterests = u.Where(_ => _.Id == c.SendUserId).FirstOrDefault() == null ? "" : u.Where(_ => _.Id == c.SendUserId).FirstOrDefault().Interests,
                RecipientName = u.Where(_ => _.Id == c.SendUserId).FirstOrDefault() == null ? "" : u.Where(_ => _.Id == c.RecipientUserId).FirstOrDefault().Name,
                RecipientProfilePic = u.Where(_ => _.Id == c.SendUserId).FirstOrDefault() == null ? "" : u.Where(_ => _.Id == c.RecipientUserId).FirstOrDefault().ProfilePicPath
            };
        }
    }
}

