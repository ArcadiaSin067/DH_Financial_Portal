using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Models
{
    public class BucketItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public float TargetAmount { get; set; }
        public float CurrentAmount { get; set; }

        public int BucketId { get; set; }

        //Nav section
        public virtual Bucket Bucket { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        //Constructor
        public BucketItem()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}