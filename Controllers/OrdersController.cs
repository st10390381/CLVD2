using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KHCrafts.Data;
using KHCrafts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KHCrafts.Controllers
{
    public class OrdersController : Controller
    {
        private readonly KHCraftsContext _context; 
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(KHCraftsContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            IQueryable<Order> ordersQuery = _context.Order.Include(o => o.Customer).Include(o => o.Product);

            if (!roles.Contains("Admin"))
            {
                ordersQuery = ordersQuery.Where(o => o.CustomerId == user.Id);
            }

            var orders = await ordersQuery.ToListAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            var orderQuery = _context.Order
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Where(o => o.OrderId == id);

            if (!roles.Contains("Admin"))
            {
                orderQuery = orderQuery.Where(o => o.CustomerId == user.Id);
            }

            var order = await orderQuery.FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            var orderHistory = await _context.Order
                .Where(oh => oh.OrderId == id)
                .Include(oh => oh.Customer)
                .ToListAsync();

            ViewData["OrderHistory"] = orderHistory;

            return View(order);
        }


        // GET: Orders/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("OrderId,TotalAmount,OrderDate,CustomerId,ProductId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", order.ProductId);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", order.ProductId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,TotalAmount,OrderDate,CustomerId,ProductId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", order.ProductId);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}

//        public OrdersController(KHCraftsContext context)
//        {
//            _context = context;
//        }

//        // GET: Orders
//        public async Task<IActionResult> Index()
//        {
//            var kHCraftsContext = _context.Order.Include(o => o.Product);
//            return View(await kHCraftsContext.ToListAsync());
//        }

//        // GET: Orders/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var order = await _context.Order
//                .Include(o => o.Product)
//                .FirstOrDefaultAsync(m => m.OrderId == id);
//            if (order == null)
//            {
//                return NotFound();
//            }

//            return View(order);
//        }

//        // GET: Orders/Create
//        public IActionResult Create()
//        {
//            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId");
//            return View();
//        }

//        // POST: Orders/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("OrderId,TotalAmount,OrderDate,CustomerId,ProductId")] Order order)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(order);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", order.ProductId);
//            return View(order);
//        }

//        // GET: Orders/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var order = await _context.Order.FindAsync(id);
//            if (order == null)
//            {
//                return NotFound();
//            }
//            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", order.ProductId);
//            return View(order);
//        }

//        // POST: Orders/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("OrderId,TotalAmount,OrderDate,CustomerId,ProductId")] Order order)
//        {
//            if (id != order.OrderId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(order);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!OrderExists(order.OrderId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", order.ProductId);
//            return View(order);
//        }

//        // GET: Orders/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var order = await _context.Order
//                .Include(o => o.Product)
//                .FirstOrDefaultAsync(m => m.OrderId == id);
//            if (order == null)
//            {
//                return NotFound();
//            }

//            return View(order);
//        }

//        // POST: Orders/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var order = await _context.Order.FindAsync(id);
//            if (order != null)
//            {
//                _context.Order.Remove(order);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool OrderExists(int id)
//        {
//            return _context.Order.Any(e => e.OrderId == id);
//        }
//    }
//}
