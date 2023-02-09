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
        private INVENTORY_SYSTEMEntities db = new INVENTORY_SYSTEMEntities();

        // GET: purchasesitems
        public ActionResult Index()
        {
            var purchasesitems = db.purchasesitems.Include(p => p.ProDetail).Include(p => p.product).Include(p => p.purchase).Include(p => p.Supplier);
            return View(purchasesitems.ToList());
        }

        // GET: purchasesitems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchasesitem purchasesitem = db.purchasesitems.Find(id);
            if (purchasesitem == null)
            {
                return HttpNotFound();
            }
            return View(purchasesitem);
        }

        // GET: purchasesitems/Create
        public ActionResult Create()
        {
            ViewBag.PDId = new SelectList(db.ProDetails, "PDId", "ProUnit");
            ViewBag.ProID = new SelectList(db.products, "ProID", "ProName");
            ViewBag.PurchaseId = new SelectList(db.purchases, "PurchaseId", "PurchaseId");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name");
            return View();
        }

        // POST: purchasesitems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PurchaseId,ProID,PDId,Quantity,SupplierID")] purchasesitem purchasesitem)
        {
            if (ModelState.IsValid)
            {
                db.purchasesitems.Add(purchasesitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PDId = new SelectList(db.ProDetails, "PDId", "ProUnit", purchasesitem.PDId);
            ViewBag.ProID = new SelectList(db.products, "ProID", "ProName", purchasesitem.ProID);
            ViewBag.PurchaseId = new SelectList(db.purchases, "PurchaseId", "PurchaseId", purchasesitem.PurchaseId);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", purchasesitem.SupplierID);
            return View(purchasesitem);
        }

        // GET: purchasesitems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchasesitem purchasesitem = db.purchasesitems.Find(id);
            if (purchasesitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.PDId = new SelectList(db.ProDetails, "PDId", "ProUnit", purchasesitem.PDId);
            ViewBag.ProID = new SelectList(db.products, "ProID", "ProName", purchasesitem.ProID);
            ViewBag.PurchaseId = new SelectList(db.purchases, "PurchaseId", "PurchaseId", purchasesitem.PurchaseId);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", purchasesitem.SupplierID);
            return View(purchasesitem);
        }

        // POST: purchasesitems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PurchaseId,ProID,PDId,Quantity,SupplierID")] purchasesitem purchasesitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchasesitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PDId = new SelectList(db.ProDetails, "PDId", "ProUnit", purchasesitem.PDId);
            ViewBag.ProID = new SelectList(db.products, "ProID", "ProName", purchasesitem.ProID);
            ViewBag.PurchaseId = new SelectList(db.purchases, "PurchaseId", "PurchaseId", purchasesitem.PurchaseId);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", purchasesitem.SupplierID);
            return View(purchasesitem);
        }

        // GET: purchasesitems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchasesitem purchasesitem = db.purchasesitems.Find(id);
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
            purchasesitem purchasesitem = db.purchasesitems.Find(id);
            db.purchasesitems.Remove(purchasesitem);
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
