using System.Configuration;

namespace FinalYear.Models
{
    public class URLClass : IURLClass
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
        public string BaseUrl()
        { 
            return ConfigurationManager.AppSettings["BaseUrl"]; 
        }
    }
}