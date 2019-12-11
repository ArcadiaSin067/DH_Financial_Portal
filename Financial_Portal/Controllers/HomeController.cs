using Financial_Portal.Helpers;
using Financial_Portal.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Financial_Portal.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private NotificationsHelper notifyHelp = new NotificationsHelper();
        private RoleHelper roleHelp = new RoleHelper();

        public ActionResult Index()
        {
            if (User.IsInRole("Head_Of_House"))
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                var invitations = user.Household.Invitations.Where(i => i.HouseholdId == user.HouseholdId);
                foreach (var invite in invitations)
                {
                    notifyHelp.DidTheyJoinUp(invite);
                }
            }
            return View();
        }

        // GET: Home/Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}