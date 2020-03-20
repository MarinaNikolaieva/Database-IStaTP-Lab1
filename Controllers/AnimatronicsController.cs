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
    public class AnimatronicsController : Controller
    {
        private readonly pizzeriaDatabaseContext _context;

        public AnimatronicsController(pizzeriaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Animatronics
        public async Task<IActionResult> Index(int? id, string? name)
        {
            var pizzeriaDatabaseContext = _context.Animatronic.Include(a => a.Pizzeria).Include(a => a.Species);
            if (id != null)
            {
                ViewBag.PizzeriaId = id;
                ViewBag.PizzeriaName = name;
                var smallDatabaseContext = _context.Animatronic.Where(a => a.PizzeriaId == id).Include(a => a.Pizzeria).Include(a => a.Species);
                return View(await smallDatabaseContext.ToListAsync());
            }
            else
            {
                ViewBag.PizzeriaName = "all database";
                return View(await pizzeriaDatabaseContext.ToListAsync());
            }  
        }

        // GET: Animatronics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animatronic = await _context.Animatronic
                .Include(a => a.Pizzeria)
                .Include(a => a.Species)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animatronic == null)
            {
                return NotFound();
            }

            return View(animatronic);
        }

        // GET: Animatronics/Create
        public IActionResult Create()
        {
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "PizzeriaName");
            ViewData["SpeciesId"] = new SelectList(_context.Species, "Id", "SpeciesName");
            return View();
        }

        // POST: Animatronics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PizzeriaId,SpeciesId,AnimatronicName,AnimatronicInfo")] Animatronic animatronic)
        {
            bool duplicate = await _context.Animatronic.AnyAsync(a => a.AnimatronicName == animatronic.AnimatronicName);
            if (duplicate)
            {
                ModelState.AddModelError("AnimatronicName", "This animatronic already exists");
            }
            bool letCheck = animatronic.AnimatronicName.Any(x => char.IsLetter(x));
            if (!letCheck)
            {
                ModelState.AddModelError("AnimatronicName", "Animatronic name must have at least 1 letter in it!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(animatronic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "PizzeriaName", animatronic.PizzeriaId);
            ViewData["SpeciesId"] = new SelectList(_context.Species, "Id", "SpeciesName", animatronic.SpeciesId);
            return View(animatronic);
        }

        // GET: Animatronics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;
            var animatronic = await _context.Animatronic.FindAsync(id);
            if (animatronic == null)
            {
                return NotFound();
            }
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "PizzeriaName", animatronic.PizzeriaId);
            ViewData["SpeciesId"] = new SelectList(_context.Species, "Id", "SpeciesName", animatronic.SpeciesId);
            return View(animatronic);
        }

        // POST: Animatronics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PizzeriaId,SpeciesId,AnimatronicName,AnimatronicInfo")] Animatronic animatronic)
        {
            bool duplicate = await _context.Animatronic.AnyAsync(a => a.AnimatronicName == animatronic.AnimatronicName && a.Id != animatronic.Id);
            if (duplicate)
            {
                ModelState.AddModelError("AnimatronicName", "This animatronic already exists");
            }
            bool letCheck = animatronic.AnimatronicName.Any(x => char.IsLetter(x));
            if (!letCheck)
            {
                ModelState.AddModelError("AnimatronicName", "Animatronic name must have at least 1 letter in it!");
            }

            if (id != animatronic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animatronic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimatronicExists(animatronic.Id))
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
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "PizzeriaName", animatronic.PizzeriaId);
            ViewData["SpeciesId"] = new SelectList(_context.Species, "Id", "SpeciesName", animatronic.SpeciesId);
            return View(animatronic);
        }

        // GET: Animatronics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animatronic = await _context.Animatronic
                .Include(a => a.Pizzeria)
                .Include(a => a.Species)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animatronic == null)
            {
                return NotFound();
            }

            return View(animatronic);
        }

        // POST: Animatronics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animatronic = await _context.Animatronic.FindAsync(id);
            _context.Animatronic.Remove(animatronic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> setPizzeriaNull(int id)
        {
            var animatronic = await _context.Animatronic.FindAsync(id);
            animatronic.PizzeriaId = null;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimatronicExists(int id)
        {
            return _context.Animatronic.Any(e => e.Id == id);
        }
    }
}
