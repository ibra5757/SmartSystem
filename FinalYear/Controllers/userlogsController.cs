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
    public class userlogsController : Controller
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();

        // GET: userlogs
        public ActionResult Index()
        {
            int Userid= Convert.ToInt32(Session["UserID"].ToString());
            var userlogs = db.UserLogs.Where(x => x.UserID == Userid);
            return View("_list",userlogs.ToList());
        }

        // GET: userlogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLog userlog = db.UserLogs.Find(id);
            if (userlog == null)
            {
                return HttpNotFound();
            }
            return View(userlog);
        }

        // GET: userlogs/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name");
            return View();
        }

        // POST: userlogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LogID,UserID,Activity,Date")] UserLog userlog)
        {
            if (ModelState.IsValid)
            {
                db.UserLogs.Add(userlog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", userlog.UserID);
            return View(userlog);
        }

        // GET: userlogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLog userlog = db.UserLogs.Find(id);
            if (userlog == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", userlog.UserID);
            return View(userlog);
        }

        // POST: userlogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LogID,UserID,Activity,Date")] UserLog userlog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userlog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", userlog.UserID);
            return View(userlog);
        }

        // GET: userlogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLog userlog = db.UserLogs.Find(id);
            if (userlog == null)
            {
                return HttpNotFound();
            }
            return View(userlog);
        }

        // POST: userlogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserLog userlog = db.UserLogs.Find(id);
            db.UserLogs.Remove(userlog);
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
