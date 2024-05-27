using System.ComponentModel.DataAnnotations;

namespace KHCrafts.Models
{
    public class SubmitContactForm
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Message { get; set; }
    }
}
