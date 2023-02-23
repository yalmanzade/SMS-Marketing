using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_VilleMarketing.Data;
using E_VilleMarketing.Models;

namespace E_VilleMarketing.Controllers
{
    public class CustomersController : Controller
    {
        private readonly DatabaseContext _context;

        public string passedLayout = "null";
        public CustomersController(DatabaseContext context)
        {
            _context = context;
        }
        public void CheckSession()
        {
            if (HttpContext.Session.GetInt32("clientID").HasValue)
            {
                passedLayout = "_ClientLayout";
            }
            else if (HttpContext.Session.GetInt32("userID").HasValue)
            {
                passedLayout = "_UserLayout";
            }
        }
        // GET: Customers
        public async Task<IActionResult> Index()
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            return View(await _context.Customers.Where(c => c.BusinessID == (int)HttpContext.Session.GetInt32("businessID")).ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,Customer_FName,Customer_LName,PhoneNum,BusinessID")] Customer customer)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            int businessID = (int)HttpContext.Session.GetInt32("businessID");
            customer.BusinessID = businessID;
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,Customer_FName,Customer_LName,PhoneNum,BusinessID")] Customer customer)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            if (_context.Customers == null)
            {
                return Problem("Entity set 'DatabaseContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return _context.Customers.Any(e => e.CustomerID == id);
        }
    }
}
