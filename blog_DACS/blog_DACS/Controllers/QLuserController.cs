using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using blog_DACS.Models;

namespace blog_DACS.Controllers
{
    public class QLuserController : Controller
    {
        private readonly BlogcanhannContext _context;

        public QLuserController(BlogcanhannContext context)
        {
            _context = context;
        }

        // GET: QLuser
        public async Task<IActionResult> Index()
        {
            var blogcanhannContext = _context.Users.Include(u => u.IdRoleNavigation);
            return View(await blogcanhannContext.ToListAsync());
        }

        // GET: QLuser/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: QLuser/Create
        public IActionResult Create()
        {
            ViewData["IdRole"] = new SelectList(_context.RoleUsers, "IdRole", "IdRole");
            return View();
        }

        // POST: QLuser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUser,FullName,PhoneNumber,Email,DateOfBirth,Gender,PlaceOfBirth,CreatedAt,LastUpdatedAt,ExistenceStatus,Pass,IdRole")] User user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Pass);
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);

            if (existingUser == null)
            {
                user.CreatedAt = DateTime.Now;
                user.IdUser = _context.Users.Max(p => p.IdUser) + 1;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
               
            }
            else
            {
                // Code xử lý khi ModelState không hợp lệ

                // Trả về một giá trị hợp lệ
                return View("Login", "Default");
            }
            ViewData["IdRole"] = new SelectList(_context.RoleUsers, "IdRole", "IdRole", user.IdRole);
            return View(user);
        }

        // GET: QLuser/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["IdRole"] = new SelectList(_context.RoleUsers, "IdRole", "IdRole", user.IdRole);
            return View(user);
        }

        // POST: QLuser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("IdUser,FullName,PhoneNumber,Email,DateOfBirth,Gender,PlaceOfBirth,CreatedAt,LastUpdatedAt,ExistenceStatus,Pass,IdRole")] User user)
        {
            if (id != user.IdUser)
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
                    if (!UserExists(user.IdUser))
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
            ViewData["IdRole"] = new SelectList(_context.RoleUsers, "IdRole", "IdRole", user.IdRole);
            return View(user);
        }

        // GET: QLuser/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: QLuser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.IdUser == id);
        }
    }
}
