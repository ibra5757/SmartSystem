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

        
        IURLClass baseUrl = new URLClass();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public class EvaluationResult
        {
            public Dictionary<string, ModelResult> Models { get; set; }
        }

        public class ModelResult
        {
            public Dictionary<string, object> BestParameters { get; set; }
            public int InputFeatures { get; set; }
            public double MAE { get; set; }
            public double MSE { get; set; }
            public double R2 { get; set; }
            public int RequiredFeatures { get; set; }
            public double RMSE { get; set; }
            public double TestingScore { get; set; }
            public double TrainingScore { get; set; }
        }


        [HttpPost]
        public async Task<ActionResult> Getdata(string MedName)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl.BaseUrl());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("http://192.168.43.236:105/data");

                if (Res.IsSuccessStatusCode)
                {
                    var Datagotfromapi = await Res.Content.ReadAsStringAsync();
                    var evaluationResult = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(Datagotfromapi);
                    string concatenatedData = "";

                    foreach (var kvp in evaluationResult)
                    {
                        string index = kvp.Key;
                        Dictionary<string, object> values = kvp.Value;

                        string concatenatedValues = string.Join(", ", values.Values);
                        string concatenatedEntry = $"{index}: {concatenatedValues}";

                        concatenatedData += concatenatedEntry + Environment.NewLine;
                    }
                        ViewBag.EvaluationResult = concatenatedData;
                    
                    return PartialView("_ShowAnalysis");
                }
                else
                {
                    // Handle API request failure
                    return PartialView("_Error");
                }
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