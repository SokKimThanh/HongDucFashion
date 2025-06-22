using System;
using System.Collections.Generic;

namespace HongDucFashion.Models
{
    public partial class InventoryTransaction
    {
        public int TransactionId { get; set; }
        public int? ProductId { get; set; }
        public int? QuantityChange { get; set; }
        public string? TransactionType { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? Note { get; set; }

        public virtual Product? Product { get; set; }
    }
}
