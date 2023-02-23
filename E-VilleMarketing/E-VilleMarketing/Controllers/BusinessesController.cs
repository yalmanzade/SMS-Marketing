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
    public class BusinessesController : Controller
    {
        private readonly DatabaseContext _context;

        public BusinessesController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Select(int? id)
        {
            if (id == null || _context.Businesses == null)
            {
                return NotFound();
            }

            var business = await _context.Businesses.FindAsync(id);
            if (business == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetInt32("businessID", (int)id);
            Console.WriteLine(HttpContext.Session.GetInt32("businessID"));
            return RedirectToAction("Index", "Composition");
        }

        // GET: Businesses
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Businesses.Where(b => b.ClientID == HttpContext.Session.GetInt32("clientID")).ToListAsync();
            return View(databaseContext);
        }

        // GET: Businesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Businesses == null)
            {
                return NotFound();
            }

            var business = await _context.Businesses
                .Include(b => b.Client)
                .FirstOrDefaultAsync(m => m.BusinessID == id);
            if (business == null)
            {
                return NotFound();
            }

            return View(business);
        }

        // GET: Businesses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Businesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BusinessID,BusinessName,BuildingNum,AptNum,StreetName,ZipCode,State,ClientID")] Business business)
        {
            int clientID = (int)HttpContext.Session.GetInt32("clientID");
            business.ClientID = clientID;
            if (ModelState.IsValid)
            {
                _context.Add(business);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(business);
        }

        // GET: Businesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Businesses == null)
            {
                return NotFound();
            }

            var business = await _context.Businesses.FindAsync(id);
            if (business == null)
            {
                return NotFound();
            }
            return View(business);
        }

        // POST: Businesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BusinessID,BusinessName,BuildingNum,AptNum,StreetName,ZipCode,State,ClientID")] Business business)
        {
            if (id != business.BusinessID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(business);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessExists(business.BusinessID))
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
            return View(business);
        }

        // GET: Businesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Businesses == null)
            {
                return NotFound();
            }

            var business = await _context.Businesses
                .Include(b => b.Client)
                .FirstOrDefaultAsync(m => m.BusinessID == id);
            if (business == null)
            {
                return NotFound();
            }

            return View(business);
        }

        // POST: Businesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Businesses == null)
            {
                return Problem("Entity set 'DatabaseContext.Businesses'  is null.");
            }
            var business = await _context.Businesses.FindAsync(id);
            if (business != null)
            {
                _context.Businesses.Remove(business);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessExists(int id)
        {
          return _context.Businesses.Any(e => e.BusinessID == id);
        }
    }
}
