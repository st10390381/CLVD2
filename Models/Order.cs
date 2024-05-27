namespace KHCrafts.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        // Foreign keys
        public string CustomerId { get; set; }
        public int ProductId { get; set; }

        // Navigation properties
        public Product? Product { get; set; }
        public Customer? Customer { get; set; }
    }
}
