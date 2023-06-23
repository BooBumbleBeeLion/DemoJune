using System;
using System.Collections.Generic;

namespace DemoExAgain.Entities
{
    public partial class PickupPoint
    {
        public int Id { get; set; }

        public string Address { get; set; } = null!;

        public string Index { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public override string ToString()
        {
            return $"{Address}, {Index}";
        }
    }
}

