namespace KHCrafts.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Category { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
    }
}
