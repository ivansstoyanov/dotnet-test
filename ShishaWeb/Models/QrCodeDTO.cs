namespace ShishaWeb.Models
{
    public class QrCodeDTO : BaseAuditDTO
    {
        public string Id { get; set; }

        public string Value { get; set; }

        public int Count { get; set; }
    }
}
