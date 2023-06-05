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
    public class CustomersController : Controller
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();

        // GET: Customers
        
        [HttpGet]
        public ActionResult GetAllCustomers()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cus=db.Customers.ToList().OrderByDescending(z=>z.CreatedDate);
            return Json(cus, JsonRequestBehavior.AllowGet);
        }

        // GET: Customers/Details/5
        public PartialViewResult Details(int? id)
        {
            
            Customer customer = db.Customers.Find(id);
            
            var balanceQuery = from ledger in db.Ledgers
                               where ledger.CusID == customer.CusID
                               orderby ledger.Date descending
                               select ledger.Balance;

            customer.Balance = balanceQuery.FirstOrDefault() ?? 0;

            return PartialView  ("Details",customer);
        }

        // GET: Customers/Create
        public PartialViewResult Create()
        {
            return PartialView();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CusID,Name,Contact,Address")] Customer customer)
        {
            customer.Balance = 0;
            if (ModelState.IsValid)
            {
                customer.CreatedDate = DateTime.Now;
                db.Customers.Add(customer);

                userlogsController userlog = new userlogsController();
                UserLog userlogs = new UserLog();
                userlogs.Activity = Session["UserName"].ToString() + " Customer Created  Sucessfully";
                userlogs.UserID = (int)Session["UserID"];
                userlogs.Date = DateTime.Now;
                userlog.Create(userlogs);
                db.SaveChanges();
                var all = db.Customers.ToList();
                return Json("Customer created successfully!");

                return Json(new { success = true, message = "Created Sucessfully.", all=all });
            }

            return View(customer);
        }
        public ActionResult Success()
        {
            // You can perform additional logic or simply return the success view
            return View();
        }
        // GET: Customers/Edit/5
        public PartialViewResult Edit(int? id)
        {
            
            Customer customer = db.Customers.Find(id);
           
            return PartialView(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CusID,Name,Contact,Address,Balance")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;

                userlogsController userlog = new userlogsController();
                UserLog userlogs = new UserLog();
                userlogs.Activity = Session["UserName"].ToString() + " Customer EDIT  Sucessfully";
                string z = Session["UserID"].ToString();
                userlogs.UserID = int.Parse(z);
                userlogs.Date = DateTime.Now;
                userlog.Create(userlogs);
                db.SaveChanges();
                return RedirectToAction("Index","Supplier");
            }
            return View(customer);
        }
        

        // POST: Customers/Delete/5
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
