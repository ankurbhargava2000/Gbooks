using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarmentSoft.Models
{
    public class OpeningStockVM
    {
        public List<Transaction> Transactions = new List<Transaction>();
        public List<Product> Products { get; set; }
        public FinancialYear FinancialYear { get; set; }

    }
}