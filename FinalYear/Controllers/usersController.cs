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
    public class usersController : Controller
    {
        private SmartInventoryEntities db = new SmartInventoryEntities();

        // GET: users
        public ActionResult Indexuser()
        {
            return View(db.Users.ToList());
        }
        [HttpPost]
        public JsonResult UpdateStatus(int id, bool isActive)
        {
            User user = db.Users.FirstOrDefault(u => u.UserID == id);
            user.IsActive = isActive;
            db.SaveChanges();
            return Json(new { success = true });
        }
        // GET: users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Name,UserName,Password,CNIC,Contact,Role,IsActive")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: users/Edit/5
        public ActionResult Edit(int? id )
        {
            var ids = Session["UserID"];
            id = Convert.ToInt32(ids);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string newpassword, string oldpassword)
        {
                
            if (oldpassword != null)
            {
                oldpassword = LoginHelper.GetMD5(oldpassword);
                var check = db.Users.Where(x => x.Password == oldpassword).FirstOrDefault();
                if (check != null)
                {
                    var ids = Session["Userid"];
                    newpassword = LoginHelper.GetMD5(newpassword);

                    if (ModelState.IsValid)
                    {
                        var chng = db.Users.Where(x => x.Password == oldpassword).First();
                        chng.Password = newpassword;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        // GET: users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult checkbox(bool chkbox)
        {
            User user = db.Users.Find(chkbox);
            db.Users.Remove(user);
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
