using System;
using System.Collections.Generic;

namespace HongDucFashion.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
            UserAccounts = new HashSet<UserAccount>();
        }

        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
