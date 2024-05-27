using KHCrafts.Data;
using KHCrafts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KHCrafts.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KHCraftsContext _context;

        public HomeController(ILogger<HomeController> logger, KHCraftsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Product.ToList();
            return View(products);
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Welcome()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitContactForm(SubmitContactForm model)
        {
            if (ModelState.IsValid)
            {
                TempData["UserInput"] = model;

                // Redirect to the "ThankYou" action
                return RedirectToAction("ThankYou");
            }

            return View("Contact", model); 
        }

        public IActionResult ThankYou()
        {
            // Retrieve the user input from TempData
            var model = TempData["UserInput"] as SubmitContactForm;

            // Pass the user input to the view
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}
