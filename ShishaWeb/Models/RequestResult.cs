using System;

namespace ShishaWeb.Models
{
    public class RequestResult
    {
        public RequestState State { get; set; }
        public string Message { get; set; }
        public string MessageCode { get; set; }
        public Object Data { get; set; }
    }

    //TODO extract to Enumerations
    public enum RequestState
    {
        Failed = -1,
        NotAuth = 0,
        Success = 1
    }
}
