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

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.SalesOrderMasters = new HashSet<SalesOrderMaster>();
            this.UserLogs = new HashSet<UserLog>();
            this.PurchaseOrderMasters = new HashSet<PurchaseOrderMaster>();
        }
    
        public int UserID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email or Phone")]
        [RegularExpression(@"^((\+92)?(0092)?(92)?(0)?)(3)([0-9]{9})$", ErrorMessage = "Please enter a valid email address or phone number")]

        public string Contact { get; set; }
        [Required]
        [Display(Name="C.N.I.C")]
        [RegularExpression("^[0-9]{5}-[0-9]{7}-[0-9]$", ErrorMessage = "CNIC No must follow the XXXXX-XXXXXXX-X format!")]

        public string CNIC { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        
        public string Role { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesOrderMaster> SalesOrderMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLog> UserLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrderMaster> PurchaseOrderMasters { get; set; }
    }
}
