using System;
using System.Net;

namespace ShishaWeb.Exceptions
{
    public class BaseException : Exception
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorMessageCode { get; set; }

        public BaseException(string message = null, Exception innerException = null) : base(message, innerException)
        {
            this.ErrorCode = (int)HttpStatusCode.BadRequest;
            this.ErrorMessage = message;
        }

        protected string GetObjectId(object item)
        {
            return item.GetType().GetProperty("Id")?.GetValue(item).ToString();
        }
    }
}
