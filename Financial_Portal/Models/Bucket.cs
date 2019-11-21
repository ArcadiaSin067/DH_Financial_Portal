using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Models
{
    public class Bucket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public float TargetAmount { get; set; }
        public float CurrentAmount { get; set; }
        
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