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

        // GET: Home/Index
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (User.IsInRole("Head_Of_House") || User.IsInRole("Admin"))
            {
                var invitations = user.Household.Invitations.Where(i => i.HouseholdId == user.HouseholdId);
                foreach (var invite in invitations)
                {
                    notifyHelp.DidTheyJoinUp(invite);
                }
            }
            var myHouse = new Household();
            if (user.HouseholdId == null && user.HouseholdId != 0) { myHouse.Id = 0; }
            else { myHouse.Id = (int)user.HouseholdId; }
            return View(myHouse);
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