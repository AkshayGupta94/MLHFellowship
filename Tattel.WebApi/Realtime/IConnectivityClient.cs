using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tattel.WebApi.DataModels;

namespace Tattel.WebApi.Realtime
{
    public interface IConnectivityClient
    {
        Task ReceiveMessage(string message);
    }
}
