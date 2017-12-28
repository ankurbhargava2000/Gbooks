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

    public partial class InvoiceDetail
    {
        public int Id { get; set; }

        [ForeignKey("InvoiceMaster")]
        public Nullable<int> invoice_id { get; set; }

        [ForeignKey("Product")]
        public Nullable<int> product_id { get; set; }
        public double sale_rate { get; set; }
        public int quantity { get; set; }
        public double discount { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
    
        public virtual InvoiceMaster InvoiceMaster { get; set; }
        public virtual Product Product { get; set; }
    }
}