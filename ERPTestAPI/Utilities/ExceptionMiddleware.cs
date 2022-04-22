using CommonLayer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using UnitOfWork.DIHelper;
using static CommonLayer.Constants;

namespace Casolve.Secure.Api.Utilities
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //public async Task InvokeAsync(HttpContext httpContext)
        //{
        //    try
        //    {
        //        OtherConstants.Clear();
        //        await _next(httpContext);
        //    }
        //    catch (Exception ex)
        //    {
        //        await HandleExceptionAsync(httpContext, ex);
        //    }
        //}

        //private Task HandleExceptionAsync(HttpContext context, Exception exception)
        //{
        //    using var serviceScope = ServiceActivator.GetScope();
        //    serviceScope.ServiceProvider.GetRequiredService<IErrorLogsRepo>().Post(new ErrorLog()
        //    {
        //        ExceptionMessage = exception.Message,
        //        StackTrace = exception.StackTrace,
        //        Source = exception.Source,
        //        CreatedDate = DateTime.UtcNow
        //    }, true);

        //    context.Response.ContentType = "application/json";
        //    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    return context.Response.WriteAsync(JsonConvert.SerializeObject(new BaseResponse()
        //    {
        //        errorMessage = exception.Message,
        //        errorStackTrace = exception.StackTrace,
        //        dynamicResult = null,
        //        isSuccessfull = false,
        //        messageType = "Failed",
        //        statusCode = 500,
        //        message = "Something went wrong. Kindly contact system administration!",
        //    }));
        //}
    }
}
