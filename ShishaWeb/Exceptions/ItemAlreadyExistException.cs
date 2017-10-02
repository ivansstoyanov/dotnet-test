using System;
using System.Net;

namespace ShishaWeb.Exceptions
{
    public class ItemAlreadyExistException : BaseException
    {
        public ItemAlreadyExistException(object item, string code = null, string message = null, Exception innerException = null) : base(message, innerException)
        {
            this.ErrorCode = (int)HttpStatusCode.Conflict;
            this.ErrorMessage = $"{item.GetType()} already exists";
            this.ErrorMessageCode = code ?? MessageCodes.ALREADY_EXISTS;
        }
    }
}
