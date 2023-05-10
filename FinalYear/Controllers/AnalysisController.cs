using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using FinalYear.Models;
using FinalYear.Interface;

namespace FinalYear.Controllers
{
    public class AnalysisController : Controller
    {
        // GET: Analysis

        //string Baseurl = "http://192.168.0.108:105/";

        IURLClass baseUrl = new URLClass();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<ActionResult> Getdata(string MedName)
        {
            //var myfilename = string.Format(@"{0}", Guid.NewGuid());
            //EmpRespons = EmpRespons.Replace("\"", string.Empty).Trim();

            using (var client = new HttpClient())
            {

                object mydat2a = new
                {
                    MedName = MedName

                };
                var myContent = JsonConvert.SerializeObject(mydat2a);
                
                client.BaseAddress = new Uri(baseUrl.BaseUrl());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.PostAsJsonAsync($"{baseUrl.BaseUrl()} data" ,mydat2a);
                if (Res.IsSuccessStatusCode)
                {
                    var Datagotfromapi = Res.Content.ReadAsStringAsync().Result;

                    var news = Datagotfromapi.Replace("\"[", "[").Replace("]\"", "]").Replace("\\", "");

                    var summaries = JsonConvert.DeserializeObject<summary>(news);

                    summary summarys = new summary() {

                        Coefficients=summaries.Coefficients,
                        Intercept=summaries.Intercept,
                        meansquareerror=summaries.meansquareerror

                    };

                    


                    ViewBag.Summary = summaries ;

                    ////Generate unique filename
                    //string filepath = @"C:\Users\Ibrahim\Desktop\" + myfilename + ".jpeg";
                    //imagebuffer.imageBuffe = Convert.FromBase64String(EmpResponse);

                }
                return PartialView("_ShowAnalysis");

            }
        }
        [HttpPost]
        public async Task<ActionResult> GetPrediction(string MedName, string typeselect, string Temperature,string Dew_Point, string Wind_Speed, string Humidity, string Pressure,string Condition_int)
        {
            //var myfilename = string.Format(@"{0}", Guid.NewGuid());
            //EmpRespons = EmpRespons.Replace("\"", string.Empty).Trim();

            using (var client = new HttpClient())
            {

                object mydat2a = new
                {
                    MedName=MedName, typeselect= typeselect, Temperature= Temperature, Dew_Point= Dew_Point, Wind_Speed= Wind_Speed, Humidity= Humidity, Pressure= Pressure, Condition_int= Condition_int
                };
                var myContent = JsonConvert.SerializeObject(mydat2a);

                client.BaseAddress = new Uri(baseUrl.BaseUrl());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.PostAsJsonAsync(  $"{baseUrl.BaseUrl()}Applygradientdecent", mydat2a);
                if (Res.IsSuccessStatusCode)
                {
                    var Datagotfromapi = Res.Content.ReadAsStringAsync().Result;

                    var news = Datagotfromapi.Replace("\"[", "[").Replace("]\"", "]").Replace("\\", "");

                    var predition = JsonConvert.DeserializeObject<PredictedValue>(news);

                    PredictedValue PredictedValues = new PredictedValue()
                    {
                        
                        PredictedValues = predition.PredictedValues

                    };



                    ViewBag.predicted = null;
                    ViewBag.predicted = predition;

                    ////Generate unique filename
                    //string filepath = @"C:\Users\Ibrahim\Desktop\" + myfilename + ".jpeg";
                    //imagebuffer.imageBuffe = Convert.FromBase64String(EmpResponse);

                }
                return PartialView("_Aftergradientdecent");

            }
        }


    }

}