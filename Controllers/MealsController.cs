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
    public class MealsController : Controller
    {
        private readonly pizzeriaDatabaseContext _context;

        public MealsController(pizzeriaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Meals
        public async Task<IActionResult> Index(int? id)
        {
            var pizzeriaDatabaseContext = _context.Meal.Include(m => m.Category);
            if (id != null)
            {
                ViewBag.CategoryId = id;
                var smallDatabaseContext = _context.Meal.Where(m => m.CategoryId == id).Include(m => m.Category);
                return View(await smallDatabaseContext.ToListAsync());
            }
            else
            return View(await pizzeriaDatabaseContext.ToListAsync());
        }

        // GET: Meals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }

            return RedirectToAction("Pizzerias", "Meals", new { mealId = id });
        }

        // GET: Meals/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View();
        }

        // POST: Meals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,MealName,MealInfo,MealPrice")] Meal meal)
        {
            bool duplicate = await _context.Meal.AnyAsync(m => m.MealName == meal.MealName);
            if (duplicate)
            {
                ModelState.AddModelError("MealName", "This meal already exists");
            }
            if (ModelState.IsValid)
            {
                _context.Add(meal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", meal.CategoryId);
            return View(meal);
        }

        // GET: Meals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal.FindAsync(id);
            if (meal == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", meal.CategoryId);
            return View(meal);
        }

        // POST: Meals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,MealName,MealInfo,MealPrice")] Meal meal)
        {
            bool duplicate = await _context.Meal.AnyAsync(m => m.MealName == meal.MealName && m.Id != meal.Id);
            if (duplicate)
            {
                ModelState.AddModelError("MealName", "This meal already exists");
            }
            if (id != meal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealExists(meal.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", meal.CategoryId);
            return View(meal);
        }

        // GET: Meals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meal
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // POST: Meals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meal = await _context.Meal.FindAsync(id);

            var mealRuns = _context.PizzeriaMeal.Where(p => p.MealId == id).Include(p => p.Pizzeria).ToList();
            foreach(var obj in mealRuns)
            {
                var pizzeriaMeal = await _context.PizzeriaMeal.FindAsync(obj.Id);
                _context.PizzeriaMeal.Remove(pizzeriaMeal);
            }

            _context.Meal.Remove(meal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Pizzerias(int? mealId)
        {
            if (mealId == null)
                return NotFound();
            ViewBag.MealId = mealId;
            return RedirectToAction("Index", "PizzeriaMeals", new { mealId = mealId, pizzeriaId = 0 });
        }

        private bool MealExists(int id)
        {
            return _context.Meal.Any(e => e.Id == id);
        }
    }
}
