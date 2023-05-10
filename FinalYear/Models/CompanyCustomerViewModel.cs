using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalYear.Models
{
    public class CompanyCustomerViewModel
    {
        public IEnumerable<Company> Companies { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
    }
}