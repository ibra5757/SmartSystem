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

        // GET: Supplier
        public ActionResult Index(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
        {
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name_Description" : "";
            ViewBag.SortingDate = Sorting_Order == "Date_Enroll" ? "Date_Description" : "Date";

            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;

            var students = from stu in db.Companies select stu;

            if (!String.IsNullOrEmpty(Search_Data))
            {
                students = students.Where(stu => stu.CompanyName.ToUpper().Contains(Search_Data.ToUpper())
                    || stu.Contact.ToUpper().Contains(Search_Data.ToUpper()));
            }
                switch (Sorting_Order)
                {
                    case "Name_Description":
                        students = students.OrderByDescending(stu => stu.CompanyName);
                        break;
                    case "Date_Enroll":
                        students = students.OrderBy(stu => stu.Contact);
                        break;
                    default:
                        students = students.OrderBy(stu => stu.CompanyName);
                        break;
                }

            int Size_Of_Page = 4;
            int No_Of_Page = (Page_No ?? 1);
            return View(students.ToPagedList(No_Of_Page, Size_Of_Page));
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
