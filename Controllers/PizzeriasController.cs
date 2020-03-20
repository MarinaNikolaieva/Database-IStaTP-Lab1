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
    public class PizzeriasController : Controller
    {
        private readonly pizzeriaDatabaseContext _context;

        public PizzeriasController(pizzeriaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Pizzerias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pizzeria.ToListAsync());
        }

        // GET: Pizzerias/Details/5
        public async Task<IActionResult> Details(int? id, string? name)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.PizzeriaId = id;
            ViewBag.PizzeriaName = name;
            var pizzeria = await _context.Pizzeria
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzeria == null)
            {
                return NotFound();
            }

            return View(pizzeria);
            //return RedirectToAction("Index", "Animatronics", new { id = pizzeria.Id });
        }

        // GET: Pizzerias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pizzerias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PizzeriaName,Address,PizzeriaInfo")] Pizzeria pizzeria)
        {
            bool duplicate = await _context.Pizzeria.AnyAsync(p => p.PizzeriaName == pizzeria.PizzeriaName);
            if (duplicate)
            {
                ModelState.AddModelError("PizzeriaName", "This pizzeria already exists");
            }
            string temp = pizzeria.Address.ToString().ToLower();
            var dub = _context.Pizzeria.Where(p => p.Id != pizzeria.Id).Include(p => p.Animatronic).ToList();
            foreach (var obj in dub)
            {
                string temp1 = obj.Address.ToString().ToLower();
                if (temp1 == temp)
                {
                    ModelState.AddModelError("Address", "The pizzeria with this address already exists");
                    break;
                }
            }
            bool letCheck = pizzeria.PizzeriaName.Any(x => char.IsLetter(x));
            if (!letCheck)
            {
                ModelState.AddModelError("PizzeriaName", "Pizzeria name must have at least 1 letter in it!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(pizzeria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pizzeria);
        }

        // GET: Pizzerias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzeria = await _context.Pizzeria.FindAsync(id);
            if (pizzeria == null)
            {
                return NotFound();
            }
            return View(pizzeria);
        }

        // POST: Pizzerias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PizzeriaName,Address,PizzeriaInfo")] Pizzeria pizzeria)
        {
            bool duplicate = await _context.Pizzeria.AnyAsync(p => p.PizzeriaName.ToLower() == pizzeria.PizzeriaName.ToLower() && p.Id != pizzeria.Id);
            if (duplicate)
            {
                ModelState.AddModelError("PizzeriaName", "This pizzeria already exists");
            }
            string temp = pizzeria.Address.ToString().ToLower();
            var dub = _context.Pizzeria.Where(p => p.Id != pizzeria.Id).Include(p => p.Animatronic).ToList();
            foreach(var obj in dub)
            {
                string temp1 = obj.Address.ToString().ToLower();
                if (temp1 == temp)
                {
                    ModelState.AddModelError("Address", "The pizzeria with this address already exists");
                    break;
                }
            }
            bool letCheck = pizzeria.PizzeriaName.Any(x => char.IsLetter(x));
            if (!letCheck)
            {
                ModelState.AddModelError("PizzeriaName", "Pizzeria name must have at least 1 letter in it!");
            }

            if (id != pizzeria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pizzeria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzeriaExists(pizzeria.Id))
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
            return View(pizzeria);
        }

        // GET: Pizzerias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzeria = await _context.Pizzeria
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzeria == null)
            {
                return NotFound();
            }

            return View(pizzeria);
        }

        // POST: Pizzerias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pizzeria = await _context.Pizzeria.FindAsync(id);

            //delete everything connected with this pizzeria
            var pizzeriameals = _context.PizzeriaMeal.Where(p => p.PizzeriaId == id).Include(p => p.Meal).ToList();
            foreach(var obj in pizzeriameals)
            {
                var pizzeriaMeal = await _context.PizzeriaMeal.FindAsync(obj.Id);
                _context.PizzeriaMeal.Remove(pizzeriaMeal);
            }
            var pizzeriaevents = _context.PizzeriaEvent.Where(p => p.PizzeriaId == id).Include(p => p.Event).ToList();
            foreach(var obj in pizzeriaevents)
            {
                var pizzeriaEvent = await _context.PizzeriaEvent.FindAsync(obj.Id);
                _context.PizzeriaEvent.Remove(pizzeriaEvent);
            }
            var pizzeriareviews = _context.Review.Where(p => p.PizzeriaId == id).Include(p => p.User).ToList();
            foreach(var obj in pizzeriareviews)
            {
                var pizzeriaReview = await _context.Review.FindAsync(obj.Id);
                _context.Review.Remove(pizzeriaReview);
            }
            var pizzeriaanimatronics = _context.Animatronic.Where(a => a.PizzeriaId == id).Include(a => a.Pizzeria).ToList();
            foreach(var obj in pizzeriaanimatronics)
            {
                var pizzeriaAnimatronic = await _context.Animatronic.FindAsync(obj.Id);
                pizzeriaAnimatronic.PizzeriaId = null;
            }

            //then delete pizzeria and save changes
            _context.Pizzeria.Remove(pizzeria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Meals(int? pizzeriaId)
        {
            if (pizzeriaId == null)
                return NotFound();
            ViewBag.PizzeriaId = pizzeriaId;
            return RedirectToAction("Index", "PizzeriaMeals", new { mealId = 0, pizzeriaId = pizzeriaId });
        }

        public async Task<IActionResult> Events(int? pizzeriaId)
        {
            if (pizzeriaId == null)
                return NotFound();
            ViewBag.PizzeriaId = pizzeriaId;
            return RedirectToAction("Index", "PizzeriaEvents", new { eventId = 0, pizzeriaId = pizzeriaId });
        }

        private bool PizzeriaExists(int id)
        {
            return _context.Pizzeria.Any(e => e.Id == id);
        }
    }
}
