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
    
    public partial class PODetail
    {
        public int PODetail_Id { get; set; }
        public int POID { get; set; }
        public int PDID { get; set; }
        public int Quantity { get; set; }
        public string BatchNo { get; set; }
    
        public virtual ProDetail ProDetail { get; set; }
        public virtual PurchaseOrderMaster PurchaseOrderMaster { get; set; }
    }
}
