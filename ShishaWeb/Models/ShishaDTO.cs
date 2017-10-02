namespace ShishaWeb.Models
{
    public class ShishaDTO : BaseAuditDTO
    {
        public string Id { get; set; }

        public string Identifier { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string Tag { get; set; }

        public string Picture { get; set; }

        public double Power { get; set; }

        public int SmokedCount { get; set; }

        public bool IsPublic { get; set; }

        public bool IsFavourite { get; set; }

        public bool IsWish { get; set; }

        public bool IsSeasonal { get; set; }

        public TabaccoDTO[] Tabaccos { get; set; }

        public double TotalRating { get; set; }

        public double OwnRating { get; set; }
    }
}
