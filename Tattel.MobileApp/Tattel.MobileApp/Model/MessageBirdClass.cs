using System;

using Tattel.MobileApp.Interfaces;

using MessageBird;
using MessageBird.Objects;
using MessageBird.Exceptions;

using Microsoft.Extensions.Configuration;



namespace Tattel.MobileApp.Model
{
    public class MessageBirdClass : IMessageBird
    {
        public Client messageClient { get; set; }
        public string verificationId { get; set; }

        private readonly IConfigurationClass _configuration;

        public MessageBirdClass(IConfigurationClass configurationClass)
        {
            _configuration = configurationClass;

            // Initialize the MessageBird API
            messageClient = Client.CreateDefault(_configuration.messageBirdApiKey);
        }


        /// <summary>
        /// Method that handles Sending OTP for user SMS Verification
        /// </summary>
        /// <param name="phoneNumber"></param>
        public string SendOTP(string phoneNumber)
        {
            try
            {
                VerifyOptionalArguments optionalArguments = new VerifyOptionalArguments
                {
                    Originator = phoneNumber,
                    Type = MessageType.Sms
                };

                // Default timeout for OTP entry is 30 secs so passing in null input
                var verify = messageClient.CreateVerify(phoneNumber, optionalArguments);
                verificationId = verify.Id;
            }
            catch (ErrorException errorException)
            {
                Console.WriteLine(errorException.Message);
            }
            return verificationId;
        }

        /// <summary>
        /// Method to handle verification of OTP
        /// </summary>
        /// <param name="verificationCode"></param>
        public void VerifyOTP(string verificationCode)
        {
            try
            {
                // Call SendVerifyToken to verify the code entered in txtCode.Text matches the code
                // sent to the user in the CreateVerify call in SendOTP. Here, the words "token" and "code"
                // mean the same thing.
                var a= messageClient.SendVerifyToken(verificationId, verificationCode);
                
            }
            catch(ErrorException errorException)
            {
                Console.WriteLine(errorException.Message);
            }
        }
    }
}
