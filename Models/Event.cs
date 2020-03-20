using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1
{
    public partial class Event
    {
        public Event()
        {
            PizzeriaEvent = new HashSet<PizzeriaEvent>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Event name is required for entering!")]
        [Display(Name = "Event name")]
        public string EventName { get; set; }
        [Display(Name = "Info")]
        public string EventInfo { get; set; }
        [Required(ErrorMessage = "Price is required for entering!")]
        [Range (typeof(int), "0", "1000")]
        [Display(Name = "Price")]
        public int EventPrice { get; set; }

        public virtual ICollection<PizzeriaEvent> PizzeriaEvent { get; set; }
    }
}
