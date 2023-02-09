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
        private INVENTORY_SYSTEMEntities db = new INVENTORY_SYSTEMEntities();

        // GET: Subcategories
        public ActionResult _index()
        {
            
            return PartialView();
        }
        //public ActionResult Getdata(JqueryDatatableParam param)
        //{

        //    var subcategories = db.Subcategories.Include(s => s.category).ToList();
        //    if (!string.IsNullOrEmpty(param.sSearch))
        //    {
        //        subcategories = subcategories.Where(x => x.SubCatname.ToLower().Contains(param.sSearch.ToLower())
        //                                      || x.category.Catname.ToLower().Contains(param.sSearch.ToLower())
        //                                      ).ToList();
        //    }
        //    var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
        //    var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
        //     if (sortColumnIndex == 3)
        //    {
        //        subcategories = sortDirection == "asc" ? subcategories.OrderBy(c => c.SubCatname).ToList() : subcategories.OrderByDescending(c => c.SubCatname).ToList();
        //    }
        //    else if (sortColumnIndex == 4)
        //    {
        //        subcategories = sortDirection == "asc" ? subcategories.OrderBy(c => c.category.Catname).ToList() : subcategories.OrderByDescending(c => c.category.Catname).ToList();
        //    }

        //    else
        //    {
        //        Func<Subcategory, string> orderingFunction = e => sortColumnIndex == 0 ? e.SubCatname :
        //                                                       sortColumnIndex == 1 ? e.category.Catname :
        //                                                       e.SubCatname;

        //        subcategories = sortDirection == "asc" ? subcategories.OrderBy(orderingFunction).ToList() : subcategories.OrderByDescending(orderingFunction).ToList();
        //    }
        //    var displayResult = subcategories.Skip(param.iDisplayStart)
        //                .Take(param.iDisplayLength).ToList();
        //    var totalRecords = subcategories.Count();

        //    return Json(new
        //    {
        //        param.sEcho,
        //        iTotalRecords = totalRecords,
        //        iTotalDisplayRecords = totalRecords,
        //        aaData = displayResult
        //    }, JsonRequestBehavior.AllowGet);
        //}



        // GET: Subcategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subcategory subcategory = db.Subcategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return PartialView(subcategory);
        }

        // GET: Subcategories/Create
        public ActionResult _Create()
        {
            ViewBag.CatID = new SelectList(db.categories, "CatID", "Catname");
            return PartialView();
        }

        // POST: Subcategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "SubCatID,SubCatname,CatID")] Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                db.Subcategories.Add(subcategory);
                db.SaveChanges();
                return RedirectToAction("Index", "ManageStock");
            }

            ViewBag.CatID = new SelectList(db.categories, "CatID", "Catname", subcategory.CatID);
            return View("Index","ManageStock");
        }

        // GET: Subcategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subcategory subcategory = db.Subcategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatID = new SelectList(db.categories, "CatID", "Catname", subcategory.CatID);
            return PartialView(subcategory);
        }

        // POST: Subcategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubCatID,SubCatname,CatID")] Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","ManageStock");
            }
            ViewBag.CatID = new SelectList(db.categories, "CatID", "Catname", subcategory.CatID);
            return PartialView(subcategory);
        }

       

        // POST: Subcategories/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            Subcategory subcategory = db.Subcategories.Find(id);
            db.Subcategories.Remove(subcategory);
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
