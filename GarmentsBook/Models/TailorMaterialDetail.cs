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
    
    public partial class TailorMaterialDetail
    {
        public int Id { get; set; }
        public int TailorChalanDetailsId { get; set; }
        public int product_id { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string Description { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual TailorChalanDetail TailorChalanDetail { get; set; }
    }
}
