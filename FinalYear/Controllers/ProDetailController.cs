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
    public class ProDetailController : Controller
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();

        // GET: ProDetail
        public ActionResult _ListProDetail()
        {
            return PartialView();
        }
        public PartialViewResult prodetail()
        {
            ViewBag.ProCmb = db.Products;
            ViewBag.Pd_type = db.ProDetails;
            ViewBag.proDetails = db.ProDetails.Include(p => p.Product);
            return PartialView("~/Views/ProDetail/_ListProDetail.cshtml");
        }
        // GET: ProDetail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProDetail proDetail = db.ProDetails.Find(id);
            if (proDetail == null)
            {
                return HttpNotFound();
            }
            return View(proDetail);
        }

        // GET: ProDetail/Create
        public ActionResult _create()
        {
            ViewBag.ProId = new SelectList(db.Products, "ProID", "ProName");
            return View();
        }

        // POST: ProDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public JsonResult Create(ProDetail proDetail)
        {
            if (ModelState.IsValid)
            {
                db.ProDetails.Add(proDetail);
                db.SaveChanges();
                ViewBag.ProCmb = db.Products;
                ViewBag.Pd_type = db.ProDetails;
                return Json(new { success = true, message = "ProductDetail Register Sucessfully." });
            }
            ViewBag.ProCmb = db.Products;
            ViewBag.Pd_type = db.ProDetails;
            ViewBag.ProId = new SelectList(db.Products, "ProID", "ProName", proDetail.ProId);
            return Json(new { success = false, message = "ProductDetail Register Fail."});
        }

        // GET: ProDetail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProDetail proDetail = db.ProDetails.Find(id);
            if (proDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProId = new SelectList(db.Products, "ProID", "ProName", proDetail.ProId);
            return PartialView(proDetail);
        }

        // POST: ProDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PDId,ProUnit,ProId,BatchId,Quantity,Type,Packing,U_Price")] ProDetail proDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProId = new SelectList(db.Products, "ProID", "ProName", proDetail.ProId);
            return View(proDetail);
        }

        // GET: ProDetail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProDetail proDetail = db.ProDetails.Find(id);
            if (proDetail == null)
            {
                return HttpNotFound();
            }
            return View(proDetail);
        }

        // POST: ProDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProDetail proDetail = db.ProDetails.Find(id);
            db.ProDetails.Remove(proDetail);
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
