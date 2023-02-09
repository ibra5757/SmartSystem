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
            Task<int> AddNewSupplier(Supplier model);
            Task<List<Supplier>> GetAllSupplier();
            Task<Supplier> GetSupplierById(int id);
             Task<List<Supplier>> SearchSupplier(string Search);
        
        
    }
}