using System;
using System.Collections.Generic;

namespace SharedObjects.Commons
{
    public class ResponseResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<string> Notification { get; set; }
        public ResponseResult()
        {

        }
        public ResponseResult(int statusCode)
        {
            StatusCode = statusCode;
        }
        public ResponseResult(int statusCode, List<string> notifications)
        {
            StatusCode = statusCode;
            Notification = notifications;
        }
        public ResponseResult(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
