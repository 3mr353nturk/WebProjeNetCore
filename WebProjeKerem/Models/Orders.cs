using System;
using System.Collections.Generic;

namespace WebProjeKerem.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderLines = new HashSet<OrderLines>();
        }

        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public double Total { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderState { get; set; }
        public string AdresBasligi { get; set; }
        public string UserName { get; set; }
        public string Adres { get; set; }
        public string Sehir { get; set; }
        public string Semt { get; set; }
        public string Mahalle { get; set; }
        public string PostaKodu { get; set; }

        public virtual ICollection<OrderLines> OrderLines { get; set; }
    }
}
