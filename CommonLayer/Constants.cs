using CommonLayer.DTOs;
using CommonLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer
{
    public class Constants
    {
        public static class JWTConfiguration
        {
            public static readonly string JWTIssuer = Utils._config["Jwt:Issuer"];
            public static readonly string JWTAudience = Utils._config["Jwt:Audience"];
            public static readonly string JWTKey = Utils._config["Jwt:Key"];
        }

        public static class TokenManager
        {
            public static List<TokenDTO> Tokens = new List<TokenDTO>();
        }

        public static class SwaggerConfiguration
        {
            public static readonly string SwaggerEndPointURL = Utils._config["SwaggerConfigurations:SwaggerEndPointURL"];
            public static readonly string SwaggerEndPointName = Utils._config["SwaggerConfigurations:SwaggerEndPointName"];
        }

     
        public static class Statuses
        {
            public static readonly string Pending = "Pending";
            public static readonly string Completed = "Completed";
        }



        public static class OtherConstants
        {
            public static string messageType = MessageType.Success;
            public static string responseMsg = "";
            public static bool isSuccessful;

            public static void Clear()
            {
                messageType = "";
                responseMsg = "";
                isSuccessful = false;
            }
        }

        public static class MessageType
        {
            public static readonly string Success = "success";
            public static readonly string Error = "error";
            public static readonly string Warning = "warning";
            public static readonly string Info = "info";
        }

     
        public static class CustomClaims
        {
            public static readonly string TenantId = "TenantId";
            public static readonly string UserId = "UserId";
            public static readonly string Role = "Role";
        }

        public static class Roles
        {
            public static readonly string Admin = "Admin";
            public static readonly string CompanyAdmin = "CompanyAdmin";
            public static readonly string User = "User";
            public static readonly string Client = "Client";
        }

        public static class ClaimValue
        {
            public static readonly string View = "View";
            public static readonly string Create = "Create";
            public static readonly string Edit = "Edit";
            public static readonly string Delete = "Delete";
        }

        public static class ClaimType
        {
            public static readonly string Contacts = "Contacts";
        }

        public static class SortOrder
        {
            public static readonly string ascend = "ascend";
            public static readonly string descend = "descend";
        }

        public static class ResponseStrings
        {
            public static readonly string NotFound = "Not Found";
            public static readonly string Success = "Success";
            public static readonly string Unauthorized = "You are currently blocked. Please try to contact customer support.";
            public static readonly string InvalidCredentials = "Invalid username or password";
            public static readonly string InvalidPassword = "Invalid current password";
        }


    }
}
