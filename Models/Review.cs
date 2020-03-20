using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1
{
    public partial class Review
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "User name is required for entering!")]
        [Display(Name = "User")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Pizzeria name is required for entering!")]
        [Display(Name = "Pizzeria")]
        public int PizzeriaId { get; set; }
        [Display(Name = "Date")]
        public DateTime ReviewDate { get; set; }
        [Required(ErrorMessage = "Review can not be empty!")]
        [Display(Name = "Text")]
        public string ReviewText { get; set; }

        public virtual Pizzeria Pizzeria { get; set; }
        public virtual Users User { get; set; }
    }
}
