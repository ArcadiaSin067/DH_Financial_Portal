using System;
using Financial_Portal.Enums;
using System.ComponentModel.DataAnnotations;


namespace Financial_Portal.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Memo { get; set; }
        public double Amount { get; set; }
        public DateTime Created { get; set; }

        [Display(Name = "Transaction Type")]
        public TransactionType TransactionType { get; set; }

        //foreign keys
        public int AccountId { get; set; }
        public int? BucketItemId { get; set; }
        public string OwnerId { get; set; }

        //Nav Section
        public virtual ApplicationUser Owner { get; set; }
        public virtual BucketItem BucketItem { get; set; }
        public virtual BankAccount Account { get; set; }
    }
}