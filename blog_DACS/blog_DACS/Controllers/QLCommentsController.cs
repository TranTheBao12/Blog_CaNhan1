using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using blog_DACS.Models;
using Microsoft.Extensions.Hosting;

namespace blog_DACS.Controllers
{
    public class QLCommentsController : Controller
    {
        private readonly BlogcanhannContext _context;

        public QLCommentsController(BlogcanhannContext context)
        {
            _context = context;
        }

        // GET: QLComments
        public async Task<IActionResult> Index()
        {
            var blogcanhannContext = _context.Comments.Include(c => c.IdPostNavigation).Include(c => c.IdUserNavigation);
            return View(await blogcanhannContext.ToListAsync());
        }

        // GET: QLComments/Details/5
        public async Task<IActionResult> Details(long? id, long? idPost)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.IdPostNavigation)
                .Include(c => c.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdUser == id && m.IdPost ==idPost);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: QLComments/Create
        public IActionResult Create()
        {
            ViewData["IdPost"] = new SelectList(_context.Posts, "IdPost", "IdPost");
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser");
            return View();
        }

        // POST: QLComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContentComment,CreatedAt,ParentComment,IdPost,IdUser")] Comment comment)
        {
            
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["IdPost"] = new SelectList(_context.Posts, "IdPost", "IdPost", comment.IdPost);
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser", comment.IdUser);
            return View(comment);
        }

        // GET: QLComments/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["IdPost"] = new SelectList(_context.Posts, "IdPost", "Title", comment.IdPost);
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "Email", comment.IdUser);
            return View(comment);
        }

        // POST: QLComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long idPost, long idUser, [Bind("IdPost,IdUser,ContentComment,CreatedAt,ParentComment")] Comment comment)
        {
            if (idPost != comment.IdPost || idUser != comment.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.IdPost, comment.IdUser))
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
            ViewData["IdPost"] = new SelectList(_context.Posts, "IdPost", "Title", comment.IdPost);
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "Email", comment.IdUser);
            return View(comment);
        }

        // GET: QLComments/Delete/5
        public async Task<IActionResult> Delete(long? idPost, long? idUser)
        {
            if (idPost == null || idUser == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.IdPostNavigation)
                .Include(c => c.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdPost == idPost && m.IdUser == idUser);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: QLComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComfi(long idPost, long idUser)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.IdPost == idPost && m.IdUser == idUser);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        private bool CommentExists(long idPost, long idUser)
        {
            return _context.Comments.Any(e => e.IdPost == idPost && e.IdUser == idUser);
        }
    }
}
