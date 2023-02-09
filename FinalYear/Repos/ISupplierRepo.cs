using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FinalYear.Models;
namespace FinalYear.Repos
{
    public interface ISupplierRepo
    {
            Task<int> AddNewSupplier(Company model);
            Task<List<Company>> GetAllSupplier();
            Task<Company> GetSupplierById(int id);
             Task<List<Company>> SearchSupplier(string Search);
        
        
    }
}