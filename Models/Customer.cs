namespace KHCrafts.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int PhoneNumber { get; set; }
        public string? Email { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
