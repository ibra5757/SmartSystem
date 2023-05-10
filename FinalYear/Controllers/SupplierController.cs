using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalYear.Models;
using PagedList;

namespace FinalYear.Controllers
{
    public class SupplierController : Controller
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SupplierTabList(string tab)
        {
            db.Configuration.ProxyCreationEnabled = false;

            SupplierController supplierController = new SupplierController();
            switch (tab)
            {
                case "#tab2":
                    var plist = supplierController.AllCustomers();
                    return plist;
                default:
                    var catlist = supplierController.List();
                    return catlist;

            }



        }
        public PartialViewResult AllCustomers()
        {
            db.Configuration.ProxyCreationEnabled = false;

            var customer = db.Customers.ToList();
            return PartialView("~/Views/Customers/list.cshtml", customer);
        }
        // GET: Supplier
        public PartialViewResult List()
        {

            var Company = db.Companies.ToList();
            
            return PartialView("List",Company);
        }

        // GET: Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company supplier = db.Companies.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Details",supplier);
        }

        // GET: Supplier/Create
        public ActionResult _Create()
        {
            return PartialView();
        }

        // POST: Supplier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company supplier)
        {
            supplier.Balance = 0;
            if (ModelState.IsValid)
            {
                db.Companies.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        // GET: Supplier/Edit/5
        public ActionResult _Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company supplier = db.Companies.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return PartialView(supplier);
        }

        // POST: Supplier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Edit(Company supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(supplier);
        }

        // GET: Supplier/Delete/5
        public ActionResult _Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company supplier = db.Companies.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return PartialView(supplier);
        }

        // POST: Supplier/Delete/5
        [HttpPost]
        public JsonResult DeleteConfirmed(int id)
        {
            Company supplier = db.Companies.Find(id);
            db.Companies.Remove(supplier);
            db.SaveChanges();

            return Json(new { success = true, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
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
