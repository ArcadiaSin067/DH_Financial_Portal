using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Financial_Portal.Models
{
    public class BucketItem
    {
        public int Id { get; set; }

        [Display(Name = "Bucket Item")]
        public string Name { get; set; }
        public DateTime Created { get; set; }

        [Display(Name = "Target Amount")]
        public double TargetAmount { get; set; }

        [Display(Name = "Current Amount")]
        public double CurrentAmount { get; set; }

        //foreign keys
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