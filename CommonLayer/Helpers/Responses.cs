using CommonLayer.DTOs;
using System;
using System.Net;
using static CommonLayer.Constants;

namespace CommonLayer.Helpers
{
    public class Responses
    {
        public ResponseDTO<dynamic> NotFound()
        {
            return new ResponseDTO<dynamic>()
            {
                Status = Convert.ToInt32(HttpStatusCode.NotFound),
                Message = ResponseStrings.NotFound,
                Data = null
            };
        }

        public ResponseDTO<dynamic> Unauthorized()
        {
            return new ResponseDTO<dynamic>()
            {
                Status = Convert.ToInt32(HttpStatusCode.Unauthorized),
                Message = ResponseStrings.Unauthorized,
                Data = null
            };
        }
        public ResponseDTO<T> OK<T>(string message, T data)
        {
            return new ResponseDTO<T>()
            {
                Message = string.Format("{0}", message),
                Data = data
            };
        }

        public ResponseDTO<T> OKGet<T>(string message, T data)
        {
            return new ResponseDTO<T>()
            {
                Message = string.Format("{0} get successfully", message),
                Data = data
            };
        }

        public ResponseDTO<T> OKGetAll<T>(string message, T data)
        {
            return new ResponseDTO<T>()
            {
                Message = string.Format("{0}s get successfully", message),
                Data = data
            };
        }

        public ResponseDTO<T> OKAdded<T>(string message, T data)
        {
            return new ResponseDTO<T>()
            {
                Message = string.Format("{0} added successfully", message),
                Data = data
            };
        }

        public ResponseDTO<T> OKUpdated<T>(string message, T data)
        {
            return new ResponseDTO<T>()
            {
                Message = string.Format("{0} updated successfully", message),
                Data = data
            };
        }

        public ResponseDTO<T> OKDeleted<T>(string message, T data)
        {
            return new ResponseDTO<T>()
            {
                Message = string.Format("{0} deleted successfully", message),
                Data = data
            };
        }

        public ResponseDTO<T> NotFound<T>(string message, T data)
        {
            return new ResponseDTO<T>()
            {
                Status = Convert.ToInt32(HttpStatusCode.NotFound),
                Message = string.Format("This {0} not found in our database", message),
                Data = data
            };
        }

        public ResponseDTO<T> SomethingWentWrong<T>(string message, T data)
        {
            return new ResponseDTO<T>()
            {
                Status = 0,
                Message = string.Format("Something went wrong <b>{0}</b>", message),
                Data = data
            };
        }
    }
}
