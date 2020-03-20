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
    public class EventsController : Controller
    {
        private readonly pizzeriaDatabaseContext _context;

        public EventsController(pizzeriaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Event.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Event = await _context.Event
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Event == null)
            {
                return NotFound();
            }
            return RedirectToAction("Pizzerias", "Events", new { eventId = id });
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventName,EventInfo,EventPrice")] Event Event)
        {
            bool duplicate = await _context.Event.AnyAsync(e => e.EventName == Event.EventName);
            if (duplicate)
            {
                ModelState.AddModelError("EventName", "This Event already exists");
            }
            bool letCheck = Event.EventName.Any(x => char.IsLetter(x));
            if (!letCheck)
            {
                ModelState.AddModelError("EventName", "Event name must have at least 1 letter in it!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(Event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Event = await _context.Event.FindAsync(id);
            if (Event == null)
            {
                return NotFound();
            }
            return View(Event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventName,EventInfo,EventPrice")] Event Event)
        {
            bool duplicate = await _context.Event.AnyAsync(e => e.EventName == Event.EventName && e.Id != Event.Id);
            if (duplicate)
            {
                ModelState.AddModelError("EventName", "This Event already exists");
            }
            bool letCheck = Event.EventName.Any(x => char.IsLetter(x));
            if (!letCheck)
            {
                ModelState.AddModelError("EventName", "Event name must have at least 1 letter in it!");
            }

            if (id != Event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(Event.Id))
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
            return View(Event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Event = await _context.Event
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Event == null)
            {
                return NotFound();
            }

            return View(Event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Event = await _context.Event.FindAsync(id);

            var eventRuns = _context.PizzeriaEvent.Where(e => e.EventId == id).Include(e => e.Pizzeria).ToList();
            foreach(var obj in eventRuns)
            {
                var pizzeriaEvent = await _context.PizzeriaEvent.FindAsync(obj.Id);
                _context.PizzeriaEvent.Remove(pizzeriaEvent);
            }
            _context.Event.Remove(Event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Pizzerias(int? eventId)
        {
            if (eventId == null)
                return NotFound();
            ViewBag.EventId = eventId;
            return RedirectToAction("Index", "PizzeriaEvents", new { eventId = eventId, pizzeriaId = 0 });
        }
        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }
    }
}
