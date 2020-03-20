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
    public class MealOrdersController : Controller
    {
        private readonly pizzeriaDatabaseContext _context;

        public MealOrdersController(pizzeriaDatabaseContext context)
        {
            _context = context;
        }

        // GET: MealOrders
        public async Task<IActionResult> Index()
        {
            var pizzeriaDatabaseContext = _context.MealOrder.Include(m => m.Meal).Include(m => m.Order);
            return View(await pizzeriaDatabaseContext.ToListAsync());
        }

        // GET: MealOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealOrder = await _context.MealOrder
                .Include(m => m.Meal)
                .Include(m => m.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mealOrder == null)
            {
                return NotFound();
            }

            return View(mealOrder);
        }

        // GET: MealOrders/Create
        public IActionResult Create()
        {
            ViewData["MealId"] = new SelectList(_context.Meal, "Id", "MealName");
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
            return View();
        }

        // POST: MealOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MealId,OrderId,MealCount")] MealOrder mealOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mealOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MealId"] = new SelectList(_context.Meal, "Id", "MealName", mealOrder.MealId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", mealOrder.OrderId);
            return View(mealOrder);
        }

        // GET: MealOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealOrder = await _context.MealOrder.FindAsync(id);
            if (mealOrder == null)
            {
                return NotFound();
            }
            ViewData["MealId"] = new SelectList(_context.Meal, "Id", "MealName", mealOrder.MealId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", mealOrder.OrderId);
            return View(mealOrder);
        }

        // POST: MealOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MealId,OrderId,MealCount")] MealOrder mealOrder)
        {
            if (id != mealOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mealOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealOrderExists(mealOrder.Id))
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
            ViewData["MealId"] = new SelectList(_context.Meal, "Id", "MealName", mealOrder.MealId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", mealOrder.OrderId);
            return View(mealOrder);
        }

        // GET: MealOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealOrder = await _context.MealOrder
                .Include(m => m.Meal)
                .Include(m => m.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mealOrder == null)
            {
                return NotFound();
            }

            return View(mealOrder);
        }

        // POST: MealOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mealOrder = await _context.MealOrder.FindAsync(id);
            _context.MealOrder.Remove(mealOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MealOrderExists(int id)
        {
            return _context.MealOrder.Any(e => e.Id == id);
        }
    }
}
