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
    public partial class acc_ledger
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        [ForeignKey("Company")]
        public int Company_Id { get; set; }

        [ForeignKey("acc_group")]
        public int group_id { get; set; }
        public virtual acc_group acc_group { get; set; }
        public virtual Company Company { get; set; }
    }
}