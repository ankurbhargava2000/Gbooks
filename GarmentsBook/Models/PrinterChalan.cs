//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GarmentSoft.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class PrinterChalan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PrinterChalan()
        {
            this.PrinterChalanDetails = new HashSet<PrinterChalanDetail>();
        }
    
        public int Id { get; set; }

        [ForeignKey("Vendor")]
        public int vendor_id { get; set; }
        public System.DateTime ChalanDate { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public string Description { get; set; }
        public Nullable<int> chalan_number { get; set; }
        public string dispatch_document_number { get; set; }
        public string dispatched_through { get; set; }
        public string bale_numbers { get; set; }
        [ForeignKey("User")]
        public Nullable<int> created_by_id { get; set; }

        [ForeignKey("FinancialYear")]
        public Nullable<int> FinancialYear_Id { get; set; }

        [ForeignKey("Company")]
        public int Company_Id { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual FinancialYear FinancialYear { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrinterChalanDetail> PrinterChalanDetails { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
