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
    using System.ComponentModel.DataAnnotations;

    public partial class Ledger
    {
        public int ID { get; set; }
        public Nullable<int> CusID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<int> CompID { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
