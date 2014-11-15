using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UserDetail> UserDetails { get; set; }
    }

    public class UserDetail
    {
        public int Id { get; set; }
        public int TeamId { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}