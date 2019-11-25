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
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household households = db.Households.Find(id);
            if (households == null)
            {
                return HttpNotFound();
            }
            return View(households);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Household households)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
                if (userRole != null)
                {
                    roleHelper.RemoveUserFromRole(userId, userRole);
                }
                if (string.IsNullOrEmpty(userRole))
                {
                    roleHelper.AddUserToRole(userId, "Head_Of_House");
                }
                households.Created = DateTime.Now;
                db.Households.Add(households);
                db.Users.Find(userId).HouseholdId = households.Id;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(households);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household households = db.Households.Find(id);
            if (households == null)
            {
                return HttpNotFound();
            }
            return View(households);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Created")] Household households)
        {
            if (ModelState.IsValid)
            {
                db.Entry(households).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(households);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household households = db.Households.Find(id);
            if (households == null)
            {
                return HttpNotFound();
            }
            return View(households);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household households = db.Households.Find(id);
            db.Households.Remove(households);
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
