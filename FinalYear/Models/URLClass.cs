using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FinalYear.Models
{
    public class URLClass
    {

        //private string _url= "http://192.168.0.108:105/";

        //public string  Geturl(string url)
        //{
        //     url= _url;
        //    return url;
        //}
        /// <summary>
        /// Get BaseUrl
        /// </summary>
        /// <returns></returns>
        internal static string BaseUrl()
        { 
            return ConfigurationManager.AppSettings["BaseUrl"]; 
        }
    }
}