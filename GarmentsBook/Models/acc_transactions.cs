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
    
    public partial class acc_transactions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public acc_transactions()
        {
            this.acc_transactions_details = new HashSet<acc_transactions_details>();
        }
    
        public int id { get; set; }
        public int voucher_type { get; set; }
        public Nullable<int> voucher_no { get; set; }
        public System.DateTime voucher_date { get; set; }
        public string description { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public Nullable<int> FinancialYear_Id { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual FinancialYear FinancialYear { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<acc_transactions_details> acc_transactions_details { get; set; }
    }
}
