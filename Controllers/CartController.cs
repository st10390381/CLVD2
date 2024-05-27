using KHCrafts.Data;
using KHCrafts.Models;
using KHCrafts.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KHCrafts.Controllers
{
    public class CartController : Controller
    {
        private readonly KHCraftsContext _context;
        private static Cart _cart = new Cart(); // In-memory cart
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(KHCraftsContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _context.Product.Find(productId);
            if (product != null)
            {
                _cart.AddItem(product, 1);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            _cart.RemoveItem(productId);

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            return View(new CheckoutViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool paymentSuccess = ProcessPayment(model);

                if (paymentSuccess)
                {
                    var user = await _userManager.GetUserAsync(User);

                    var order = new Order
                    {
                        TotalAmount = _cart.Items.Sum(item => item.Product.Price * item.Quantity),
                        OrderDate = DateTime.UtcNow,
                        CustomerId = user.Id,
                        ProductId = _cart.Items.First().ProductId
                    };

                    _context.Order.Add(order);
                    await _context.SaveChangesAsync();

                    _cart = new Cart();

                    TempData["Message"] = "Thank you for your order!";
                    return RedirectToAction("OrderConfirmation");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Payment processing failed. Please try again.");
                }
            }

            return View(model);
        }

        private bool ProcessPayment(CheckoutViewModel model)
        {
            return true;
        }

        public IActionResult OrderConfirmation()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
    }
}
