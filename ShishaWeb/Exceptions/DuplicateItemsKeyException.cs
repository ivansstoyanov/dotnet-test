using System;
using System.Net;

namespace ShishaWeb.Exceptions
{
    public class DuplicateItemsKeyException : BaseException
    {
        public DuplicateItemsKeyException(object item, string code = null, string message = null, Exception innerException = null) : base(message, innerException)
        {
            this.ErrorCode = (int)HttpStatusCode.Conflict;
            this.ErrorMessage = $"{item.GetType()} has duplicate tobaccos";
            this.ErrorMessageCode = code ?? MessageCodes.DUPLICATE_TOBACCOS;
        }
    }
}
