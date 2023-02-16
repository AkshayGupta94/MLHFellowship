using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Driver;

using Tattel.WebApi.DataModels;
using Tattel.WebApi.Interfaces;

namespace Tattel.WebApi.Persistence
{
    public class MessageRepository : IMessageRepository
    {
        private readonly TattelContext context;

        public MessageRepository()
        {
            context = new TattelContext();
        }

        public async Task<Message> Get(string senderId, string receiverId)
        {
            var sender = Builders<Message>.Filter.Eq(x => x.SenderUserId, senderId);
            var receiver = Builders<Message>.Filter.Eq(x => x.ReceiverUserId, receiverId);

            return await context.Messages.Find(sender & receiver).FirstOrDefaultAsync();
        }

        public async Task<List<Message>> Get()
        {
            var filter = Builders<Message>.Filter.Empty;
            return await context.Messages.Find(filter).SortBy(i => i.SenderUserId).ToListAsync();
        }

        public async Task Add(Message Message)
        {
            var sender = Builders<Message>.Filter.Eq(x => x.SenderUserId, Message.SenderUserId);
            var receiver = Builders<Message>.Filter.Eq(x => x.ReceiverUserId, Message.ReceiverUserId);
            var result = await context.Messages.Find(sender & receiver).FirstOrDefaultAsync();

            if (result == null)
            {
                await context.Messages.InsertOneAsync(Message);
            }
        }

        public async Task<UpdateResult> Update(Message message)
        {
            var sender = Builders<Message>.Filter.Eq(x => x.SenderUserId, message.SenderUserId);
            var receiver = Builders<Message>.Filter.Eq(x => x.ReceiverUserId, message.ReceiverUserId);

            var update = Builders<Message>.Update
                                 .Set(s => s.MessageText, message.MessageText)
                                 .Set(s => s.TimeStamp, message.TimeStamp);

            return await context.Messages.UpdateOneAsync(sender & receiver, update);
        }

        public async Task<DeleteResult> Delete(string senderId, string receiverId)
        {
            var sender = Builders<Message>.Filter.Eq(x => x.SenderUserId, senderId);
            var receiver = Builders<Message>.Filter.Eq(x => x.ReceiverUserId, receiverId);
            return await context.Messages.DeleteOneAsync(sender & receiver);
        }

        public async Task<DeleteResult> Delete()
        {
            var filter = Builders<Message>.Filter.Empty;
            return await context.Messages.DeleteManyAsync(filter);
        }

    }
}
