namespace ShishaWeb.Models
{
    public class TabaccoDTO : BaseAuditDTO
    {
        public string Id { get; set; }

        public int InternalId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Picture { get; set; }

        public double Power { get; set; }
    }
}
