using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalYear.Models;

namespace FinalYear.Models
{
    public class _6digitRand
    {
       

        public  int GenerateRnd()
        {
            int randomNumber = 0;
            Random random = new Random();

            while (true)
            {
                randomNumber = random.Next(100000, 1000000);
                
                if (!NumberExistsInDatabase(randomNumber))
                {
                    break;
                }
            }

            return randomNumber;
        }

        private  bool NumberExistsInDatabase(int randomNumber)
        {
            using (SmartInventoryEntities _db = new SmartInventoryEntities())
            {
               var value= _db.Products.Find(randomNumber);
                if (value==null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}