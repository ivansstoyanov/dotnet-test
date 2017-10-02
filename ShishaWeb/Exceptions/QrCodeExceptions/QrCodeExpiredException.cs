using System;
using System.Net;

namespace ShishaWeb.Exceptions.QrCodeExceptions
{
    public class QrCodeExpiredException : BaseException
    {
        public QrCodeExpiredException(object item, string code = null, string message = null, Exception innerException = null) : base(message, innerException)
        {
            this.ErrorCode = (int)HttpStatusCode.Conflict;
            this.ErrorMessage = $"{item.GetType()} with id - {this.GetObjectId(item)} - has expired";
            this.ErrorMessageCode = code ?? MessageCodes.CODE_EXPIRED;
        }        
    }
}
