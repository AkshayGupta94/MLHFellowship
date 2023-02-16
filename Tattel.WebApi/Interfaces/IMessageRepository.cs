using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MongoDB.Driver;
using Tattel.WebApi.DataModels;

namespace Tattel.WebApi.Interfaces
{
    public interface IMessageRepository
    {

        Task<Message> Get(string senderId, string receiverId);
        Task<List<Message>> Get();
        Task Add(Message message);
        Task<UpdateResult> Update(Message message);
        Task<DeleteResult> Delete(string senderId, string receiverId);
        Task<DeleteResult> Delete();
    }
}
