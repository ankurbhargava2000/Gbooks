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

    public partial class PrintJobWorkReceivedDetail
    {
        public int Id { get; set; }

        [ForeignKey("PrintJobWorkReceived")]
        public int chalan_id { get; set; }

        [ForeignKey("Product")]
        public int product_id { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<int> Fold { get; set; }
        public string Description { get; set; }
    
        public virtual PrintJobWorkReceived PrintJobWorkReceived { get; set; }
        public virtual Product Product { get; set; }
    }
}
