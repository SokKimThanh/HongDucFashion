using System;
using System.Collections.Generic;

namespace HongDucFashion.Models
{
    public partial class Promotion
    {
        public Promotion()
        {
            Products = new HashSet<Product>();
        }

        public int PromotionId { get; set; }
        public string? PromotionName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? DiscountPercent { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
