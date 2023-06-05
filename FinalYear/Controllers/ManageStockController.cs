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
        private SmartInventoryEntities db = new SmartInventoryEntities();

        categoriesController categoriesController = new categoriesController();

        SubcategoriesController subcategoriesController = new SubcategoriesController();


        ProDetailController proDetailController = new ProDetailController();
        // GET: ManageStock
        _6digitRand _6DigitRand = new _6digitRand();
        long dig=0;
        public ActionResult Index()
        {
            
             dig= _6DigitRand.GenerateRnd();
            Session["Rnd"] = dig;
            ViewBag.ProCmb = db.Products.OrderByDescending(z=>z.CreatedDate);
            ViewBag.Pd_type = db.ProDetails.OrderByDescending(z => z.CreatedDate);
            
            
            return View();
        }
        
        public PartialViewResult productlist()
        {
            ViewBag.ProductPartial = db.Products.Include(p => p.Category).Include(p => p.SubCategory).OrderByDescending(x=>x.CreatedDate);
            return PartialView("_Product");
        }
        [HttpGet]
        public JsonResult UpdateTable()
        {
            var updatedTable = db.Products
                .Select(p => new
                {
                    Name = p.ProName,
                    CategoryName = p.Category.Catname,
                    SubCategoryName = p.SubCategory.SubCatname,
                    Actions=p.CatID,
                    CreatedDate=p.CreatedDate
                }).OrderByDescending(z => z.CreatedDate)
                .ToList();

            return Json(new { data = updatedTable }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TabList(string tab)
        {
            switch (tab)
            {
                case "#tab2":
                    var catlist = categoriesController.catagorylistrlist();
                    return catlist ;
                case "#tab3":
                    var subcatlist = subcategoriesController.subcatlist();
                    return subcatlist;
                case "#tab4":
                    var prodetaillisy = proDetailController.prodetail();
                    return prodetaillisy;
                default:

                    ManageStockController manageStockController = new ManageStockController();
                    var plist = manageStockController.productlist();
                    return plist;
            }


            
        }
        [HttpPost]
        public JsonResult FindByName(string ProName)
        {

            var Category = db.Products.Where(x => x.ProName == ProName).FirstOrDefault();

            bool isUnique = false;
            if (Category == null)
            {
                isUnique = true;
            }
            return Json(new { isUnique = isUnique, JsonRequestBehavior.AllowGet });

        }

        // GET: ManageStock/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return PartialView("_detail",product);
        }

        // GET: ManageStock/Create
        public ActionResult _create()
        {
            ViewBag.CatID = new SelectList(db.Categories, "CatID", "Catname");
            ViewBag.SubCatID = new SelectList(db.SubCategories, "SubCatID", "SubCatname");
            ViewBag.SupplierID = new SelectList(db.Companies, "SupplierID", "Name");
            return PartialView();
        }

        // POST: ManageStock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public JsonResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedDate = DateTime.Now;
                db.Products.Add(product);
                db.SaveChanges();
                dig = _6DigitRand.GenerateRnd();
                Session["Rnd"] = dig;
                return Json(new { success = true, message = "Product Register Sucessfully." ,dig=dig});
            }

            ViewBag.CatID = new SelectList(db.Categories, "CatID", "Catname", product.CatID);
            ViewBag.SubCatID = new SelectList(db.SubCategories, "SubCatID", "SubCatname", product.SubCatID);
            dig = _6DigitRand.GenerateRnd();
            Session["Rnd"] = dig;
            return Json(new { success = false, message = "Product Register Fail.", dig = dig });
        }


        // GET: ManageStock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            { 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatID = new SelectList(db.Categories, "CatID", "Catname", product.CatID);
            ViewBag.SubCatID = new SelectList(db.SubCategories, "SubCatID", "SubCatname", product.SubCatID);
            return PartialView("_Edit",product);
        }

        // POST: ManageStock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit( Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Product Register Sucessfully."});
            }
            ViewBag.CatID = new SelectList(db.Categories, "CatID", "Catname", product.CatID);
            ViewBag.SubCatID = new SelectList(db.SubCategories, "SubCatID", "SubCatname", product.SubCatID);
            return Json(new { success = false, message = "Product Register UnSucessfully."});
        }

        // GET: ManageStock/Delete/5
        public ActionResult _Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
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
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
