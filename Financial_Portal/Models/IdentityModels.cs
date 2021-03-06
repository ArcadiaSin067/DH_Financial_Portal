﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using Financial_Portal.Helpers;
using System.Linq;

namespace Financial_Portal.Models
{
    public class ApplicationUser : IdentityUser
    {
        private RoleHelper rhelp = new RoleHelper();

        public string FirstName { get; set; }
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
        [NotMapped]
        public string UserRole
        {
            get
            {
                return $"{rhelp.ListUserRoles(Id).FirstOrDefault()}";
            }
        }

        //foreign key
        public int? HouseholdId { get; set; }

        //Nav section
        public virtual Household Household { get; set; }
        public virtual ICollection<BankAccount> Accounts { get; set; }
        public virtual ICollection<Bucket> Buckets { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        //constructor
        public ApplicationUser()
        {
            Accounts = new HashSet<BankAccount>();
            Buckets = new HashSet<Bucket>();
            Notifications = new HashSet<Notification>();
            Transactions = new HashSet<Transaction>();
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

        public DbSet<BankAccount> Accounts { get; set; }
        public DbSet<BucketItem> BucketItems { get; set; }
        public DbSet<Bucket> Buckets { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ResetBalances> ResetBalances { get; set; }
    }
}