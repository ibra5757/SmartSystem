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
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.ProDetails = new HashSet<ProDetail>();
        }
    
        public int ProID { get; set; }
        public string ProName { get; set; }
        public Nullable<int> CatID { get; set; }
        public Nullable<int> SubCatID { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProDetail> ProDetails { get; set; }
        public virtual SubCategory SubCategory { get; set; }
    }
}
