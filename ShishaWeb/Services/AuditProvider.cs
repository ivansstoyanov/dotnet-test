using ShishaWeb.Services.Interfaces;

namespace ShishaWeb.Services
{
    public class AuditProvider : IAuditProvider
    {
        public string CurrentUserId { get; set; }

        public string CurrentUserEmail { get; set; }
    }
}
