using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Financial_Portal.Models;
using System.Web.Configuration;

namespace Financial_Portal.Helpers
{
    public class EmailHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        private static string ConfiguredEmail = WebConfigurationManager.AppSettings["emailfrom"];
        public static async Task ComposeEmailAsync(EmailModel email)
        {
            try
            {
                var senderEmail = $"{email.FromEmail}<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, ConfiguredEmail)
                {
                    Subject = email.Subject,
                    Body = $"<strong>{email.FromName} has sent you the following message</strong> <hr /> {email.Body}",
                    IsBodyHtml = true
                };
                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }
        public static async Task ComposeEmailAsync(RegisterViewModel model, string callbackUrl)
        {
            try
            {
                var senderEmail = $"Biggy_Bank Admin<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, model.Email)
                {
                    Subject = "Confirm your account",
                    Body = $"Please confirm your account by clicking <a href=\"{callbackUrl}\">here</a>",
                    IsBodyHtml = true
                };
                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }
        public static async Task ComposeEmailAsync(ForgotPasswordViewModel model, string callbackUrl)
        {
            try
            {
                var senderEmail = $"Biggy_Bank Admin<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, model.Email)
                {
                    Subject = "Reset Password",
                    Body = $"Please reset your password by clicking <a href=\"{callbackUrl}\">here</a>",
                    IsBodyHtml = true
                };
                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}