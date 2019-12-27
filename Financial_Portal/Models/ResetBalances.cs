using System;
using System.Collections.Generic;

namespace Financial_Portal.Models
{
    public class ResetBalances
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastReset { get; set; }
        public DateTime NextReset { get; set; }
        public List<Bucket> BucketsCurrent { get; set; }
        public List<BucketItem> ItemsCurrent { get; set; }
    }
}