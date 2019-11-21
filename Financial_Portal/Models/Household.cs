using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Models
{
    public class Household
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }

        //Nav section
        public virtual ICollection<BankAccount> Accounts { get; set; }
        public virtual ICollection<Bucket> Bucket { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        //Constructor
        public Household()
        {
            Accounts = new HashSet<BankAccount>();
            Bucket = new HashSet<Bucket>();
            Invitations = new HashSet<Invitation>();
            Members = new HashSet<ApplicationUser>();
            Notifications = new HashSet<Notification>();
        }
    }
}