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

namespace Financial_Portal.Controllers
{
    [Authorize]
    [RequireHttps]
    public class BucketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ShowMyStuff showStuff = new ShowMyStuff();

        // GET: Buckets
        public ActionResult Index()
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(showStuff.MyStuffOnly(controllerName));


            //var buckets = db.Buckets.Include(b => b.Household).Include(b => b.Owner);
            //return View(buckets.ToList());
        }

        // GET: Buckets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bucket buckets = db.Buckets.Find(id);
            if (buckets == null)
            {
                return HttpNotFound();
            }
            return View(buckets);
        }

        // GET: Buckets/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Buckets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Created,CurrentAmount,OwnerId,HouseholdId")] Bucket buckets)
        {
            if (ModelState.IsValid)
            {
                db.Buckets.Add(buckets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", buckets.HouseholdId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", buckets.OwnerId);
            return View(buckets);
        }

        // GET: Buckets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bucket buckets = db.Buckets.Find(id);
            if (buckets == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", buckets.HouseholdId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", buckets.OwnerId);
            return View(buckets);
        }

        // POST: Buckets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Created,CurrentAmount,OwnerId,HouseholdId")] Bucket buckets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buckets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", buckets.HouseholdId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", buckets.OwnerId);
            return View(buckets);
        }

        // GET: Buckets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bucket buckets = db.Buckets.Find(id);
            if (buckets == null)
            {
                return HttpNotFound();
            }
            return View(buckets);
        }

        // POST: Buckets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bucket buckets = db.Buckets.Find(id);
            db.Buckets.Remove(buckets);
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
