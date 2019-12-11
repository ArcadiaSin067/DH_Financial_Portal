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
        private static ApplicationDbContext db = new ApplicationDbContext();
        public void AccountBalanceNotifications(Transaction transaction)
        {
            if (transaction.Account.CurrentBal <= transaction.Account.LowBalanceLevel && transaction.Account.CurrentBal >= 0.00)
            {
                var notifyMe = new Notification
                {
                    Created = DateTime.Now,
                    HouseholdId = (int)transaction.Owner.HouseholdId,
                    IsRead = false,
                    Message = $"Warning! Your balance for Account: {transaction.Account.Name} has" +
                              $"{transaction.Account.CurrentBal} remaining.",
                    RecipientId = transaction.OwnerId,
                    Title = "Low Balance Alert!"
                };
                db.Notifications.Add(notifyMe);
                db.SaveChanges();
            }
            else if (transaction.Account.CurrentBal < 0.00)
            {
                var notifyMe = new Notification
                {
                    Created = DateTime.Now,
                    HouseholdId = (int)transaction.Owner.HouseholdId,
                    IsRead = false,
                    Message = $" Sorry {transaction.Owner.FirstName}, your balance for Account: " +
                              $"{transaction.Account.Name} has overdrafted! Immediate attention necessary!",
                    RecipientId = transaction.OwnerId,
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
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            var notifications = db.Notifications.Where(n => n.RecipientId == currentUserId && !n.IsRead).ToList();
            return notifications;
        }
    }
}