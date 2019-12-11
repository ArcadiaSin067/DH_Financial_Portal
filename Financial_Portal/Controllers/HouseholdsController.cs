using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using Financial_Portal.Models;
using Financial_Portal.Helpers;
using Microsoft.AspNet.Identity;
using Financial_Portal.Extensions;

namespace Financial_Portal.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private InvitationHelper inviteHelp = new InvitationHelper();
        private NotificationsHelper notifyHelp = new NotificationsHelper();
        private RoleHelper roleHelper = new RoleHelper();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Leave
        public async Task<ActionResult> Leave()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case "Head_Of_House":
                    var occupants = db.Users.Where(u => u.HouseholdId == user.HouseholdId).Count();
                    if (occupants > 1)
                    {
                        var myHouseId = user.HouseholdId;
                        var members = db.Users.Where(u => u.HouseholdId == myHouseId && u.Id != userId);
                        ViewBag.Successor = new SelectList(members, "Id", "FullName");
                        return View();
                    }
                    else
                    {
                        var myHouse = db.Households.Find(user.HouseholdId);
                        roleHelper.RemoveUserFromRole(userId, "Head_Of_House");
                        db.Households.Remove(myHouse);
                        user.HouseholdId = null;
                        db.SaveChanges();
                        await ControllerContext.HttpContext.RefreshAuthentication(user);
                        TempData["Success"] = "You have successfully left / blown up your household.";
                        return RedirectToAction("Index", "Home");
                    }
                case "Member":
                default:
                    roleHelper.RemoveUserFromRole(userId,"Member");
                    user.HouseholdId = null;
                    db.SaveChanges();
                    await ControllerContext.HttpContext.RefreshAuthentication(user);
                    TempData["Success"] = "You have successfully left / run away from your household.";
                    return RedirectToAction("Index", "Home");
            }

        }

        // POST: Households/Leave
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Leave(string successor)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var newHead = db.Users.Find(successor);
            user.HouseholdId = null;
            db.SaveChanges();
            roleHelper.RemoveUserFromRole(user.Id, "Head_Of_House");
            roleHelper.RemoveUserFromRole(newHead.Id, "Member");
            roleHelper.AddUserToRole(newHead.Id, "Head_Of_House");
            await ControllerContext.HttpContext.RefreshAuthentication(user);
            TempData["Appointed"] = $"You have successfully left your household, and have appointed {newHead.FullName} as new head of your household.";
            return RedirectToAction("Index", "Home");
        }

        // GET: Households/Join
        public ActionResult Join(string recipientEmail, string code)
        {
            var realGuid = Guid.Parse(code);
            var invitation = db.Invitations.FirstOrDefault(i => i.RecipientEmail == recipientEmail && i.Code == realGuid);
            if (invitation == null) { TempData["Warning"] = "No Invitation Found."; return RedirectToAction("Index", "Home"); }
            var expirationDate = invitation.Created.AddDays(invitation.TTL);
            if (invitation.IsValid && DateTime.Now < expirationDate)
            {
                var invitationVM = new AcceptInvitationViewModel
                {
                    Id = invitation.Id,
                    Email = recipientEmail,
                    Code = realGuid,
                    HouseholdId = invitation.HouseholdId
                };
                return View(invitationVM);
            }
            else
            {
                TempData["Warning"] = "This invitation expired or is no longer valid.";
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Households/Join
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Join(JoinInvitationViewModel invitation)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                var userRole = roleHelper.ListUserRoles(user.Id).FirstOrDefault();
                if (userRole != null)
                {
                    roleHelper.RemoveUserFromRole(user.Id, userRole);
                }
                roleHelper.AddUserToRole(user.Id, "Member");
                user.HouseholdId = invitation.HouseholdId;
                var invite = db.Invitations.Find(invitation.Id);
                notifyHelp.DidTheyJoinUp(invite);
                inviteHelp.MarkAsInvalid(invitation.Id);
                db.SaveChanges();
                await ControllerContext.HttpContext.RefreshAuthentication(user);
                var houseName = db.Households.Find(invitation.HouseholdId).Name;
                TempData["Appointed"] = $"You joined the '{houseName}' household!";
                return RedirectToAction("Index", "Home");
            }
            return View(invitation);
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
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Household households)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                foreach(var userRole in roleHelper.ListUserRoles(userId))
                {
                    roleHelper.RemoveUserFromRole(userId, userRole);
                }
                roleHelper.AddUserToRole(userId, "Head_Of_House");
                households.Created = DateTime.Now;
                db.Households.Add(households);
                db.Users.Find(userId).HouseholdId = households.Id;
                db.SaveChanges();
                await ControllerContext.HttpContext.RefreshAuthentication(db.Users.Find(userId));
                TempData["Appointed"] = $"'{households.Name}' created.";
                return RedirectToAction("Configure", "Households", new { id = households.Id});
            }
            return View(households);
        }

        // GET: Households/Configure/5
        public ActionResult Configure(int? Id)
        {
            if (Id != null && Id != 0 && db.Users.Find(User.Identity.GetUserId()).Household.IsConfigured == false)
            {
                ViewBag.HouseholdId = (int)Id;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Households/Configure/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Configure(ConfigureHouseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                var bankAccount = new BankAccount
                {
                    OwnerId = user.Id,
                    Name = model.BankName,
                    Created = DateTime.Now,
                    StartBal = model.StartBal,
                    CurrentBal = model.StartBal,
                    AccountType = model.AccountType,
                    HouseholdId = model.HouseholdId,
                    LowBalanceLevel = model.LowBalanceLevel
                };
                db.Accounts.Add(bankAccount);
                db.SaveChanges();
                var bucket = new Bucket
                {
                    CurrentAmount = 0,
                    OwnerId = user.Id,
                    Created = DateTime.Now,
                    Name = model.BucketName,
                    HouseholdId = model.HouseholdId
                };
                db.Buckets.Add(bucket);
                db.SaveChanges();
                var bucketItem = new BucketItem
                {
                    CurrentAmount = 0,
                    BucketId = bucket.Id,
                    Name = model.ItemName,
                    Created = DateTime.Now,
                    TargetAmount = model.ItemTargetAmount
                };
                db.BucketItems.Add(bucketItem);

                user.Household.IsConfigured = true;
                db.SaveChanges();
                TempData["Success"] = "Your Household is now configured.";
                return RedirectToAction("Index", "Home");
            }
            TempData["Errors"] = ErrorReader.ErrorCompiler(ModelState);
            return RedirectToAction("Configure", "Households");
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
