using System;
using System.Net;

namespace ShishaWeb.Exceptions.QrCodeExceptions
{
    public class InvalidIdException : BaseException
    {
        public InvalidIdException(object item, string code = null, string message = null, Exception innerException = null) : base(message, innerException)
        {
            this.ErrorCode = (int)HttpStatusCode.Conflict;
            this.ErrorMessage = $"{item.GetType()} with id - {this.GetObjectId(item)} - is invalid";
            this.ErrorMessageCode = code ?? MessageCodes.INVALID_ID;
        }        
    }
}
