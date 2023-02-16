using MessageBird;

namespace Tattel.MobileApp.Interfaces
{
    public interface IMessageBird
    {
        public Client messageClient { get; set; }

        public string SendOTP(string phoneNumber);

        public void VerifyOTP(string code);
    }
}
