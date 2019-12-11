using System.Web;
using System.Linq;
using Financial_Portal.Models;
using Microsoft.AspNet.Identity;

namespace Financial_Portal.Helpers
{
    public class ShowMyStuff
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public object MyStuffOnly(string controllerName)//parameter wanted is the controllerName
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            switch (controllerName)
            {
                case "BankAccounts":
                    return db.Accounts.Where(a => a.HouseholdId == user.HouseholdId).ToList();
                case "Buckets":
                    return db.Buckets.Where(b => b.HouseholdId == user.HouseholdId).ToList();
                case "BucketItems":
                    return db.Households.Find(user.HouseholdId).Buckets.SelectMany(b => b.BucketItems).ToList();
                case "Notifications":
                    return NotificationsHelper.GetUnreadNotifications();
                default:
                    return null;
            }
            // make a dynamic list of my things for whichever controller the method calling this came from

        }
    }
}