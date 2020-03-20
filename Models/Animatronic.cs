using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1
{
    public partial class Animatronic
    {
        public int Id { get; set; }
        [Display(Name = "Pizzeria")]
        public int? PizzeriaId { get; set; }
        [Display(Name = "Species")]
        public int SpeciesId { get; set; }
        [Required(ErrorMessage = "Animatronic name is required for entering!")]
        [Display(Name = "Name")]
        public string AnimatronicName { get; set; }
        [Display(Name = "About animatronic")]
        public string AnimatronicInfo { get; set; }
        
        public virtual Pizzeria Pizzeria { get; set; }
        public virtual Species Species { get; set; }
    }
}
