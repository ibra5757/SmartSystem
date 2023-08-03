using FinalYear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Security;
using WebMatrix.WebData;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.IO;

namespace FinalYear.Controllers
{
    public class AccountController : Controller
    {
        // GET: credentials

        public string Baseurl = "http://127.0.0.1:105";
        private static string userRole;

        public static string UserRole
        {
            get { return userRole; }
           private set { userRole = value; }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["loginerror"] = "Username and password are required.";
                return View("Login");
            }

            var hashedPassword = LoginHelper.GetMD5(password);

            using (SmartInventoryEntities db = new SmartInventoryEntities())
            {
                var user = db.Users.FirstOrDefault(a => a.UserName.Equals(username) && a.Password.Equals(hashedPassword) && (a.IsActive == true || a.IsActive == null));
                if (user != null)
                {
                    userlogsController userlog = new userlogsController();
                    UserLog userlogs = new UserLog();
                    userlogs.Activity = user.Name + " Login Successful";
                    userlogs.UserID = user.UserID;
                    userlogs.Date = DateTime.Now;
                    userlog.Create(userlogs);

                    FormsAuthentication.SetAuthCookie(username, false);
                    Session["FullName"] = user.Name;
                    Session["UserID"] = user.UserID.ToString();
                    Session["UserName"] = user.UserName;
                    userRole = user.Role;

                    return RedirectToAction("Dashboard");
                }
                else
                {
                    TempData["loginerror"] = "Invalid username or password.";
                    return View("Login");
                }
            }
        }

        public async Task<ActionResult> Dashboard()
        {
            if (Session["UserID"] != null)
            {
                  return View("Dashboard");
                

            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            user.Role = Permission.user;
            user.IsActive = false;
            if (ModelState.IsValid)
            {
                using (SmartInventoryEntities _db = new SmartInventoryEntities())
                {
                    var check = _db.Users.FirstOrDefault(s => s.UserName == user.UserName);
                    if (check == null)
                    {
                        user.Password = LoginHelper.GetMD5(user.Password);
                        _db.Configuration.ValidateOnSaveEnabled = false;
                        _db.Users.Add(user);
                        _db.SaveChanges();
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.error = "UserName already exists";
                        return View();
                    }

                }

            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
        public ActionResult Forgert()
        {
            return View();
        }

        [AllowAnonymous]
        public string CheckUserName(string input)
        {
            using (SmartInventoryEntities _db = new SmartInventoryEntities())
            {
                var ifuser = _db.Users.Where(x => x.UserName == input).FirstOrDefault();
                if (ifuser == null)
                {
                    return "Available";
                }
                else
                {
                    return "Not Available";
                }
            }
        }
        [HttpPost]
        public  async Task<ActionResult> show(string startDate, string endDate, string type)
        {
            var data = await relyAsync(startDate, endDate,type);
            return PartialView();
        }
       
        [HttpPost]

       
        public async Task<string> relyAsync(string startDate,string endDate,string type )
        {

            object mydata = new
            {
                startDate =startDate ,
                endDate=endDate,
                type=type
            };
            var myContent = JsonConvert.SerializeObject(mydata);
            var EmpRespons = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.PostAsJsonAsync(Baseurl + "GetGraphs", mydata);
                if (Res.IsSuccessStatusCode)
                {
                    EmpRespons = Res.Content.ReadAsStringAsync().Result;
                    //EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);
                    //var myfilename = string.Format(@"{0}", Guid.NewGuid());
                    EmpRespons = EmpRespons.Replace("\"", string.Empty).Trim();
                    ViewBag.Data = EmpRespons;
                    ////Generate unique filename
                    //string filepath = @"C:\Users\Ibrahim\Desktop\" + myfilename + ".jpeg";
                    //imagebuffer.imageBuffe = Convert.FromBase64String(EmpResponse);

                    //using (var imageFile = new FileStream(filepath, FileMode.Create))
                    //{
                    //    imageFile.Write(bytess, 0, bytess.Length);
                    //    imageFile.Flush();
                    //}
                }
                return EmpRespons;
            }
        }
        [HttpGet]
        public  async Task<ActionResult> GetVs()
        {
            using (var client = new HttpClient())
            {

                
                
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(Baseurl + "image");
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        var imageData = memoryStream.ToArray();
                        var imageDataString = Convert.ToBase64String(imageData);
                        var model = new ImageModel { ImageData = imageDataString };
                        return PartialView("_piechartview",model);
                    }
                    // Use the Image object
                }
            }
        }
        //public async Task<string> get()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var EmpRespons="";
        //        client.BaseAddress = new Uri(Baseurl);
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        HttpResponseMessage Res = await client.GetAsync("GetGraphs");
        //        if (Res.IsSuccessStatusCode)
        //        {
        //            EmpRespons = Res.Content.ReadAsStringAsync().Result;
        //            //EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);
        //            //var myfilename = string.Format(@"{0}", Guid.NewGuid());
        //            EmpRespons = EmpRespons.Replace("\"", string.Empty).Trim();
        //            ViewBag.Data = string.Empty;
        //            ViewBag.Data = EmpRespons;
        //            ////Generate unique filename
        //            //string filepath = @"C:\Users\Ibrahim\Desktop\" + myfilename + ".jpeg";
        //            //imagebuffer.imageBuffe = Convert.FromBase64String(EmpResponse);

        //            //using (var imageFile = new FileStream(filepath, FileMode.Create))
        //            //{
        //            //    imageFile.Write(bytess, 0, bytess.Length);
        //            //    imageFile.Flush();
        //            //}
        //        }
        //        else
        //        {
        //            ViewBag.Data = null;
        //        }
        //        return EmpRespons;
        //    }
        //}
    }

}