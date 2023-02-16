using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using Tattel.WebApi.Persistence;
using Tattel.WebApi.DataModels;

namespace Tattel.WebApi.Realtime
{
    [Authorize]
    public class ConnectivityHub : Hub<IConnectivityClient>
    {
        //private readonly IClientConnectionRepository _clientConnectionRepository;

        public ConnectivityHub()
        {
            //_clientConnectionRepository = new ClientConnectionRepository();
        }

        public async Task SendMessageToAUser(string userId, string message)
        {
            await Clients.User(userId).ReceiveMessage(message);
        }

        public async Task SendMessageToTestRoom(string testCentreId, string roomNo, string message)
        {
            await Clients.Group($"{testCentreId}_{roomNo}").ReceiveMessage(message);
        }

        public async Task SendMessageToTestCentre(string testCentreId, string message)
        {
            await Clients.Group(testCentreId).ReceiveMessage(message);
        }

        public async Task SendMessageToAllCentres(string message)
        {
            await Clients.All.ReceiveMessage(message);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirst(c => c.Type == "id")?.Value;

            var httpContext = Context.GetHttpContext();
            var testSessionId = httpContext.Request.Query["sessionid"];
            var testCentreId = httpContext.Request.Query["centreid"];

            if (!String.IsNullOrEmpty(testSessionId) && !String.IsNullOrEmpty(testCentreId))
            {
                int roomNo = -1;
                int.TryParse(httpContext.Request.Query["roomno"].ToString(), out roomNo);

                try
                {

                }
                catch
                {
                }
            }
            else
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "ControlRoom"); //Control Room
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string disconnectReason = null;

            if(exception != null)
            {
                disconnectReason = exception.Message;
            }

            try
            {
                //var connection = await _clientConnectionRepository.GetAsync(Context.ConnectionId);
                //if (connection != null)
                //{
                //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.TestCentreId); 
                //    await _clientConnectionRepository.DisconnectAsync(Context.ConnectionId, intentionalDisconnect, disconnectReason);
                //}
            }
            catch
            {
                   
            }

            await base.OnDisconnectedAsync(exception);
        }
  
    }
}
 
