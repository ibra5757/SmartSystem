
using FinalYear.Interface;
using System.Configuration;

namespace FinalYear.Models
{
    public class URLClass : IURLClass
    {

         
        /// <summary>
        /// Get BaseUrl
        /// </summary>
        /// <returns></returns>
        public  string BaseUrl()
        { 
            return ConfigurationManager.AppSettings["BaseUrl"]; 
        }
    }
}