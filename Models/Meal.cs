using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1
{
    public partial class Meal
    {
        public Meal()
        {
            MealOrder = new HashSet<MealOrder>();
            PizzeriaMeal = new HashSet<PizzeriaMeal>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required for entering!")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Meal name is required for entering!")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Meal name must consist from letters only!")]
        [Display(Name = "Meal name")]
        public string MealName { get; set; }
        [Display(Name = "Info")]
        public string MealInfo { get; set; }
        [Required(ErrorMessage = "Price is required for entering!")]
        [Range(typeof(int), "0", "1000")]
        [Display(Name = "Price")]
        public int MealPrice { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<MealOrder> MealOrder { get; set; }
        public virtual ICollection<PizzeriaMeal> PizzeriaMeal { get; set; }
    }
}
