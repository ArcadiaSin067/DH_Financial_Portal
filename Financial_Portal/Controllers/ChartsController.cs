using Financial_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Financial_Portal.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public JsonResult GetStackedData(int houseId)
        {
            var myAccounts = db.Accounts.Where(b => b.HouseholdId == houseId).ToList();
            var dataBlock = new DataBlock
            {
                Labels = myAccounts.Select(a => a.Name).ToList(),
                BarLabel1 = "Starting Balance",
                BarLabel2 = "Current Balance",
                BarLabel3 = "Low Balance Level",
                BGColor1 = "#11F136",
                BGColor2 = "#EFEB76",
                BGColor3 = "#F13811",
                Data1 = myAccounts.Select(a => a.StartBal).ToList(),
                Data2 = myAccounts.Select(a => a.CurrentBal).ToList(),
                Data3 = myAccounts.Select(a => a.LowBalanceLevel).ToList()
            };
            return Json(dataBlock);
        }
    }
}