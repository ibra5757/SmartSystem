using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using FinalYear.Models;
using System.Data.Entity;

namespace FinalYear.Repos
{
    public class SupplierRepo: ISupplierRepo
    {
        private readonly SmartInventoryEntities context; 
        public SupplierRepo()
        {
            context = new SmartInventoryEntities();
        }
        public SupplierRepo(SmartInventoryEntities context)
        {
            context = this.context;
        }
        public async Task<int> AddNewSupplier(Company model)
        {
            var newSupplier = new Company()
            {
                CompanyName = model.CompanyName,
                Contact = model.Contact,
            };


            context.Companies.Add(newSupplier);
            await context.SaveChangesAsync();

            return newSupplier.CompanyID;
        }
        public async Task<List<Company>> GetAllSupplier()
        {


            var sup = new List<Company>();
            var data = await context.Companies.ToListAsync();
            if (data?.Any() == true)
            {
                foreach (var item in data)
                {
                    sup.Add(new Company()
                    {
                        CompanyName = item.CompanyName,
                        Contact = item.Contact,



                    });
                }
            }
            return sup;

        }



        public async Task<Company> GetSupplierById(int id)
        {
            return await context.Companies.Where(x => x.CompanyID == id).Select(_Supplier => new Company()
            {
                CompanyName = _Supplier.CompanyName,
                Contact = _Supplier.Contact,

            }).FirstOrDefaultAsync();

        }


        public  async Task<List<Company>> SearchSupplier(string Search)
        {
            return await  context.Companies.Where(x=>x.Contact.ToUpper().Contains(Search) || x.CompanyName.ToUpper().Contains(Search) || Search == null).ToListAsync();


        }


    }
}
