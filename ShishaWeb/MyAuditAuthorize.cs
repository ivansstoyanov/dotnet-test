using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ShishaWeb.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShishaWeb
{
    public class MyAuditAuthorize
    {
        private readonly RequestDelegate _next;
        public MyAuditAuthorize(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var auditProvider = (IAuditProvider)httpContext.RequestServices.GetService(typeof(IAuditProvider));

            var claimsIdentity = httpContext.User.Identity as ClaimsIdentity;
            foreach (Claim c in claimsIdentity.Claims)
            {
                if (c.Type == "Email")
                {
                    auditProvider.CurrentUserEmail = c.Value;
                }
                else if (c.Type == "Id")
                {
                    auditProvider.CurrentUserId = c.Value;
                }
            }

            await _next(httpContext);
        }
    }

    public static class CustomMiddleware
    {
        public static IApplicationBuilder UseMyAuditAuthorize(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyAuditAuthorize>();
        }
    }
}
