using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzeriaReviewsController : ControllerBase
    {
        private readonly pizzeriaDatabaseContext _context;
        public PizzeriaReviewsController (pizzeriaDatabaseContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var pizzerias = _context.Pizzeria.Include(p => p.Review).ToList();
            List<object> revPizzerias = new List<object>();
            revPizzerias.Add(new[] { "Pizzeria", "Number of reviews" });
            foreach(var p in pizzerias)
            {
                revPizzerias.Add(new object[] { p.PizzeriaName, p.Review.Count() });
            }
            return new JsonResult(revPizzerias);
        }
    }
}