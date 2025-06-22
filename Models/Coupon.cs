using System;
using System.Collections.Generic;

namespace HongDucFashion.Models
{
    public partial class Coupon
    {
        public Coupon()
        {
            Orders = new HashSet<Order>();
        }

        public int CouponId { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public decimal? DiscountAmount { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
