using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShishaWeb.Exceptions;
using ShishaWeb.Exceptions.QrCodeExceptions;
using ShishaWeb.Models;

namespace ShishaWeb.Filters
{
    public class QrCodeManagementExceptionFilterAttribute : ExceptionFilterAttribute
    {
        //TODO add logging

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is InvalidIdException ||
                context.Exception is MaxRegisterReachedException ||
                context.Exception is QrCodeExpiredException)
            {
                var response = new RequestResult()
                {
                    State = RequestState.Failed,
                    Message = (context.Exception as BaseException).ErrorMessage,
                    MessageCode = (context.Exception as BaseException).ErrorMessageCode,
                };

                context.Result = new ObjectResult(response)
                {
                    StatusCode = (context.Exception as BaseException).ErrorCode,
                    DeclaredType = typeof(RequestResult)
                };

                context.ExceptionHandled = true;
            }
            else
            {
                context.ExceptionHandled = false;
            }
        }
    }
}
