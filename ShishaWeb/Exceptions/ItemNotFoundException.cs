using System;
using System.Net;

namespace ShishaWeb.Exceptions
{
    public class ItemNotFoundException : BaseException
    {
        public ItemNotFoundException(string message, Exception innerException = null) : base(message, innerException)
        {
            this.ErrorCode = (int)HttpStatusCode.NotFound;
        }
    }
}
