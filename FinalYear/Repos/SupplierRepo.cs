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
        private readonly INVENTORY_SYSTEMEntities context; 
        public SupplierRepo()
        {
            context = new INVENTORY_SYSTEMEntities();
        }
        public SupplierRepo(INVENTORY_SYSTEMEntities context)
        {
            context = this.context;
        }
        public async Task<int> AddNewSupplier(Supplier model)
        {
            var newSupplier = new Supplier()
            {
                Name = model.Name,
                Contact = model.Contact,
                Company = model.Company
            };


            context.Suppliers.Add(newSupplier);
            await context.SaveChangesAsync();

            return newSupplier.SupplierID;
        }
        public async Task<List<Supplier>> GetAllSupplier()
        {


            var sup = new List<Supplier>();
            var data = await context.Suppliers.ToListAsync();
            if (data?.Any() == true)
            {
                foreach (var item in data)
                {
                    sup.Add(new Supplier()
                    {
                        Name = item.Name,
                        Contact = item.Contact,
                        Company = item.Company


                    });
                }
            }
            return sup;

        }



        public async Task<Supplier> GetSupplierById(int id)
        {
            return await context.Suppliers.Where(x => x.SupplierID == id).Select(_Supplier => new Supplier()
            {
                Name = _Supplier.Name,
                Contact = _Supplier.Contact,
                Company = _Supplier.Company

            }).FirstOrDefaultAsync();

        }


        public  async Task<List<Supplier>> SearchSupplier(string Search)
        {
            return await  context.Suppliers.Where(x => x.Company.ToUpper().Contains(Search) || x.Contact.ToUpper().Contains(Search) || x.Name.ToUpper().Contains(Search) || Search == null).ToListAsync();


        }


    }
}
