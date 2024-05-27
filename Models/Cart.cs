namespace KHCrafts.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(Product product, int quantity)
        {
            var cartItem = Items.Find(i => i.ProductId == product.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem { Product = product, ProductId = product.ProductId, Quantity = quantity });
            }
        }

        public void RemoveItem(int productId)
        {
            var cartItem = Items.Find(i => i.ProductId == productId);
            if (cartItem != null)
            {
                Items.Remove(cartItem);
            }
        }
    }
}
