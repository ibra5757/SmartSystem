using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using FinalYear.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FinalYear.Controllers
{
    public class SalesOrderMastersController : Controller
    {
        _6digitRand _6DigitRand = new _6digitRand();
        long dig = 0;
        private SmartInventoryEntities db = new SmartInventoryEntities();

        // GET: SalesOrderMasters
        public ActionResult Index()
        {
            dig = _6DigitRand.GenerateRnd();
            Session["saleno"] = "S" + dig.ToString();
            return View();
        }
        public PartialViewResult ViewSales()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var sales = db.SalesOrderMasters
                 .Include(p => p.Customer).Include(p => p.User)
                 .ToList().OrderByDescending(z=>z.Date);
            
            return PartialView("ViewSales", sales);
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
                TotalQuantity = g.Sum(p => p.Quantity) - (db.SODetails.Where(s => s.PDID == g.Key).Sum(s => s.S_Quantity) != null ? (int)db.SODetails.Where(s => s.PDID == g.Key).Sum(s => s.S_Quantity) : 0),
                Price = (int)(stock.UnitPrice),
                Packing = (int)(stock.Packing)
            }).ToList();

            if (query.Count != 0)
            {
                return Json(query, "The requested quantity available.", JsonRequestBehavior.AllowGet);

            }
            else
            {
                // Create a new list with default values
                var emptyQuery = new List<QuantityAva>
    {
        new QuantityAva
        {
            PDID = stock.PDId, // Or any appropriate default value for PDID
            TotalQuantity = 0,
            Price = (int)stock.UnitPrice,
            Packing = (int)stock.Packing
        }
    };

                return Json(emptyQuery, "The requested quantity is not available.", JsonRequestBehavior.AllowGet);
            }



        }

        public PartialViewResult SalesList()
        {
            ViewBag.ProCmb = db.Products;
            ViewBag.Pd_type = db.ProDetails;
            ViewBag.AllProduct = db.Products;

            ViewBag.PDID = new SelectList(db.ProDetails, "PDId", "ProductUnit");
            ViewBag.SOID = new SelectList(db.SalesOrderMasters, "SOID", "BillNo");
            ViewBag.salesOrderMaste = db.SalesOrderMasters.Include(s => s.Customer).Include(s => s.User);
            return PartialView("Create");
        }
        public ActionResult TabList(string tab)
        {
            SalesOrderMastersController salesOrderMastersController = new SalesOrderMastersController();
            switch (tab)
            {
                case "#tab2":
                    var catlist = salesOrderMastersController.ViewSales();
                    return catlist;

                default:
                    var plists = salesOrderMastersController.SalesList();
                    return plists;
            }



        }
        // GET: SalesOrderMasters/Details/5
        public JsonResult Details(int? id)
        {


            var saleordermaster = (from pom in db.SalesOrderMasters
                                  join u in db.Users on pom.UserID equals u.UserID
                                  join c in db.Customers on pom.CusID equals c.CusID
                                  join pod in db.SODetails on pom.SOID equals pod.SOID
                                  join pd in db.ProDetails on pod.PDID equals pd.PDId
                                  join p in db.Products on pd.ProId equals p.ProID
                                  where pom.SOID == id
                                  select new
                                  {
                                      SOID = pom.SOID,
                                      Date = pom.Date,
                                      UserID = u.UserName,
                                      Name = c.Name,
                                      BillNo = pom.BillNo,
                                      TotalAmount=pom.TotalAmount,
                                      Discount=pom.Discount,
                                      GrandTotal=pom.GrandTotal,
                                      Prodetail = new
                                      {
                                          ProName = p.ProName,
                                          SODetail_Id = pod.SODetail_Id,
                                          ProductType = pd.ProductType,
                                          ProductUnit = pd.ProductUnit,
                                          Packing = pd.Packing,
                                          UnitPrice = pod.U_price,
                                          Quantity = pod.S_Quantity,
                                      }
                                  }).ToList();

            var result = saleordermaster.Select(po => new {
                SOID = po.SOID,
                Date = po.Date,
                UserID = po.UserID,
                Name = po.Name,
                BillNo = po.BillNo,
                TotalAmount = po.TotalAmount,
                Discount = po.Discount,
                GrandTotal = po.GrandTotal,
                Prodetail = new
                {
                    ProName = po.Prodetail.ProName,
                    PODetail_Id = po.Prodetail.SODetail_Id,
                    ProductType = po.Prodetail.ProductType,
                    ProductUnit = po.Prodetail.ProductUnit,
                    Packing = po.Prodetail.Packing,
                    UnitPrice = po.Prodetail.UnitPrice,
                    Quantity = po.Prodetail.Quantity,

                }
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        // GET: SalesOrderMasters/Create
        public ActionResult Create()
        {
            ViewBag.PDID = new SelectList(db.ProDetails, "PDId", "ProductUnit");
            ViewBag.SOID = new SelectList(db.SalesOrderMasters, "SOID", "BillNo");
            ViewBag.CusID = new SelectList(db.Customers, "CusID", "Name");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name");
            return PartialView();
        }
        public class CreateSalesOrderViewModel
        {
            public SalesOrderMaster SalesOrder { get; set; }
            public List<SODetail> Items { get; set; }
        }

        // POST: SalesOrderMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Mvc.HttpPost]
        public ActionResult Create(CreateSalesOrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var salesOrder = model.SalesOrder;
                    var items = model.Items;

                    decimal DAmount = salesOrder.GrandTotal;
                    var balance = db.Ledgers.Where(x => x.CusID == salesOrder.CusID).GroupBy(x => x.CusID).Select(g => new
                    {
                        CusID = g.Key,
                        balance = g.Sum(x => x.Debit - x.Credit)
                    }).FirstOrDefault()?.balance ?? 0.0m;
                    Ledger ledger = new Ledger
                    {
                        CusID = salesOrder.CusID,
                        CompID = null,
                        Description = salesOrder.BillNo,

                        Debit = DAmount,
                        Credit = 0,
                        Balance = balance + DAmount,
                        Date = DateTime.Now

                    };
                    LedgersController ledgersController = new LedgersController();
                    ledgersController.Create(ledger,"",0,0,"");


                    SalesOrderMaster salesOrderMaster = new SalesOrderMaster();
                    salesOrderMaster.BillNo = salesOrder.BillNo;
                    salesOrderMaster.CusID = salesOrder.CusID;
                    salesOrderMaster.Date = DateTime.Now;
                    salesOrderMaster.TotalAmount = salesOrder.TotalAmount;
                    salesOrderMaster.Discount = salesOrder.Discount;
                    salesOrderMaster.GrandTotal = salesOrder.GrandTotal;
                    var sessio = Session["UserID"];
                    salesOrderMaster.UserID = Convert.ToInt16(sessio);
                    salesOrderMaster.SODetails = new List<SODetail>();
                    foreach (var item in items)
                    {
                        SODetail detail = new SODetail();
                        detail.PDID = item.PDID;
                        detail.S_Quantity = item.S_Quantity;
                        detail.U_price = item.U_price;
                        detail.Discount = item.Discount;
                        detail.TotalPrice = item.TotalPrice;
                        salesOrderMaster.SODetails.Add(detail);

                    }

                    db.SalesOrderMasters.Add(salesOrderMaster);
                    db.SaveChanges();
                    dig = _6DigitRand.GenerateRnd();
                    Session["saleno"] = "S" + dig.ToString();
                    return Json(new { success = true, message = "Sales Sucessfully.", dig = "S" + dig.ToString() });
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
            return Json(new { success = false, message = "Sale UnSucessfully.", dig = "S" + dig.ToString() });

        }

        // GET: SalesOrderMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrderMaster salesOrderMaster = db.SalesOrderMasters.Find(id);
            if (salesOrderMaster == null)
            {
                return HttpNotFound();
            }
            ViewBag.CusID = new SelectList(db.Customers, "CusID", "Name", salesOrderMaster.CusID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", salesOrderMaster.UserID);
            return View(salesOrderMaster);
        }

        // POST: SalesOrderMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Date,SOID,BillNo,CusID,TotalAmount,Discount,GrandTotal,UserID")] SalesOrderMaster salesOrderMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesOrderMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CusID = new SelectList(db.Customers, "CusID", "Name", salesOrderMaster.CusID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", salesOrderMaster.UserID);
            return View(salesOrderMaster);
        }

        // GET: SalesOrderMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrderMaster salesOrderMaster = db.SalesOrderMasters.Find(id);
            if (salesOrderMaster == null)
            {
                return HttpNotFound();
            }
            return View(salesOrderMaster);
        }

        // POST: SalesOrderMasters/Delete/5
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesOrderMaster salesOrderMaster = db.SalesOrderMasters.Find(id);
            db.SalesOrderMasters.Remove(salesOrderMaster);
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
