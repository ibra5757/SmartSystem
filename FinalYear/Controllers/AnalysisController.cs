using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using FinalYear.Models;

namespace FinalYear.Controllers
{
    public class AnalysisController : Controller
    {
        // GET: Analysis
        public async Task<ActionResult> Index()
        {
            //string Baseurl = "http://192.168.43.236:105/";
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(Baseurl);
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage Res =await client.GetAsync("data");
            //    if (Res.IsSuccessStatusCode)
            //    {
            //        var EmpRespons = Res.Content.ReadAsStringAsync().Result;
                    
            //        //EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);
            //        //var myfilename = string.Format(@"{0}", Guid.NewGuid());
            //        //EmpRespons = EmpRespons.Replace("\"", string.Empty).Trim();

            //       // ViewBag.MyString = EmpRespons;
                    
            //        ////Generate unique filename
            //        //string filepath = @"C:\Users\Ibrahim\Desktop\" + myfilename + ".jpeg";
            //        //imagebuffer.imageBuffe = Convert.FromBase64String(EmpResponse);
                    
            //        //using (var imageFile = new FileStream(filepath, FileMode.Create))
            //        //{
            //        //    imageFile.Write(bytess, 0, bytess.Length);
            //        //    imageFile.Flush();
            //        //}
            //    }
                return View();

            
        }
        
    }
}