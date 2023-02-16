using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Driver;
using Tattel.WebApi.DataModels;
using Tattel.WebApi.DTO;

namespace Tattel.WebApi.Interfaces
{
    public interface IConversationRepository
    {
        Task Add(Conversation conversation);
        Task<bool> IsConversationsExist(string senderId, string receiverId);
        Task<bool> UpdateResponse(string proposalId, bool response);
        Task<bool> AddMessage(Message message, string proposalId);
        Task<List<Message>> GetAllMessages(string proposalId);
        Task<List<ProposalDto>> GetAllProposals(string userId);
        Task<Conversation> GetProposalById(string proposalId);
        Task<string> AddNewMeeting(string senderId, string recipientId);
    }
}
