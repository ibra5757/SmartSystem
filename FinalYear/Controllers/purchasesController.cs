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
        public ActionResult Index()
        {
            ViewBag.Unit = db.ProDetails.Select(x => x.ProductUnit).Distinct().ToList();
            ViewBag.Type = db.ProDetails.Select(x => x.ProductType).Distinct().ToList();
            ViewBag.AllProduct = db.Products.ToList();
            ViewBag.SupplierID = db.Companies.ToList();
            var purchases = db.PurchaseOrderMasters.ToList();
            return View(purchases.ToList());
        }

        public ActionResult _list()
        {

            return PartialView();
        }
        // GET: purchases/Details/5
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
        public ActionResult _create()
        {//actual wala view konsa h tera 

            return PartialView();
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
        [HttpPost]
        public ActionResult Create(PODetail pODetail,PurchaseOrderMaster purchaseOrderMaster,string ProductUnit,string ProName)
        {
            try
            {
                //using (var context = new MyDbContext())
                //{

                //}

                    var UserID = Session["UserID"].ToString();

                if (ModelState.IsValid)

                {
                    purchaseOrderMaster.UserID = int.Parse(UserID);
                    purchaseOrderMaster.CompanyId = int.Parse(purchaseOrderMaster.CompanyId.ToString());
                    purchaseOrderMaster.Date = DateTime.Now;
                    purchaseOrderMaster.PODetails = new List<PODetail>
                    {
                        new PODetail
                        {
                    POID = purchaseOrderMaster.POID,
                    PDID = int.Parse(ProductUnit),
                    Quantity = int.Parse(pODetail.Quantity.ToString()),
                    BatchNo= pODetail.BatchNo.ToString()
                        }

                        };
                    
                    db.PurchaseOrderMasters.Add(purchaseOrderMaster);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Purcase Register Sucessfully."});

                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string z=string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return Json(new { success = false, message = "Purchase UnSucessfully." });
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
