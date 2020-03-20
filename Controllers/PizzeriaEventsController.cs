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
    public class PizzeriaEventsController : Controller
    {
        private readonly pizzeriaDatabaseContext _context;

        public PizzeriaEventsController(pizzeriaDatabaseContext context)
        {
            _context = context;
        }

        // GET: PizzeriaEvents
        public async Task<IActionResult> Index(int? pizzeriaId, int? eventId)
        {
            var pizzeriaDatabaseContext = _context.PizzeriaEvent.Include(p => p.Event).Include(p => p.Pizzeria);
            ViewBag.PizzeriaId = pizzeriaId;
            ViewBag.EventId = eventId;
            if (pizzeriaId == null || pizzeriaId == 0)
            {
                var smallContext = _context.PizzeriaEvent.Where(p => p.EventId == eventId).Include(p => p.Event).Include(p => p.Pizzeria);
                return View(await smallContext.ToListAsync());
            }
            if (eventId == null || eventId == 0)
            {
                var smallContext = _context.PizzeriaEvent.Where(p => p.PizzeriaId == pizzeriaId).Include(p => p.Event).Include(p => p.Pizzeria);
                return View(await smallContext.ToListAsync());
            }
            return View(await pizzeriaDatabaseContext.ToListAsync());
        }

        // GET: PizzeriaEvents/Details/5
        public async Task<IActionResult> Details(int? id, int? pizzeriaId)
        {
            ViewBag.PizzeriaId = pizzeriaId;
            if (id == null)
            {
                return NotFound();
            }
            var eventId = _context.PizzeriaEvent.Find(id).EventId;
            var pizzeriaEvent = await _context.PizzeriaEvent
                .Include(p => p.Event)
                .Include(p => p.Pizzeria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzeriaEvent == null)
            {
                return NotFound();
            }
            ViewBag.Count = _context.PizzeriaEvent.Where(p => p.EventId == eventId).Include(p => p.Pizzeria).Count();
            ViewBag.PizzeriaList = _context.PizzeriaEvent.Where(p => p.EventId == eventId).Include(p => p.Pizzeria).ToList();
            return View(pizzeriaEvent);
        }

        // GET: PizzeriaEvents/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "EventName");
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "PizzeriaName");
            return View();
        }

        // POST: PizzeriaEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int eventId, int pizzeriaId, [Bind("Id,PizzeriaId,EventId")] PizzeriaEvent pizzeriaEvent)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(pizzeriaEvent);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["EventId"] = new SelectList(_context.Event, "Id", "EventName", pizzeriaEvent.EventId);
            //ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "Address", pizzeriaEvent.PizzeriaId);
            bool check = await _context.PizzeriaEvent.AnyAsync(p => p.EventId == eventId && p.PizzeriaId == pizzeriaId);
            if (check)
            {
                ViewBag.ErrorMessage = "Error! This connection already exists!";
                ModelState.AddModelError("EventId", "Error!");
            }
            if (ModelState.IsValid)
            {
                _context.Add(pizzeriaEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "PizzeriaEvents", new { id = pizzeriaId });
            }
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "EventName", pizzeriaEvent.EventId);
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "PizzeriaName", pizzeriaEvent.PizzeriaId);
            return View(pizzeriaEvent);
        }

        // GET: PizzeriaEvents/Edit/5
        public async Task<IActionResult> Edit(int? id, int? pizzeriaId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzeriaEvent = await _context.PizzeriaEvent.FindAsync(id);
            if (pizzeriaEvent == null)
            {
                return NotFound();
            }
            ViewBag.PizzeriaId = pizzeriaId;
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "EventName", pizzeriaEvent.EventId);
            ViewBag.EventId = pizzeriaEvent.EventId;
            ViewData["PizzeriaId"] = new SelectList(_context.Pizzeria, "Id", "Address", pizzeriaEvent.PizzeriaId);
            return View(pizzeriaEvent);
        }

        // POST: PizzeriaEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int pizzeriaId, [Bind("Id,PizzeriaId,EventId")] PizzeriaEvent pizzeriaEvent)
        {
            if (id != pizzeriaEvent.Id)
            {
                return NotFound();
            }
            ViewBag.PizzeriaId = pizzeriaId;
            ViewBag.EventId = pizzeriaEvent.EventId;
            try
            {
                _context.Update(pizzeriaEvent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PizzeriaEventExists(pizzeriaEvent.Id))
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

        // GET: PizzeriaEvents/Delete/5
        public async Task<IActionResult> Delete(int? id, int? pizzeriaId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzeriaEvent = await _context.PizzeriaEvent
                .Include(p => p.Event)
                .Include(p => p.Pizzeria)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.PizzeriaId = pizzeriaId;
            if (pizzeriaEvent == null)
            {
                return NotFound();
            }

            return View(pizzeriaEvent);
        }

        // POST: PizzeriaEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int pizzeriaId)
        {
            ViewBag.PizzeriaId = pizzeriaId;
            var pizzeriaEvent = await _context.PizzeriaEvent.FindAsync(id);
            _context.PizzeriaEvent.Remove(pizzeriaEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PizzeriaEventExists(int id)
        {
            return _context.PizzeriaEvent.Any(e => e.Id == id);
        }
    }
}
