using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Financial_Portal.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        [StringLength(35, MinimumLength = 2, ErrorMessage = "Must have minimum length of 2 characters and maximum length of 35.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(35, MinimumLength = 2, ErrorMessage = "Must have minimum length of 2 characters and maximum length of 35.")]
        public string LastName { get; set; }

        public string AvatarPath { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public int HouseholdId { get; set; }

        //Nav section
        public virtual Households Household { get; set; }
        public virtual ICollection<Accounts> Accounts { get; set; }
        public virtual ICollection<Buckets> Buckets { get; set; }
        public virtual ICollection<Notifications> Notifications { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }

        //constructor
        public ApplicationUser()
        {
            Accounts = new HashSet<Accounts>();
            Buckets = new HashSet<Buckets>();
            Notifications = new HashSet<Notifications>();
            Transactions = new HashSet<Transactions>();
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<BucketItems> BucketItems { get; set; }
        public DbSet<Buckets> Buckets { get; set; }
        public DbSet<Households> Households { get; set; }
        public DbSet<Invitations> Invitations { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
    }
}