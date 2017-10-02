namespace ShishaWeb.Services.Interfaces
{
    public interface IAuditProvider
    {
        string CurrentUserId { get; set; }

        string CurrentUserEmail { get; set; }
    }
}
