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
        private INVENTORY_SYSTEMEntities db = new INVENTORY_SYSTEMEntities();

        // GET: purchases
        public ActionResult Index()
        {
            ViewBag.Unit = db.ProDetails.Select(x => x.ProUnit).Distinct().ToList();
            ViewBag.Type = db.ProDetails.Select(x => x.Type).Distinct().ToList();
            ViewBag.AllProduct = db.products.ToList();
            ViewBag.SupplierID = db.Suppliers.ToList();
            var purchases = db.purchases.ToList();
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
            purchase purchase = db.purchases.Find(id);
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
            var SUBID = db.ProDetails.Where(x=>x.ProId ==name).Select(x=>new { x.Type ,x.ProId }).Distinct().ToList();
            return Json(SUBID,JsonRequestBehavior.AllowGet);
        }
        public JsonResult pro(int name,string type)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var pro_ = db.ProDetails.Where(x => x.ProId == name && x.Type==type).ToList();

            return Json(pro_, JsonRequestBehavior.AllowGet);

        }

        // POST: purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string SupplierID,string ProName,string Quantity,string Type,string ProUnit,string BatchId)
        {
            try
            {
                var UserID = Session["UserID"].ToString();

                if (ModelState.IsValid)
                {
                    if (ProName != string.Empty  && Type != string.Empty)
                    {
                        int pid = int.Parse(ProName);
                        int pdid = int.Parse(ProUnit);
                        
                        var quantity = db.ProDetails.Where(x => x.ProId == pid && x.PDId == pdid).Select(x => x.Quantity).FirstOrDefault();
                        if (quantity != 0)
                        {

                            ProDetail pd = db.ProDetails.Where(x => x.ProId == pid && x.PDId == pdid).FirstOrDefault();
                            pd.Quantity = quantity + int.Parse(Quantity);
                            db.Entry(pd).State = EntityState.Modified;
                            
                        }
                        db.SaveChanges();
                        purchase purchase = new purchase();
                        purchase.UserID = int.Parse(UserID);
                        purchase.SupplierID = int.Parse(SupplierID);
                        purchase.Date = DateTime.Now;
                        db.purchases.Add(purchase);
                        db.SaveChanges();
                        purchasesitem purchasesitem = new purchasesitem();
                        purchasesitem.PurchaseId = purchase.PurchaseId;
                        purchasesitem.ProID = int.Parse(ProName);
                        purchasesitem.PDId = int.Parse(ProUnit);
                        purchasesitem.Quantity = int.Parse(Quantity);
                        purchasesitem.SupplierID = int.Parse(SupplierID);
                        db.purchasesitems.Add(purchasesitem);
                        db.SaveChanges();
                    }
                    else
                    {
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
            }
            return RedirectToAction("Index");
        }

        // GET: purchases/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchase purchase = db.purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.users, "UserID", "Name", purchase.UserID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", purchase.SupplierID);
            return View(purchase);
        }

        // POST: purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PurchaseId,Date,UserID,SupplierID")] purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.users, "UserID", "Name", purchase.UserID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", purchase.SupplierID);
            return View(purchase);
        }

        // GET: purchases/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchase purchase = db.purchases.Find(id);
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
            purchase purchase = db.purchases.Find(id);
            db.purchases.Remove(purchase);
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
