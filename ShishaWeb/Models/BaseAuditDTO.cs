namespace ShishaWeb.Models
{
    public class BaseAuditDTO
    {
        public string CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string LastModifiedAt { get; set; }

        public string LastModifiedBy { get; set; }
    }
}
