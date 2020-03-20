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
    public class PizzeriaMealsController : Controller
    {
        private readonly pizzeriaDatabaseContext _context;

        public PizzeriaMealsController(pizzeriaDatabaseContext context)
        {
            _context = context;
        }

        // GET: PizzeriaMeals
        public async Task<IActionResult> Index(int? pizzeriaId, int? mealId)
        {
            var pizzeriaDatabaseContext = _context.PizzeriaMeal.Include(p => p.Pizzeria).Include(p => p.Meal);
            ViewBag.PizzeriaId = pizzeriaId;
            ViewBag.MealId = mealId;
            if (pizzeriaId == null || pizzeriaId == 0)
            {
                var smallContext = _context.PizzeriaMeal.Where(p => p.MealId == mealId).Include(p => p.Pizzeria).Include(p => p.Meal);
                return View(await smallContext.ToListAsync());
            }
            if(mealId == null || mealId == 0)
            {
                var smallContext = _context.PizzeriaMeal.Where(p => p.PizzeriaId == pizzeriaId).Include(p => p.Pizzeria).Include(p => p.Meal);
                return View(await smallContext.ToListAsync());
            }
            return View(await pizzeriaDatabaseContext.ToListAsync());
        }

        // GET: PizzeriaMeals/Details/5
        public async Task<IActionResult> Details(int? id, int? pizzeriaId)
        {
            ViewBag.PizzeriaId = pizzeriaId;
            if (id == null)
            {
                return NotFound();
            }
            var mealId = _context.PizzeriaMeal.Find(id).MealId;
            var pizzeriaMeal = await _context.PizzeriaMeal
                .Include(p => p.Meal)
                .Include(p => p.Pizzeria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzeriaMeal == null)
            {
                return NotFound();
            }
            ViewBag.Count = _context.PizzeriaMeal.Where(p => p.MealId == mealId).Include(p => p.Pizzeria).Count();
            ViewBag.PizzeriaList = _context.PizzeriaMeal.Where(p => p.MealId == mealId).Include(p => p.Pizzeria).ToList();
            return View(pizzeriaMeal);
        }

        // GET: PizzeriaMeals/Create
        public IActionResult Create()
        {
            ViewData["MealId"] = new SelectList(_context.Meal, "Id", "MealName");
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "PizzeriaName");
            return View();
        }

        // POST: PizzeriaMeals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int mealId, int pizzeriaId, [Bind("Id,PizzeriaId,MealId")] PizzeriaMeal pizzeriaMeal)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(pizzeriaMeal);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["MealId"] = new SelectList(_context.Meal, "Id", "MealName", pizzeriaMeal.MealId);
            //ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "Address", pizzeriaMeal.PizzeriaId);
            bool check = await _context.PizzeriaMeal.AnyAsync(p => p.MealId == mealId && p.PizzeriaId == pizzeriaId);
            if (check)
            {
                ViewBag.ErrorMessage = "Error! This transfer already exists!";
                ModelState.AddModelError("MealId", "Error!");
            }
            if (ModelState.IsValid)
            {
                _context.Add(pizzeriaMeal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "PizzeriaMeals", new { id = pizzeriaId });
            }
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "PizzeriaName", pizzeriaMeal.PizzeriaId);
            ViewData["MealId"] = new SelectList(_context.Meal, "Id", "MealName", pizzeriaMeal.MealId);
            return View(pizzeriaMeal);
        }

        // GET: PizzeriaMeals/Edit/5
        public async Task<IActionResult> Edit(int? id, int? pizzeriaId)
        {
            //ViewBag.ErrorMessage = "";  DON'T NEED EDITING AT ALL!!!
            if (id == null)
            {
                return NotFound();
            }

            var pizzeriaMeal = await _context.PizzeriaMeal.FindAsync(id);
            if (pizzeriaMeal == null)
            {
                return NotFound();
            }
            ViewBag.PizzeriaId = pizzeriaId;
            ViewData["MealId"] = new SelectList(_context.Meal, "Id", "MealName", pizzeriaMeal.MealId);
            ViewBag.MealId = pizzeriaMeal.MealId;
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria.Where(p => p.Id == pizzeriaId), "Id", "PizzeriaName", pizzeriaMeal.PizzeriaId);
            return View(pizzeriaMeal);
        }

        // POST: PizzeriaMeals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int pizzeriaId, [Bind("Id,PizzeriaId,MealId")] PizzeriaMeal pizzeriaMeal)
        {
            if (id != pizzeriaMeal.Id)
            {
                return NotFound();
            }
            ViewBag.PizzeriaId = pizzeriaId;
            ViewBag.MealId = pizzeriaMeal.MealId;
            bool check = await _context.PizzeriaMeal.AnyAsync(p => p.MealId == pizzeriaMeal.MealId && p.PizzeriaId == pizzeriaId);
            if (check)
            {
                //ViewBag.ErrorMessage = "Error! This transfer already exists!";
                ModelState.AddModelError("MealId", "Error!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pizzeriaMeal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzeriaMealExists(pizzeriaMeal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "PizzeriaMeals", new { id = pizzeriaId });
            }
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "PizzeriaName", pizzeriaMeal.PizzeriaId);
            ViewData["MealId"] = new SelectList(_context.Meal, "Id", "MealName", pizzeriaMeal.MealId);
            return View(pizzeriaMeal);
        }

        // GET: PizzeriaMeals/Delete/5
        public async Task<IActionResult> Delete(int? id, int? pizzeriaId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzeriaMeal = await _context.PizzeriaMeal
                .Include(p => p.Meal)
                .Include(p => p.Pizzeria)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.PizzeriaId = pizzeriaId;
            if (pizzeriaMeal == null)
            {
                return NotFound();
            }

            return View(pizzeriaMeal);
        }

        // POST: PizzeriaMeals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int pizzeriaId)
        {
            ViewBag.PizzeriaId = pizzeriaId;
            var pizzeriaMeal = await _context.PizzeriaMeal.FindAsync(id);
            _context.PizzeriaMeal.Remove(pizzeriaMeal);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "PizzeriaMeals", new { id = pizzeriaId });
        }

        private bool PizzeriaMealExists(int id)
        {
            return _context.PizzeriaMeal.Any(e => e.Id == id);
        }
    }
}
