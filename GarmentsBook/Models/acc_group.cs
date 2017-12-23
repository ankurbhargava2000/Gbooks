

namespace GarmentSoft.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class acc_group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public acc_group()
        {
            this.acc_group1 = new HashSet<acc_group>();
            this.acc_ledger = new HashSet<acc_ledger>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        [ForeignKey("acc_group2")]
        public Nullable<int> base_group_id { get; set; }
        public string group_type { get; set; }
        public Nullable<bool> is_base_group { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<acc_group> acc_group1 { get; set; }
        public virtual acc_group acc_group2 { get; set; }

        [ForeignKey("Company")]
        public int? Company_Id { get; set; }
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<acc_ledger> acc_ledger { get; set; }
    }
}
