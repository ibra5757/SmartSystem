using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalYear.Models;

namespace FinalYear.Controllers
{
    public class purchasesController : Controller
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();

        // GET: purchases
        _6digitRand _6DigitRand = new _6digitRand();
        long dig = 0;
        public ActionResult Index()
        {
            dig=_6DigitRand.GenerateRnd();
            Session["Pno"] = "P"+dig.ToString();
            ViewBag.Unit = db.ProDetails.Select(x => x.ProductUnit).Distinct().ToList();
            ViewBag.Type = db.ProDetails.Select(x => x.ProductType).Distinct().ToList();
            ViewBag.AllProduct = db.Products.ToList();
            ViewBag.SupplierID = db.Companies.ToList();
            return View();
        }
        [System.Web.Mvc.HttpGet]
        public JsonResult CheckQuantity(int name, string unit, string type)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var pro_ = db.ProDetails.Where(x => x.ProId == name && x.ProductType == type && x.ProductUnit == unit).Select(x => x.PDId).ToList();
            var alpha = pro_[0];
            var stock = db.ProDetails.Where(x => x.PDId == alpha).FirstOrDefault();
            var query = db.PODetails
                              .Where(p => p.PDID == alpha)
                              .GroupBy(p => p.PDID)
            .Select(g => new QuantityAva
            {
                PDID = g.Key,
                TotalQuantity = g.Sum(p => p.Quantity) - db.SODetails.Where(s => s.PDID == g.Key).Sum(s => s.S_Quantity),
                Price = (int)stock.UnitPrice,
                Packing = (int)stock.Packing
            }).ToList();

            if (query != null)
            {
                return Json(query, "The requested quantity available.", JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(query, "The requested quantity is not available.", JsonRequestBehavior.AllowGet);

            }


        }

        public PartialViewResult _list()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var purchases = db.PurchaseOrderMasters
                 .Include(p => p.Company).Include(p => p.User) 
                 .ToList();

            return PartialView("_list", purchases.ToList());
        }
        public ActionResult PurchasesTabList(string tab)
        {
            db.Configuration.ProxyCreationEnabled = false;

            purchasesController purchasesController = new purchasesController();
            switch (tab)
            {
                case "#tab2":
                    var catlist = purchasesController._list();
                    return catlist;

                default:
                    var plist = purchasesController._create();
                    return plist;
            }



        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrderMaster purchase = db.PurchaseOrderMasters.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // GET: purchases/Create
        public PartialViewResult _create()
        {
            dig = _6DigitRand.GenerateRnd();
       //     Session["Pno"] = "P" + dig.ToString();
            ViewBag.Unit = db.ProDetails.Select(x => x.ProductUnit).Distinct().ToList();
            ViewBag.Type = db.ProDetails.Select(x => x.ProductType).Distinct().ToList();
            ViewBag.AllProduct = db.Products.ToList();
            ViewBag.SupplierID = db.Companies.ToList();
            db.Configuration.ProxyCreationEnabled = false;

            return PartialView("_create");
        }
        public JsonResult  cmb(int name) {
            db.Configuration.ProxyCreationEnabled = false;
            var SUBID = db.ProDetails.Where(x=>x.ProId ==name).Select(x=>new { x.ProductType ,x.ProId }).Distinct().ToList();
            return Json(SUBID,JsonRequestBehavior.AllowGet);
        }
        public JsonResult pro(int name,string type)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var pro_ = db.ProDetails.Where(x => x.ProId == name && x.ProductType==type).ToList();

            return Json(pro_, JsonRequestBehavior.AllowGet);

        }

        // POST: purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public class CreateSalesOrderViewModel
        {
            public PurchaseOrderMaster PurchaseOrder { get; set; }
            public List<PODetail> Items { get; set; }
        }
        [HttpPost]
        public ActionResult Create(CreateSalesOrderViewModel model)
        {
            var PurchaseOrder = model.PurchaseOrder;
            var items = model.Items;
            try
            {
                var UserID = Session["UserID"].ToString();


                if (ModelState.IsValid)

                {
                    decimal DAmount = 0;
                    foreach (var item in items)
                    {
                        var p = db.ProDetails.Where(x => x.PDId == item.PDID).Select(y => y.UnitPrice).First();
                        DAmount = Convert.ToDecimal(p) * Convert.ToDecimal(item.Quantity.ToString());
                    }
                    var balance = db.Ledgers.Where(x => x.CompID == PurchaseOrder.CompanyId)
                      .GroupBy(x => x.CompID)
                      .Select(g => new
                      {
                          CompID = g.Key,
                          balance = g.Sum(x => x.Debit - x.Credit)
                      })
                      .FirstOrDefault()?.balance ?? 0.0m;
                    Ledger ledger = new Ledger
                    {
                        CusID = null,
                        CompID = int.Parse(PurchaseOrder.CompanyId.ToString()),
                        Description = PurchaseOrder.BillNo,

                        Debit = DAmount,
                        Credit = 0,
                        Balance = balance + DAmount,
                        Date = DateTime.Now

                    };
                    LedgersController ledgersController = new LedgersController();
                    ledgersController.Create(ledger, "", 0, 0, "");
                    PurchaseOrderMaster purchaseOrderMaster = new PurchaseOrderMaster();
                    purchaseOrderMaster.UserID = int.Parse(UserID);
                    purchaseOrderMaster.BillNo = PurchaseOrder.BillNo;
                    purchaseOrderMaster.CompanyId = PurchaseOrder.CompanyId;
                    purchaseOrderMaster.Date = DateTime.Now;
                    purchaseOrderMaster.PODetails = new List<PODetail>();
                    foreach (var item in items)
                    {
                        PODetail pODetail = new PODetail
                        {
                            POID = purchaseOrderMaster.POID,
                            PDID = item.PDID,
                            Quantity = item.Quantity,
                            BatchNo = item.BatchNo.ToString()
                        };
                        purchaseOrderMaster.PODetails.Add(pODetail);
                    };

                    db.PurchaseOrderMasters.Add(purchaseOrderMaster);
                    db.SaveChanges();
                    dig = _6DigitRand.GenerateRnd();
                    Session["Pno"] = dig.ToString();
                    return Json(new { success = true, message = "Purcase Register Sucessfully.", dig = "P" + dig.ToString() });

                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string z = string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return Json(new { success = false, message = "Purchase UnSucessfully.", dig = dig.ToString() });
        }

        // GET: purchases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var POdetao = db.PODetails.Where(x => x.POID == id).FirstOrDefault();
            ViewBag.Unit = db.ProDetails.Select(x => x.ProductUnit).Distinct().ToList();
            ViewBag.Type = db.ProDetails.Select(x => x.ProductType).Distinct().ToList();
            ViewBag.AllProduct = db.Products.ToList();
            return PartialView(POdetao);
        }

        // POST: purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( PurchaseOrderMaster purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", purchase.UserID);
            ViewBag.SupplierID = new SelectList(db.Companies, "SupplierID", "Name", purchase.CompanyId);
            return View(purchase);
        }

        // GET: purchases/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrderMaster purchase = db.PurchaseOrderMasters.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PurchaseOrderMaster purchase = db.PurchaseOrderMasters.Find(id);
            db.PurchaseOrderMasters.Remove(purchase);
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
