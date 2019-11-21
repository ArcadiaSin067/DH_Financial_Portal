using Financial_Portal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Models
{
    public class Transactions
    {
        public int Id { get; set; }
        public float Amount { get; set; }
        public DateTime Created { get; set; }
        public string Memo { get; set; }
        public TransactionType TransactionType { get; set; }

        public int AccountId { get; set; }
        public int? BucketItemId { get; set; }
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }
        public virtual BucketItems BucketItem { get; set; }
        public virtual Accounts Account { get; set; }
    }
}