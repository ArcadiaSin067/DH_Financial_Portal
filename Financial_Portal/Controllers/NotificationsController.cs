using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Financial_Portal.Helpers;
using Financial_Portal.Models;
using Microsoft.AspNet.Identity;

namespace Financial_Portal.Controllers
{
    [Authorize]
    [RequireHttps]
    public class NotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ShowMyStuff showStuff = new ShowMyStuff();

        // GET: Notifications
        public ActionResult Index()
        {
            if (NotificationsHelper.GetUnreadNotifications().Count() > 0)
            {
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                var notifications = showStuff.MyStuffOnly(controllerName);
                return View(notifications);
            }
            TempData["NoNotifications"] = "You have no unread Notifications.";
            return RedirectToAction("Index", "Home");
        }

        // GET: Notifications/IndexHead
        [Authorize(Roles = "Admin, Head_Of_House")]
        public ActionResult IndexHead()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var notifications = db.Notifications.Where(n => n.HouseholdId == user.HouseholdId).ToList();
            if (notifications.Count() > 0)
            {
                return View(notifications);
            }
            TempData["NoNotifications"] = "Your household has no Notifications yet.";
            return RedirectToAction("Index", "Home");
        }

        // GET: Notifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notifications = db.Notifications.Find(id);
            if (notifications == null)
            {
                return HttpNotFound();
            }
            return View(notifications);
        }

        // GET: Notifications/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IsRead,Message,Title,Created,RecipientId,HouseholdId")] Notification notifications)
        {
            if (ModelState.IsValid)
            {
                db.Notifications.Add(notifications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", notifications.HouseholdId);
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", notifications.RecipientId);
            return View(notifications);
        }

        // GET: Notifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notifications = db.Notifications.Find(id);
            if (notifications == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", notifications.HouseholdId);
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", notifications.RecipientId);
            return View(notifications);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IsRead,Message,Title,Created,RecipientId,HouseholdId")] Notification notifications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notifications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", notifications.HouseholdId);
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", notifications.RecipientId);
            return View(notifications);
        }

        // GET: Notifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notifications = db.Notifications.Find(id);
            if (notifications == null)
            {
                return HttpNotFound();
            }
            return View(notifications);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notification notifications = db.Notifications.Find(id);
            db.Notifications.Remove(notifications);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
