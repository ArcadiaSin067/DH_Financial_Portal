using System;
using System.Linq;
using Financial_Portal.Models;

namespace Financial_Portal.Helpers
{
    public class CurrentsReset
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void ResetCurrentBalances()
        {
            var reset = db.ResetBalances.FirstOrDefault();
            if (reset.NextReset.Month == DateTime.Today.Month && reset.LastReset.Month == DateTime.Today.Month - 1)
            {
                reset.BucketsCurrent = db.Buckets.Where(b => b.CurrentAmount != 0).ToList();
                reset.ItemsCurrent = db.BucketItems.Where(bi => bi.CurrentAmount != 0).ToList();
                foreach (var bucket in reset.BucketsCurrent)
                {
                    bucket.CurrentAmount = 0;
                }
                foreach (var item in reset.ItemsCurrent)
                {
                    item.CurrentAmount = 0;
                }
                reset.NextReset = DateTime.Today.AddMonths(1);
                reset.LastReset = DateTime.Now;
                db.Entry(reset).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
