using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1
{
    public partial class Users
    {
        public Users()
        {
            Orders = new HashSet<Orders>();
            Review = new HashSet<Review>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "User name is required for entering!")]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<Review> Review { get; set; }
    }
}
