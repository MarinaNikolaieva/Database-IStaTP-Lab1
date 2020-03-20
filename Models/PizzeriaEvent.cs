using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1
{
    public partial class PizzeriaEvent
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Pizzeria is required for entering!")]
        [Display(Name = "Pizzeria")]
        public int PizzeriaId { get; set; }
        [Required(ErrorMessage = "Event is required for entering!")]
        [Display(Name = "Event")]
        public int EventId { get; set; }

        public virtual Event Event { get; set; }
        public virtual Pizzeria Pizzeria { get; set; }
    }
}
