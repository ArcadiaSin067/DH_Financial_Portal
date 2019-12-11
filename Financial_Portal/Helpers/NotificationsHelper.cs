using Financial_Portal.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Helpers
{
    public class NotificationsHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void AccountBalanceNotifications(Transaction transaction)
        {
            var transactAccount = db.Accounts.Find(transaction.AccountId);
            var transactHouse = db.Households.Find(transactAccount.HouseholdId);
            var transactOwner = db.Users.Find(transaction.OwnerId);
            if (transactAccount.CurrentBal <= transactAccount.LowBalanceLevel && transactAccount.CurrentBal >= 0)
            {
                var notifyMe = new Notification
                {
                    Created = DateTime.Now,
                    HouseholdId = transactHouse.Id,
                    IsRead = false,
                    Message = $"Warning! Your balance for Account: {transactAccount.Name} has" +
                              $" ${transactAccount.CurrentBal} remaining.",
                    RecipientId = transactOwner.Id,
                    Title = "Low Balance Alert!"
                };
                db.Notifications.Add(notifyMe);
                db.SaveChanges();
            }
            else if (transactAccount.CurrentBal < 0.00)
            {
                var notifyMe = new Notification
                {
                    Created = DateTime.Now,
                    HouseholdId = transactHouse.Id,
                    IsRead = false,
                    Message = $" Sorry {transactOwner.FirstName}, your balance for Account: " +
                              $"{transactAccount.Name} has overdrafted! Immediate attention necessary!",
                    RecipientId = transactOwner.Id,
                    Title = "Account Balance Overdrafted!"
                };
                db.Notifications.Add(notifyMe);
                db.SaveChanges();
            }
        }

        public void DidTheyJoinUp(Invitation invite)
        {
            var recipient = db.Users.FirstOrDefault(u => u.Email == invite.RecipientEmail);
            var hasJoined = recipient != null ? recipient.Email : "Nope";
            var housePpl = invite.Household.Members.ToList();
            var HoH = housePpl.FirstOrDefault(u => u.UserRole.Contains("Head_Of_House"));
            //var HoH = db.Users.Find(headOnly);
            var expirationDate = invite.Created.AddDays(invite.TTL);
            if (DateTime.Now > expirationDate && hasJoined == "Nope" && invite.IsValid) //I want to check open invitations for head of house on login for this bit
            {
                var notifyHead = new Notification
                {
                    Created = DateTime.Now,
                    HouseholdId = invite.HouseholdId,
                    Id = invite.Id,
                    IsRead = false,
                    Message = $"Your Invitation to the individual at Email: {invite.RecipientEmail} has Expired.",
                    RecipientId = HoH.Id,
                    Title = "Expired Invitation"
                };
                db.Invitations.Find(invite.Id).IsValid = false;
                db.Notifications.Add(notifyHead);
                db.SaveChanges();
            }
            else if (DateTime.Now < expirationDate && hasJoined != "Nope" && invite.IsValid) //This bit is intended for the Households/Join POST & Account/Register POST
            {
                var notifyHead = new Notification
                {
                    Created = DateTime.Now,
                    HouseholdId = invite.HouseholdId,
                    Id = invite.Id,
                    IsRead = false,
                    Message = $"The individual at Email: {hasJoined} has joined your Household!",
                    RecipientId = HoH.Id,
                    Title = "Invitation Accepted"
                };
                db.Notifications.Add(notifyHead);
                db.SaveChanges();
            }
        }

        public static List<Notification> GetUnreadNotifications()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            var notifications = db.Notifications.Where(n => n.RecipientId == currentUserId && !n.IsRead).ToList();
            return notifications;
        }
    }
}