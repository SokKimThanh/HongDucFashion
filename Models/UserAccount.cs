using System;
using System.Collections.Generic;

namespace HongDucFashion.Models
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            Roles = new HashSet<RoleAccount>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }

        public virtual ICollection<RoleAccount> Roles { get; set; }
    }
}
