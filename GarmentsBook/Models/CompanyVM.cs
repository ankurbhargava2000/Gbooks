using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarmentSoft.Models
{
    public class AddUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool AddToCompany { get; set; }
        public bool IsDefault { get; set; }
    }

    public class CompanyVM
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? YearId { get; set; }
        public bool? IsDefault { get; set; }
    }
}