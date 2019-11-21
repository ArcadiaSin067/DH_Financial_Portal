using Financial_Portal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float StartBal { get; set; }
        public float CurrentBal { get; set; }
        public DateTime Created { get; set; }
        public AccountTypes AccountType { get; set; }

        public string OwnerId { get; set; }
        public int HouseholdId { get; set; }

        //Nav section
        public virtual ApplicationUser Owner { get; set; }
        public virtual Households Household { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
        //Constructor
        public Accounts()
        {
            Transactions = new HashSet<Transactions>();
        }
    }
}