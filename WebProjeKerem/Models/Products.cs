using System;
using System.Collections.Generic;

namespace WebProjeKerem.Models
{
    public partial class Products
    {
        public Products()
        {
            OrderLines = new HashSet<OrderLines>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public bool IsHome { get; set; }
        public bool IsApproved { get; set; }
        public int CategoryId { get; set; }

        public virtual Categories Category { get; set; }
        public virtual ICollection<OrderLines> OrderLines { get; set; }
    }
}
