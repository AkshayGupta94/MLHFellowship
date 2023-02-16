using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Driver;
using Tattel.WebApi.DataModels;


namespace Tattel.WebApi.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Get(string id);
        Task<List<User>> Get();
        Task<string> Add(User user);
        Task<UpdateResult> Update(User user);
        Task<DeleteResult> Delete(string id);
        Task<DeleteResult> Delete();
        Task<string>UploadImageToStorage(Stream stream, string fileName);
        Task<UpdateResult> UpdateLocation(string userId, double latitude, double longitude);
        Task<List<User>> GetNearbyUsers(string userId, int count, int range);
        Task<UpdateResult> UpdateToken(string userId, string token);
        Task<string> GetToken(string userId);
        Task<UpdateResult> UpdateUserStatus(string userId, bool isOnline);
        Task<User> GetUserByEmail(string email);
        Task<List<UserInterest>> GetUserInterests();
        Task AddUserInterests();
        Task<string> AddUserOtp(string userEmail, string otp);
        Task<Verification_Otp> GetUserOtp(string userEmail, bool isDelete = false);
    }
}
