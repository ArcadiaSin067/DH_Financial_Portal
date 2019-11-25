using System;
using System.ComponentModel.DataAnnotations;

namespace Financial_Portal.Models
{
    public class Invitation
    {
        public int Id { get; set; }

        [Display(Name = "Time To Live")]
        public int TTL { get; set; }
        public Guid Code { get; set; }
        public bool IsValid { get; set; }
        public DateTime Created { get; set; }

        [Display(Name = "Recipient Email")]
        public string RecipientEmail { get; set; }

        //foreign keys
        public int HouseholdId { get; set; }

        //Nav section
        public virtual Household Household { get; set; }
    }
}