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
    
    public partial class USP_VendorStockLedger_Result
    {
        public Nullable<int> SequenceNo { get; set; }
        public Nullable<int> product_id { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string DocType { get; set; }
        public string DocNo { get; set; }
        public Nullable<int> DocId { get; set; }
        public Nullable<decimal> InQTY { get; set; }
        public Nullable<decimal> OutQTY { get; set; }
    }
}
