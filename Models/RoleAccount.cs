using System;
using System.Collections.Generic;

namespace HongDucFashion.Models
{
    public partial class RoleAccount
    {
        public RoleAccount()
        {
            Users = new HashSet<UserAccount>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<UserAccount> Users { get; set; }
    }
}
