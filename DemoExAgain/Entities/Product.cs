using System;
using System.Collections.Generic;

namespace DemoExAgain.Entities
{
    public partial class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int ManufacturerId { get; set; }

        public decimal Cost { get; set; }

        public int Discount { get; set; }

        public string? Photo { get; set; }

        public int QuantityInStock { get; set; }

        public virtual Manufacturer Manufacturer { get; set; } = null!;

        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }

}

