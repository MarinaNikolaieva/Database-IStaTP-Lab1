using System;
using System.Collections.Generic;

namespace Lab1
{
    public partial class Orders
    {
        public Orders()
        {
            MealOrder = new HashSet<MealOrder>();
        }

        public int Id { get; set; }
        public int? EventId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<MealOrder> MealOrder { get; set; }
    }
}
