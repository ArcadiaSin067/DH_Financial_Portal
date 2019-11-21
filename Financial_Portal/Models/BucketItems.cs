using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Models
{
    public class BucketItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public float TargetAmount { get; set; }
        public float CurrentAmount { get; set; }

        public int BucketId { get; set; }

        //Nav section
        public virtual Buckets Bucket { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }

        //Constructor
        public BucketItems()
        {
            Transactions = new HashSet<Transactions>();
        }
    }
}