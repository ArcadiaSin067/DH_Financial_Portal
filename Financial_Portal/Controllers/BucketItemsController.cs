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
    public class BucketItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ShowMyStuff showStuff = new ShowMyStuff();

        // GET: BucketItems
        public ActionResult Index()
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(showStuff.MyStuffOnly(controllerName));


            //var bucketItems = db.BucketItems.Include(b => b.Bucket);
            //return View(bucketItems.ToList());
        }

        // GET: BucketItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BucketItem bucketItems = db.BucketItems.Find(id);
            if (bucketItems == null)
            {
                return HttpNotFound();
            }
            return View(bucketItems);
        }

        // GET: BucketItems/Create
        public ActionResult Create()
        {
            ViewBag.BucketId = new SelectList(db.Buckets, "Id", "Name");
            return View();
        }

        // POST: BucketItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Created,TargetAmount,CurrentAmount,BucketId")] BucketItem bucketItems)
        {
            if (ModelState.IsValid)
            {
                db.BucketItems.Add(bucketItems);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BucketId = new SelectList(db.Buckets, "Id", "Name", bucketItems.BucketId);
            return View(bucketItems);
        }

        // GET: BucketItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BucketItem bucketItems = db.BucketItems.Find(id);
            if (bucketItems == null)
            {
                return HttpNotFound();
            }
            ViewBag.BucketId = new SelectList(db.Buckets, "Id", "Name", bucketItems.BucketId);
            return View(bucketItems);
        }

        // POST: BucketItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Created,TargetAmount,CurrentAmount,BucketId")] BucketItem bucketItems)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bucketItems).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BucketId = new SelectList(db.Buckets, "Id", "Name", bucketItems.BucketId);
            return View(bucketItems);
        }

        // GET: BucketItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BucketItem bucketItems = db.BucketItems.Find(id);
            if (bucketItems == null)
            {
                return HttpNotFound();
            }
            return View(bucketItems);
        }

        // POST: BucketItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BucketItem bucketItems = db.BucketItems.Find(id);
            db.BucketItems.Remove(bucketItems);
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
