using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalYear.Models;

namespace FinalYear.Controllers
{
    public class LedgersController : Controller
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();

        // GET: Ledgers
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAllMembers()
        {
            var ledgers = db.Ledgers.Include(l => l.Company).Include(l => l.Customer);
            return View(ledgers.ToList());
        }
        [HttpPost]
        public JsonResult ViewLedger(int value, int type)
        {
            db.Configuration.ProxyCreationEnabled = false;


            if (type == 1)
            {
                var ledger = db.Ledgers.Where(x => x.CusID == value).ToList();
                return Json(ledger, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ledger = db.Ledgers.Where(x => x.CompID == value).ToList();
                return Json(ledger, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult ViewPage()
        {
            var viewModel = new CompanyCustomerViewModel
            {
                Companies = db.Companies.ToList(),
                Customers = db.Customers.ToList()
            };
            return PartialView("ViewPage",viewModel);
        }
        [HttpGet]
        public ActionResult LedgerTabList(string tab)
        {
            LedgersController ledgersController = new LedgersController();
            switch (tab)
            {
                case "#tab1":
                    var catlist = ledgersController.Create();
                    return catlist;

                default:
                    var plist = ledgersController.ViewPage();
                    return plist;
            }



        }
        // GET: Ledgers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ledger ledger = db.Ledgers.Find(id);
            if (ledger == null)
            {
                return HttpNotFound();
            }
            return View(ledger);
        }

        // GET: Ledgers/Create
        public PartialViewResult Create()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var viewModel = new CompanyCustomerViewModel
            {
                Companies = db.Companies.ToList(),
                Customers = db.Customers.ToList()
            };
            return PartialView("Create", viewModel);
        }
        [HttpGet]
        public ActionResult GetBalance(int id, string type)
        {
            db.Configuration.ProxyCreationEnabled = false;

            decimal balance = 0;
            if (type == "company")
            {
                // Get the latest balance of the company with the given id
                balance = db.Ledgers
        .Where(l => l.CompID == id)
        .OrderByDescending(l => l.Date)
        .Select(l => l.Balance)
        .FirstOrDefault() ?? 0;
            }
            else if (type == "customer")
            {
                // Get the latest balance of the customer with the given id
                balance = db.Ledgers
                    .Where(l => l.CusID == id)
        .OrderByDescending(l => l.Date)
        .Select(l => l.Balance)
        .FirstOrDefault() ?? 0;
            }
            return Json(new { balance }, JsonRequestBehavior.AllowGet);
        }
        // POST: Ledgers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,CusID,Description,Debit,Credit,Balance,CompID")] Ledger ledger,string selectedType,int selectedvalue, int Amount,string value)
        {
            if (ModelState.IsValid)
            {
                decimal? NewBalance = 0;
                if (selectedType=="customer")
                {
                    var balance = db.Ledgers.Where(x => x.CusID == selectedvalue).GroupBy(x => x.CusID).Select(g => new
                    {
                        CusID = g.Key,
                        balance = g.Sum(x => x.Debit - x.Credit)
                    }).FirstOrDefault()?.balance ?? 0.0m;
                    if (balance == 0 && value == "Credit")
                    {
                        ledger.Balance = balance - Amount;
                    }


                    ledger.CusID = selectedvalue;
                    ledger.Date = DateTime.Now;
                    if (value=="Credit")
                    {
                        ledger.Balance = balance - Amount;

                        ledger.Credit = Amount;
                        ledger.Debit = 0;

                    }
                    else
                    {
                        ledger.Balance = balance + Amount;

                        ledger.Credit = 0;
                        ledger.Debit = Amount;
                    }

                }
                else if(selectedType == "company")
                {
                    var balance = db.Ledgers.Where(x => x.CompID == selectedvalue).GroupBy(x => x.CompID).Select(g => new
                    {
                        CompID = g.Key,
                        balance = g.Sum(x => x.Debit - x.Credit)
                    }).FirstOrDefault()?.balance ?? 0.0m;
                    if (balance == 0 && value== "Credit")
                    {
                        ledger.Balance = balance - Amount;
                    }
                    ledger.CompID = selectedvalue;
                    ledger.Date = DateTime.Now;
                    if (value == "Credit")
                    {
                        ledger.Balance = balance - Amount;

                        ledger.Credit = Amount;
                        ledger.Debit = 0;
                    }
                    else
                    {
                        ledger.Balance = balance + Amount;

                        ledger.Credit = 0;
                        ledger.Debit = Amount;
                    }
                }
                else
                {
                        
                }
                NewBalance = ledger.Balance;
                db.Ledgers.Add(ledger);
                db.SaveChanges();
                
                return Json(new { success = true, message = "Update Sucessfully.", NewBalance= NewBalance });
            }
            else
            {
                return Json(new { success = false, message = "Update UNSucessfully." });
            }
            
        }

        // GET: Ledgers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ledger ledger = db.Ledgers.Find(id);
            if (ledger == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompID = new SelectList(db.Companies, "CompanyID", "CompanyName", ledger.CompID);
            ViewBag.CusID = new SelectList(db.Customers, "CusID", "Name", ledger.CusID);
            return View(ledger);
        }

        // POST: Ledgers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CusID,Description,Debit,Credit,Balance,CompID")] Ledger ledger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ledger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompID = new SelectList(db.Companies, "CompanyID", "CompanyName", ledger.CompID);
            ViewBag.CusID = new SelectList(db.Customers, "CusID", "Name", ledger.CusID);
            return View(ledger);
        }

        // GET: Ledgers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ledger ledger = db.Ledgers.Find(id);
            if (ledger == null)
            {
                return HttpNotFound();
            }
            return View(ledger);
        }

        // POST: Ledgers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ledger ledger = db.Ledgers.Find(id);
            db.Ledgers.Remove(ledger);
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
