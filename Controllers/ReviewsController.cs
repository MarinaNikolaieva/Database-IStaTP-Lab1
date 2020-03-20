using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1;

namespace Lab1.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly pizzeriaDatabaseContext _context;

        public ReviewsController(pizzeriaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index(int? id, string? name, int? userId)
        {
            var pizzeriaDatabaseContext = _context.Review.Include(r => r.Pizzeria).Include(r => r.User);
            if (id != 0 && name != "0")
            {
                ViewBag.PizzeriaId = id;
                ViewBag.PizzeriaName = name;
                var smallContext = _context.Review.Where(r => r.PizzeriaId == id).Include(r => r.Pizzeria).Include(r => r.User);
                return View(await smallContext.ToListAsync());
            }
            if (userId != 0)
            {
                ViewBag.PizzeriaName = "one user";
                var smallContext = _context.Review.Where(r => r.UserId == userId).Include(r => r.Pizzeria).Include(r => r.User);
                return View(await smallContext.ToListAsync());
            }
            else
            {
                ViewBag.PizzeriaName = "all database";
                return View(await pizzeriaDatabaseContext.ToListAsync());
            }
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Pizzeria)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            ViewBag.UserId = review.UserId;
            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "PizzeriaName");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,PizzeriaId,ReviewDate,ReviewText")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.ReviewDate = DateTime.Now;
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "PizzeriaName", review.PizzeriaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", review.UserId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "Address", review.PizzeriaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", review.UserId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,PizzeriaId,ReviewDate,ReviewText")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "Address", review.PizzeriaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", review.UserId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Pizzeria)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Review.FindAsync(id);
            _context.Review.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.Id == id);
        }
    }
}
