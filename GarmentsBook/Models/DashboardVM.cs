﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarmentSoft.Models
{
    public class DashboardVM
    {
        public decimal? totalPurchases { get; set; }
        public double totalSales { get; set; }
        public double totalExpenses { get; set; }
        public List<TopModel> topProducts { get; set; }
        public List<TopModel> topCustomers { get; set; }
        public List<TopModel> topVendors { get; set; }
        public List<PurchaseOrder> latestPurchase { get; set; }
        public ChartModel SalesAmount { get; set; }
        public ChartModel SalesCount { get; set; }

    }

    public class TopModel
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Count { get; set; }
    }

    public class ChartModel
    {
        public DateTime? Label { get; set; }
        public string Data { get; set; }
        public string LabelJson { get; set; }
        public string PointJson { get; set; }

    }
    
}