using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1
{
    public partial class Pizzeria
    {
        public Pizzeria()
        {
            Animatronic = new HashSet<Animatronic>();
            PizzeriaEvent = new HashSet<PizzeriaEvent>();
            PizzeriaMeal = new HashSet<PizzeriaMeal>();
            Review = new HashSet<Review>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Pizzeria name is required for entering!")]
        [Display(Name = "Name")]
        public string PizzeriaName { get; set; }
        [Required(ErrorMessage = "Address is required for entering!")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "About pizzeria")]
        public string PizzeriaInfo { get; set; }

        public virtual ICollection<Animatronic> Animatronic { get; set; }
        public virtual ICollection<PizzeriaEvent> PizzeriaEvent { get; set; }
        public virtual ICollection<PizzeriaMeal> PizzeriaMeal { get; set; }
        public virtual ICollection<Review> Review { get; set; }
    }
}
