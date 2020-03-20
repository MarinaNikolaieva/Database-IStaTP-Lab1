using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1
{
    public partial class Category
    {
        public Category()
        {
            Meal = new HashSet<Meal>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required for entering!")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Category name must consist from letters only!")]
        [Display(Name = "Category name")]
        public string CategoryName { get; set; }

        public virtual ICollection<Meal> Meal { get; set; }
    }
}
