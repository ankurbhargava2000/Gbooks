//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SampleWebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PrintJobWorkReceived
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PrintJobWorkReceived()
        {
            this.PrintJobWorkReceivedDetails = new HashSet<PrintJobWorkReceivedDetail>();
        }
    
        public int Id { get; set; }
        public int vendor_id { get; set; }
        public System.DateTime ChalanDate { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public string Description { get; set; }
        public int chalan_number { get; set; }
        public Nullable<decimal> GrossCharges { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> NetCharges { get; set; }
        public Nullable<int> created_by_id { get; set; }
        public Nullable<int> FinancialYear_Id { get; set; }
        public int Company_Id { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Company Company { get; set; }
        public virtual FinancialYear FinancialYear { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrintJobWorkReceivedDetail> PrintJobWorkReceivedDetails { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
