using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Tattel.WebApi.Realtime
{
    public class UserIdProvider: IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var userId = connection.User?.FindFirst(c => c.Type == "id")?.Value;
            return userId;
        }
    }
}
