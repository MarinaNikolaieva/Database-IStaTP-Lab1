using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1
{
    public partial class Species
    {
        public Species()
        {
            Animatronic = new HashSet<Animatronic>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Species name is required for entering!")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Species name must consist from letters only!")]
        [Display(Name = "Name")]
        public string SpeciesName { get; set; }

        public virtual ICollection<Animatronic> Animatronic { get; set; }
    }
}
