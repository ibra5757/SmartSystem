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
    public class SubcategoriesController : Controller
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();

        // GET: Subcategories
        public ActionResult _index()
        {
            
            return PartialView();
        }
        public PartialViewResult subcatlist()
        {
            ViewBag.Subcatagory = db.SubCategories.ToList();
            return PartialView("~/Views/Subcategories/_index.cshtml");
        }
        [HttpPost]
        public JsonResult FindByName(string SubCatname)
        {

            var Category = db.SubCategories.Where(x => x.SubCatname == SubCatname).FirstOrDefault();

            bool isUnique = false;
            if (Category == null)
            {
                isUnique = true;
            }
            return Json(new { isUnique = isUnique, JsonRequestBehavior.AllowGet });

        }

        // GET: Subcategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subcategory = db.SubCategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return PartialView("Detail",subcategory);
        }

        // GET: Subcategories/Create
        public ActionResult Create()
        {
            ViewBag.CatID = new SelectList(db.Categories, "CatID", "Catname");
            return PartialView();
        }

        // POST: Subcategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

       [HttpPost]
        public JsonResult Create(SubCategory subcategory)
        {
            if (ModelState.IsValid)
            {
                db.SubCategories.Add(subcategory);
                db.SaveChanges();

                return Json(new { success = true, message = "Category Sucessfully." });
            }


            return Json(new { success = false, message = "Category False." });


        }

        // GET: Subcategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subcategory = db.SubCategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatID = new SelectList(db.Categories, "CatID", "Catname", subcategory.CatID);
            return PartialView(subcategory);
        }

        // POST: Subcategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubCatID,SubCatname,CatID")] SubCategory subcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","ManageStock");
            }
            ViewBag.CatID = new SelectList(db.Categories, "CatID", "Catname", subcategory.CatID);
            return PartialView(subcategory);
        }

       

        // POST: Subcategories/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            SubCategory subcategory = db.SubCategories.Find(id);
            db.SubCategories.Remove(subcategory);
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
