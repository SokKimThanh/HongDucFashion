using System;
using System.Collections.Generic;

namespace HongDucFashion.Models
{
    public partial class Product
    {
        public Product()
        {
            InventoryTransactions = new HashSet<InventoryTransaction>();
            OrderDetails = new HashSet<OrderDetail>();
            Promotions = new HashSet<Promotion>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public int? AvailableQuantity { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<Promotion> Promotions { get; set; }
    }
}
