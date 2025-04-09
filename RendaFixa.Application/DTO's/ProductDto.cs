namespace FixedIncome.Application.DTO_s
{
    public class ProductDto()
    {
        public int Id { get; set; }
        public string? BondAsset { get; set; } 
        public string? Index { get; set; }
        public double Tax { get; set; } 
        public string? IssuerName { get; set; }
        public decimal UnitPrice { get; set; } 
        public int Stock { get; set; }
    }
}
