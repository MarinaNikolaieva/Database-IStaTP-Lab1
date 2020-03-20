using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1
{
    public partial class PizzeriaMeal
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Pizzeria is required for entering!")]
        [Display(Name = "Pizzeria")]
        public int PizzeriaId { get; set; }
        [Required(ErrorMessage = "Meal is required for entering!")]
        [Display(Name = "Meal")]
        public int MealId { get; set; }

        public virtual Meal Meal { get; set; }
        public virtual Pizzeria Pizzeria { get; set; }
    }
}
