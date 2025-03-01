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
    public class SharesController : Controller
    {
        private readonly BlogcanhannContext _context;

        public SharesController(BlogcanhannContext context)
        {
            _context = context;
        }

        // GET: Shares
        public async Task<IActionResult> Index()
        {
            var blogcanhannContext = _context.Shares.Include(s => s.IdPostNavigation).Include(s => s.IdUserNavigation);
            return View(await blogcanhannContext.ToListAsync());
        }

        // GET: Shares/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var share = await _context.Shares
                .Include(s => s.IdPostNavigation)
                .Include(s => s.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (share == null)
            {
                return NotFound();
            }

            return View(share);
        }

        // GET: Shares/Create
        public IActionResult Create()
        {
            ViewData["IdPost"] = new SelectList(_context.Posts, "IdPost", "Title");
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "Email");
            return View();
        }

        // POST: Shares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SharedAt,IdUser,IdPost")] Share share)
        {
            if (ModelState.IsValid)
            {
                _context.Add(share);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPost"] = new SelectList(_context.Posts, "IdPost", "Title", share.IdPost);
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "Email", share.IdUser);
            return View(share);
        }

        // GET: Shares/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var share = await _context.Shares.FindAsync(id);
            if (share == null)
            {
                return NotFound();
            }
            ViewData["IdPost"] = new SelectList(_context.Posts, "IdPost", "Title", share.IdPost);
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "Email", share.IdUser);
            return View(share);
        }

        // POST: Shares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SharedAt,IdUser,IdPost")] Share share)
        {
            if (id != share.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(share);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShareExists(share.IdUser))
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
            ViewData["IdPost"] = new SelectList(_context.Posts, "IdPost", "Title", share.IdPost);
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "Email", share.IdUser);
            return View(share);
        }

        // GET: Shares/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var share = await _context.Shares
                .Include(s => s.IdPostNavigation)
                .Include(s => s.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (share == null)
            {
                return NotFound();
            }

            return View(share);
        }

        // POST: Shares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var share = await _context.Shares.FindAsync(id);
            if (share != null)
            {
                _context.Shares.Remove(share);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShareExists(long id)
        {
            return _context.Shares.Any(e => e.IdUser == id);
        }
    }
}
