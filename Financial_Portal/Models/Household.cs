using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Financial_Portal.Models
{
    public class Household
    {
        public int Id { get; set; }

        [Display(Name = "Household Name")]
        public string Name { get; set; }
        public DateTime Created { get; set; }

        //Nav section
        public virtual ICollection<Bucket> Buckets { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        //Constructor
        public Household()
        {
            Buckets = new HashSet<Bucket>();
            Invitations = new HashSet<Invitation>();
            Members = new HashSet<ApplicationUser>();
            BankAccounts = new HashSet<BankAccount>();
            Notifications = new HashSet<Notification>();
        }
    }
}