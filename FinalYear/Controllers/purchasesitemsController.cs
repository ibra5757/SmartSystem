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
    public class purchasesitemsController : Controller
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();

        // GET: purchasesitems
        public ActionResult Index()
        {
            var purchasesitems = db.PODetails.Include(p => p.ProDetail).Include(p => p.ProDetail.ProId).Include(p => p.PurchaseOrderMaster);
            return View(purchasesitems.ToList());
        }

        // GET: purchasesitems/Details/5
        public ActionResult Details(int? id)
        {
            
                ViewBag.purchasesitemsList = db.PODetails.Where(x=>x.POID==id).ToList();
            
            return PartialView();
        }

        // GET: purchasesitems/Create
        public ActionResult Create()
        {
            ViewBag.PDId = new SelectList(db.ProDetails, "PDId", "ProUnit");
            ViewBag.ProID = new SelectList(db.Products, "ProID", "ProName");
            ViewBag.PurchaseId = new SelectList(db.PurchaseOrderMasters, "PurchaseId", "PurchaseId");
            ViewBag.SupplierID = new SelectList(db.Companies, "SupplierID", "Name");
            return View();
        }

        // POST: purchasesitems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PurchaseId,ProID,PDId,Quantity,SupplierID")] PODetail purchasesitem)
        {
            if (ModelState.IsValid)
            {
                db.PODetails.Add(purchasesitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PDId = new SelectList(db.ProDetails, "PDId", "ProUnit", purchasesitem.PODetail_Id);
            ViewBag.ProID = new SelectList(db.Products, "ProID", "ProName", purchasesitem.ProDetail);
            ViewBag.PurchaseId = new SelectList(db.PurchaseOrderMasters, "PurchaseId", "PurchaseId", purchasesitem.POID);
          //  ViewBag.SupplierID = new SelectList(db.Companies, "SupplierID", "Name", purchasesitem.c);
            return View(purchasesitem);
        }

        // GET: purchasesitems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           PODetail  purchasesitem = db.PODetails.Find(id);
            if (purchasesitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.PDId = new SelectList(db.ProDetails, "PDId", "ProductUnit", purchasesitem.PODetail_Id);
            ViewBag.ProID = new SelectList(db.Products, "ProID", "ProName", purchasesitem.POID);
            ViewBag.POID = new SelectList(db.PurchaseOrderMasters, "POID", "POID", purchasesitem.POID);

            return PartialView("_Edit",purchasesitem);
        }

        // POST: purchasesitems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PurchaseId,ProID,PDId,Quantity,SupplierID")] PODetail purchasesitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchasesitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PDId = new SelectList(db.ProDetails, "PDId", "ProUnit", purchasesitem.PODetail_Id);
            ViewBag.ProID = new SelectList(db.Products, "ProID", "ProName", purchasesitem.POID);
            ViewBag.PurchaseId = new SelectList(db.PurchaseOrderMasters, "PurchaseId", "PurchaseId", purchasesitem.POID);

            return View(purchasesitem);
        }

        // GET: purchasesitems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PODetail purchasesitem = db.PODetails.Find(id);
            if (purchasesitem == null)
            {
                return HttpNotFound();
            }
            return View(purchasesitem);
        }

        // POST: purchasesitems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PODetail purchasesitem = db.PODetails.Find(id);
            db.PODetails.Remove(purchasesitem);
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
