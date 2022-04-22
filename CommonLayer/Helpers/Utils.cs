using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using static CommonLayer.Constants;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using CommonLayer.DTOs;

namespace CommonLayer.Helpers
{
    public class Utils
    {
        public static IConfigurationRoot _config { get; set; }
        public static string NewAccessToken { get; set; }

        public static string GetUserId(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.User.GetUserId();
        }

        public static string GetTenantId(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.User.GetTenantId();
        }

        public static string GetRole(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.User.GetRole();
        }

        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Guid.NewGuid().ToString()
                      + Path.GetExtension(fileName);
        }

        public static string GetQueryParams(HttpRequest request)
        {
            string result = "";
            if (!string.IsNullOrEmpty(request.QueryString.Value))
            {
                string q = request.QueryString.Value.Split("=")[1];
                result = HttpUtility.UrlDecode(q);
            }
            return result;
        }

       
        public static List<string> GetClaimTypes()
        {
            List<string> list = new List<string>();
            list.Add(ClaimType.Contacts);
            return list;
        }
    }
}