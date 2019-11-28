using Financial_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Financial_Portal.Extensions
{
    public static class InvitationExtension
    {
        public static async Task EmailInvitation(this Invitation invitation)
        {
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var callbackUrl = url.Action("Register", "Account", new { recipientEmail = invitation.RecipientEmail, code = invitation.Code, houseId = invitation.HouseholdId, Id = invitation.Id },
                protocol: HttpContext.Current.Request.Url.Scheme);
            var callbackUrl2 = url.Action("Join", "Households", new { recipientEmail = invitation.RecipientEmail, code = invitation.Code },
                protocol: HttpContext.Current.Request.Url.Scheme);
            var from = $"Big_Bank Application<{WebConfigurationManager.AppSettings["emailfrom"]}>";
            var emailMessage = new MailMessage(from, invitation.RecipientEmail)
            {
                Subject = $"You've been Invited to join a Household within the Big_Bank application.",
                Body = $"If you are not a registered user with Big_Bank click <a href=\"{callbackUrl}\">here</a> to begin registration.<br /><br />" +
                        $"Registered users can click <a href=\"{callbackUrl2}\">here</a> to join the household.",
                IsBodyHtml = true
            };
            var svc = new PersonalEmail();
            await svc.SendAsync(emailMessage);
        }



    }
}