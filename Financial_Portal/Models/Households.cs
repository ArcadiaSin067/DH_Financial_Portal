using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Models
{
    public class Households
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }

        //Nav section
        public virtual ICollection<Accounts> Accounts { get; set; }
        public virtual ICollection<Buckets> Bucket { get; set; }
        public virtual ICollection<Invitations> Invitations { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Notifications> Notifications { get; set; }

        //Constructor
        public Households()
        {
            Accounts = new HashSet<Accounts>();
            Bucket = new HashSet<Buckets>();
            Invitations = new HashSet<Invitations>();
            Members = new HashSet<ApplicationUser>();
            Notifications = new HashSet<Notifications>();
        }
    }
}