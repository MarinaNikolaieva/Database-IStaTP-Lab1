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
    public class MealCategsController : ControllerBase
    {
        private readonly pizzeriaDatabaseContext _context;
        public MealCategsController(pizzeriaDatabaseContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var categories = _context.Category.Include(c => c.Meal).ToList();
            List<object> mealCateg = new List<object>();
            mealCateg.Add(new[] { "Category", "Meal number" });
            foreach(var c in categories)
            {
                mealCateg.Add(new object[] { c.CategoryName, c.Meal.Count() });
            }
            return new JsonResult(mealCateg);
        }
    }
}