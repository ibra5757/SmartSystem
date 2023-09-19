using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalYear.Models
{
    public class Featurerequest
    {
        public string target { get; set; }
        public string Temperature { get; set; }
        public string Dew_Point { get; set; }
        public string Wind_Speed { get; set; }
        public string Humidity { get; set; }
        public string Pressure { get; set; }
        public string Condition_int { get; set; }
        public string Event { get; set; }
        public string Seasons { get; set; }
        public string type { get; set; }
    }
}