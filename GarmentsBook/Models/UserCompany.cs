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

    public partial class UserCompany
    {
        public int Id { get; set; }

        [ForeignKey("Company")]
        public Nullable<int> Company_Id { get; set; }

        [ForeignKey("User")]
        public Nullable<int> UserId { get; set; }
        public Nullable<bool> is_default { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}