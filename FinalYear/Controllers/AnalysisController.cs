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
                HttpResponseMessage Res = await client.GetAsync("http://127.0.0.1:105/data");

                if (Res.IsSuccessStatusCode)
                {
                    var Datagotfromapi = await Res.Content.ReadAsStringAsync();
                    var evaluationResult = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(Datagotfromapi);
                    List<summary> evaluationResults = new List<summary>();
                    foreach (var kvp in evaluationResult)
                    {
                        string index = kvp.Key;
                        Dictionary<string, object> values = kvp.Value;

                        string concatenatedValues = string.Join(Environment.NewLine, values.Select(pair => $"{pair.Key}: {pair.Value}"));

                        string concatenatedEntry = $"{index}: {concatenatedValues}";

                        summary entry = new summary
                        {
                            Name = index,
                            Values = concatenatedValues
                        };
                        evaluationResults.Add(entry);
                    }

                    ViewBag.EvaluationResults = evaluationResults;


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
        public async Task<ActionResult> GetPrediction(string MedName, string typeselect, string Temperature,string Dew_Point, string Wind_Speed, string Humidity, string Pressure,string Condition_int, string Event,string Wind)
        {
            //var myfilename = string.Format(@"{0}", Guid.NewGuid());
            //EmpRespons = EmpRespons.Replace("\"", string.Empty).Trim();

            using (var client = new HttpClient())
            {

                object mydat2a = new
                {
                    MedName=MedName, typeselect= typeselect, Temperature= Temperature, Dew_Point= Dew_Point,
                    Wind= Wind, Wind_Speed = Wind_Speed, Humidity= Humidity, Pressure= Pressure, Condition_int= Condition_int,
                    Event= Event
                };
                var myContent = JsonConvert.SerializeObject(mydat2a);

                client.BaseAddress = new Uri(baseUrl.BaseUrl());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.PostAsJsonAsync("http://127.0.0.1:105/Applygradientdecent", mydat2a);
                if (Res.IsSuccessStatusCode)
                {
                    var Datagotfromapi = Res.Content.ReadAsStringAsync().Result;

                    var predition = JsonConvert.DeserializeObject<PredictedValue>(Datagotfromapi);

                    PredictedValue predictedValues = new PredictedValue()
                    {
                        Predictions = predition.Predictions
                    };

                    ViewBag.predicted = null;
                    ViewBag.predicted = predictedValues;

                    // ...
                }
                return PartialView("_Aftergradientdecent");

            }
        }


    }

}