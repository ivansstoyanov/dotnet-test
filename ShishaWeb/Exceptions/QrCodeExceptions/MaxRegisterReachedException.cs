using System;
using System.Net;

namespace ShishaWeb.Exceptions.QrCodeExceptions
{
    public class MaxRegisterReachedException : BaseException
    {
        public MaxRegisterReachedException(object item, string code = null, string message = null, Exception innerException = null) : base(message, innerException)
        {
            this.ErrorCode = (int)HttpStatusCode.Conflict;
            this.ErrorMessage = $"{item.GetType()} with id - {this.GetObjectId(item)} - reached maximum registrations";
            this.ErrorMessageCode = code ?? MessageCodes.MAX_REGISTER_REACHED;
        }        
    }
}
