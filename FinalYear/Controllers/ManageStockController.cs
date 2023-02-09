using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalYear.Models;
using System.Dynamic;

namespace FinalYear.Controllers
{
    public class ManageStockController : Controller
    {
        private INVENTORY_SYSTEMEntities db = new INVENTORY_SYSTEMEntities();

        // GET: ManageStock

        public ActionResult Index()
        {
            ViewBag.ProCmb = db.products;
            ViewBag.Pd_type = db.ProDetails;

            ViewBag.proDetails = db.ProDetails.Include(p => p.product);
            ViewBag.ProductPartial = db.products.Include(p => p.category).Include(p => p.Subcategory);
            ViewBag.list = db.categories.ToList();
            ViewBag.Subcatagory= db.Subcategories.ToList();
            return View();
        }


        // GET: ManageStock/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return PartialView("_detail",product);
        }

        // GET: ManageStock/Create
        public ActionResult _create()
        {
            ViewBag.CatID = new SelectList(db.categories, "CatID", "Catname");
            ViewBag.SubCatID = new SelectList(db.Subcategories, "SubCatID", "SubCatname");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name");
            return PartialView();
        }

        // POST: ManageStock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ProName,CatID,SubCatID")] product product)
        {
            if (ModelState.IsValid)
            {
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatID = new SelectList(db.categories, "CatID", "Catname", product.CatID);
            ViewBag.SubCatID = new SelectList(db.Subcategories, "SubCatID", "SubCatname", product.SubCatID);
            return View(product);
        }


        // GET: ManageStock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatID = new SelectList(db.categories, "CatID", "Catname", product.CatID);
            ViewBag.SubCatID = new SelectList(db.Subcategories, "SubCatID", "SubCatname", product.SubCatID);
            return PartialView(product);
        }

        // POST: ManageStock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProID,ProName,Quantity,price,packing,Date,CatID,SubCatID,SupplierID")] product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatID = new SelectList(db.categories, "CatID", "Catname", product.CatID);
            ViewBag.SubCatID = new SelectList(db.Subcategories, "SubCatID", "SubCatname", product.SubCatID);
            return View(product);
        }

        // GET: ManageStock/Delete/5
        public ActionResult _Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete",product);
        }

        // POST: ManageStock/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index","ManageStock");
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
