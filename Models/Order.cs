using System;
using System.Collections.Generic;

namespace HongDucFashion.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? CustomerId { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? CouponId { get; set; }

        public virtual Coupon? Coupon { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
