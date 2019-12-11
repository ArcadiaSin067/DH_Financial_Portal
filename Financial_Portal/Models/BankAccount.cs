using System;
using Financial_Portal.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Financial_Portal.Models
{
    public class BankAccount
    {
        public int Id { get; set; }

        [Display(Name = "Account Name")]
        public string Name { get; set; }

        [Display(Name = "Starting Balance")]
        public double StartBal { get; set; }

        [Display(Name = "Current Balance")]
        public double CurrentBal { get; set; }
        public DateTime Created { get; set; }

        [Display(Name = "Low Balance")]
        public double LowBalanceLevel { get; set; }

        [Display(Name = "Account Type")]
        public AccountTypes AccountType { get; set; }


        //foreign keys
        public string OwnerId { get; set; }
        public int HouseholdId { get; set; }

        //Nav section
        public virtual ApplicationUser Owner { get; set; }
        public virtual Household Household { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        //Constructor
        public BankAccount()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}