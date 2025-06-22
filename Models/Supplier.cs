using System;
using System.Collections.Generic;

namespace HongDucFashion.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? ContactInfo { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
