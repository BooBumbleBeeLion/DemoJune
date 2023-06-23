using System;
using System.Collections.Generic;

namespace DemoExAgain.Entities
{

    public partial class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public int PickupPointId { get; set; }

        public string? ClientName { get; set; }

        public int CodeToGet { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        public virtual PickupPoint PickupPoint { get; set; } = null!;
    }
}