using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Financial_Portal.Models;
using Financial_Portal.Helpers;
using Microsoft.AspNet.Identity;
using Financial_Portal.Extensions;

namespace Financial_Portal.Controllers
{
    [Authorize]
    [RequireHttps]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ShowMyStuff showStuff = new ShowMyStuff();
        private NotificationsHelper notifyMe = new NotificationsHelper();

        // GET: Transactions
        public ActionResult Index()
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(showStuff.MyStuffOnly(controllerName));


            //var transactions = db.Transactions.Include(t => t.Account).Include(t => t.BucketItem).Include(t => t.Owner);
            //return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transactions = db.Transactions.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            return View(transactions);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            var houseId = db.Users.Find(User.Identity.GetUserId()).HouseholdId ?? 0;
            ViewBag.AccountId = new SelectList(db.Accounts.Where(b => b.HouseholdId == houseId), "Id", "Name");
            ViewBag.BucketItemId = new SelectList(db.BucketItems.Where(b => b.Bucket.HouseholdId  == houseId), "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Amount,Memo,TransactionType,AccountId,BucketItemId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.OwnerId = User.Identity.GetUserId();
                transaction.Created = DateTime.Now;
                db.Transactions.Add(transaction);
                db.SaveChanges();
                transaction.UpdateBalances();
                notifyMe.AccountBalanceNotifications(transaction);
                return RedirectToAction("Index");
            }
            var houseId = db.Users.Find(User.Identity.GetUserId()).HouseholdId ?? 0;
            ViewBag.AccountId = new SelectList(db.Accounts.Where(b => b.HouseholdId == houseId), "Id", "Name", transaction.AccountId);
            ViewBag.BucketItemId = new SelectList(db.BucketItems.Where(b => b.Bucket.HouseholdId == houseId), "Id", "Name", transaction.BucketItemId);
            TempData["Errors"] = ErrorReader.ErrorCompiler(ModelState);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transactions = db.Transactions.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transactions.AccountId);
            ViewBag.BucketItemId = new SelectList(db.BucketItems, "Id", "Name", transactions.BucketItemId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", transactions.OwnerId);
            return View(transactions);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Amount,Created,Memo,TransactionType,AccountId,BucketItemId,OwnerId")] Transaction transactions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transactions.AccountId);
            ViewBag.BucketItemId = new SelectList(db.BucketItems, "Id", "Name", transactions.BucketItemId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", transactions.OwnerId);
            return View(transactions);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transactions = db.Transactions.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            return View(transactions);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transactions = db.Transactions.Find(id);
            db.Transactions.Remove(transactions);
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
