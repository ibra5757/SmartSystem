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
    public class categoriesController : Controller
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();

        [HttpPost]
        public ActionResult Create(Category Category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(Category);
                db.SaveChanges();
                return Json(new { success = true , message = "Category Sucessfully." });
            }

            return Json(new { success = false , message = "Category False." });
        }
        // GET: categories
        public PartialViewResult catagorylistrlist()
        {
            ViewBag.list = db.Categories.ToList();
            return PartialView("~/Views/categories/_index.cshtml");
        }
        public ActionResult _index()
        {
            return PartialView();
        }
        public JsonResult FindByName(string Name)
        {

            var Category = db.Categories.Where(x => x.Catname == Name).FirstOrDefault();

            bool isUnique = false;
            if (Category == null)
            {
                isUnique = true;
            }
            return Json(new {isUnique= isUnique, JsonRequestBehavior.AllowGet});

        }

        // GET: categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category Category = db.Categories.Find(id);
            if (Category == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Detail",Category);
        }
        
        // GET: categories/Create
        public ActionResult _Create()
        {
            return PartialView();
        }

        // POST: categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       

        // GET: categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category Category = db.Categories.Find(id);
            if (Category == null)
            {
                return HttpNotFound();
            }
            return PartialView(Category);
        }

        // POST: categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CatID,Catname")] Category Category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","ManageStock");
            }
            return PartialView(Category);
        }

        // GET: categories/Delete/5
        public ActionResult _Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category Category = db.Categories.Find(id);
            if (Category == null)
            {
                return HttpNotFound();
            }
            return PartialView(Category);
        }

        // POST: categories/Delete/5
        [HttpPost]
        public JsonResult DeleteConfirmed(int id)
        {
            Category Category = db.Categories.Find(id);
            db.Categories.Remove(Category);
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
