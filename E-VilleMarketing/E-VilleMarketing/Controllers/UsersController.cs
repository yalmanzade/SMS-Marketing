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
    public class UsersController : Controller
    {
        private readonly DatabaseContext _context;

        public string passedLayout = "null";
        public UsersController(DatabaseContext context)
        {
            _context = context;
        }
        public void CheckSession()
        {
            if (HttpContext.Session.GetInt32("clientID").HasValue)
            {
                passedLayout = "_ClientLayout";
            }
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            var databaseContext = _context.Users;
            return View(await databaseContext.Where(u => u.BusinessID == (int)HttpContext.Session.GetInt32("businessID")).ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,User_Email,User_Password,BusinessID")] User user)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            user.BusinessID = (int)HttpContext.Session.GetInt32("businessID");
            foreach (var login in _context.Logins)
            {
                if (login.Email == user.User_Email)
                    return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                Account Login = new Account();
                Login.Email = user.User_Email;
                Login.Password = user.User_Password;
                _context.Logins.Add(Login);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Error", "Home");
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,User_Email,User_Password,BusinessID")] User user)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Business)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            if (_context.Users == null)
            {
                return Problem("Entity set 'DatabaseContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return _context.Users.Any(e => e.UserID == id);
        }
    }
}
