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
    public class SODetailsController : Controller
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();

        // GET: SODetails
        public ActionResult Index()
        {
            var sODetails = db.SODetails.Include(s => s.ProDetail).Include(s => s.SalesOrderMaster);
            return View(sODetails.ToList());
        }

        // GET: SODetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SODetail sODetail = db.SODetails.Find(id);
            if (sODetail == null)
            {
                return HttpNotFound();
            }
            return View(sODetail);
        }

        // GET: SODetails/Create
        public ActionResult Create()
        {
            ViewBag.PDID = new SelectList(db.ProDetails, "PDId", "ProductUnit");
            ViewBag.SOID = new SelectList(db.SalesOrderMasters, "SOID", "BillNo");
            return View();
        }

        // POST: SODetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SODetail_Id,SOID,PDID,S_Quantity,BatchIdNo,U_price,Discount,TotalPrice")] SODetail sODetail)
        {
            if (ModelState.IsValid)
            {
                db.SODetails.Add(sODetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PDID = new SelectList(db.ProDetails, "PDId", "ProductUnit", sODetail.PDID);
            ViewBag.SOID = new SelectList(db.SalesOrderMasters, "SOID", "BillNo", sODetail.SOID);
            return View(sODetail);
        }

        // GET: SODetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SODetail sODetail = db.SODetails.Find(id);
            if (sODetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.PDID = new SelectList(db.ProDetails, "PDId", "ProductUnit", sODetail.PDID);
            ViewBag.SOID = new SelectList(db.SalesOrderMasters, "SOID", "BillNo", sODetail.SOID);
            return View(sODetail);
        }

        // POST: SODetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SODetail_Id,SOID,PDID,S_Quantity,BatchIdNo,U_price,Discount,TotalPrice")] SODetail sODetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sODetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PDID = new SelectList(db.ProDetails, "PDId", "ProductUnit", sODetail.PDID);
            ViewBag.SOID = new SelectList(db.SalesOrderMasters, "SOID", "BillNo", sODetail.SOID);
            return View(sODetail);
        }

        // GET: SODetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SODetail sODetail = db.SODetails.Find(id);
            if (sODetail == null)
            {
                return HttpNotFound();
            }
            return View(sODetail);
        }

        // POST: SODetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SODetail sODetail = db.SODetails.Find(id);
            db.SODetails.Remove(sODetail);
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
