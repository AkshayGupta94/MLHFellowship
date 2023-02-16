using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Serilog;
using System.Diagnostics;


namespace Tattel.WebApi.Middleware
{
    public class RequestResponseLogHandler : IActionFilter
    {
        RequestResponseData requestResponseData;
        Stopwatch stopWatch;

        public RequestResponseLogHandler()
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var controllerName = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;

            requestResponseData = new RequestResponseData
            {
                Path = ($"{controllerName}/{actionName}").ToLower(),
                Method = context.HttpContext.Request.Method,
                ClientIp = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                Timestamp = DateTime.Now,
                UserId = context.HttpContext.User.Identity.Name
            };

            stopWatch = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            stopWatch.Stop();

            var statusCode = (context.Result as ObjectResult)?.StatusCode ?? -1;
            requestResponseData.StatusCode = statusCode;
            requestResponseData.ResponseTime = stopWatch.ElapsedMilliseconds;

            Log.Information("{@requestResponseData}", requestResponseData);
        }
    }
}
