using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Financial_Portal.Models
{
    public class Bucket
    {
        public int Id { get; set; }

        [Display(Name = "Bucket Name")]
        public string Name { get; set; }
        public DateTime Created { get; set; }

        [Display(Name = "Current Amount")]
        public double CurrentAmount { get; set; }
        
        //foreign keys
        public string OwnerId { get; set; }
        public int HouseholdId { get; set; }

        //Nav section
        public virtual ApplicationUser Owner { get; set; }
        public virtual Household Household { get; set; }
        public virtual ICollection<BucketItem> BucketItems { get; set; }
        //Constructor
        public Bucket()
        {
            BucketItems = new HashSet<BucketItem>();
        }
    }
}