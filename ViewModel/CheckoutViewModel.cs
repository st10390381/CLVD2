using System.ComponentModel.DataAnnotations;

namespace KHCrafts.ViewModel
{
    public class CheckoutViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [RegularExpression(@"\d{16}", ErrorMessage = "Please enter a valid 16-digit card number.")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"(0[1-9]|1[0-2])\/?([0-9]{2})", ErrorMessage = "Please enter a valid expiry date in MM/YY format.")]
        public string ExpiryDate { get; set; }

        [Required]
        [RegularExpression(@"\d{3}", ErrorMessage = "Please enter a valid 3-digit CVV.")]
        public string CVV { get; set; }

        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
    }
}
