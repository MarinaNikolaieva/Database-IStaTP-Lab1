using System;
using System.Collections.Generic;

namespace Lab1
{
    public partial class MealOrder
    {
        public int Id { get; set; }
        public int MealId { get; set; }
        public int OrderId { get; set; }
        public int MealCount { get; set; }

        public virtual Meal Meal { get; set; }
        public virtual Orders Order { get; set; }
    }
}
