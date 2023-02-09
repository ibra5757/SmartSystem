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

namespace FinalYear.Controllers
{
    public class AccountController : Controller
    {
        // GET: credentials

        public string Baseurl = "http://192.168.67.116:105/";
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
      
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = LoginHelper.GetMD5(password);
                using (INVENTORY_SYSTEMEntities db = new INVENTORY_SYSTEMEntities())
                {
                    if (username.Trim() == "" && password.Trim() == "")
                    {
                        TempData["loginerror"] = "Id & password is Empty";
                    }
                    else if (username.Trim() != "" && password.Trim() == "")
                    {
                        TempData["loginerror"] = "username is invalid";

                    }
                    else if (username.Trim() == "" && password.Trim() != "")
                    {
                        TempData["loginerror"] = "password is worng";
                    }
                    else if (username.Trim()!="" &&password.Trim()!="")
                    {
                        var obj = db.users.Where(a => a.UserName.Equals(username) && a.Password.Equals(f_password)).FirstOrDefault();
                        var unamecheck = db.users.Where(a => a.Password.Equals(username)).FirstOrDefault();
                        var upasswordcheck = db.users.Where(a => a.Password.Equals(f_password)).FirstOrDefault();

                        if (obj != null)
                        {

                            userlogsController userlog = new userlogsController();
                            userlog userlogs = new userlog();
                            userlogs.Activity = obj.Name.ToString() + " Register  Sucessfull";
                            userlogs.UserID = obj.UserID;
                            userlogs.Date = DateTime.Now;
                            userlog.Create(userlogs);
                            FormsAuthentication.SetAuthCookie(username, false);
                            Session["FullName"] = obj.Name.ToString();
                            Session["UserID"] = obj.UserID.ToString();
                            Session["UserName"] = obj.UserName.ToString();
                            return RedirectToAction("Dashboard");
                        }
                        else
                        {
                            if (unamecheck == null && upasswordcheck == null)
                            {

                                TempData["loginerror"] = "Id & password is worng";
                            }
                            else if (unamecheck != null && upasswordcheck == null)
                            {
                                TempData["loginerror"] = "username is Wrong";

                            }
                            else if (unamecheck == null && upasswordcheck != null)
                            {
                                TempData["loginerror"] = "password is worng";
                            }
                            //return Content("<script language='javascript' type='text/javascript'>alert('Invalid Login!!'); </script>");
                            return View("Login");
                        }
                    }
                    
                }
            }

            return View();
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
        public ActionResult Register(user user)
        {
            user.Role = "NO";
            user.IsActive = false;
            if (ModelState.IsValid)
            {
                using (INVENTORY_SYSTEMEntities _db = new INVENTORY_SYSTEMEntities())
                {
                    var check = _db.users.FirstOrDefault(s => s.UserName == user.UserName);
                    if (check == null)
                    {

                        //userlogsController userlog = new userlogsController();
                        //userlog userlogs = new userlog();
                        //userlogs.Activity = check.Name.ToString() + " Register  Sucessfull";
                        //userlogs.UserID = check.UserID;
                        //userlogs.Date = DateTime.Now;
                        //userlog.Create(userlogs);
                        user.Password = LoginHelper.GetMD5(user.Password);
                        _db.Configuration.ValidateOnSaveEnabled = false;
                        _db.users.Add(user);
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
            bool ifuser = WebSecurity.UserExists(input);
            if (ifuser == false)
            {
                return "Available";
            }
            if (ifuser == true)
            {
                return "Not Available";   
            }
            return "";
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

            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
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