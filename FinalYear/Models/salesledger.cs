//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinalYear.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class salesledger
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string Explanation { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public Nullable<int> CusID { get; set; }
        public string Status { get; set; }
    
        public virtual customer customer { get; set; }
    }
}