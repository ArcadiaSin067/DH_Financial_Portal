using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Models
{
    public class Notifications
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }

        public string RecipientId { get; set; }
        public int HouseholdId { get; set; }

        //Nav section
        public virtual Households Household { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
    }
}