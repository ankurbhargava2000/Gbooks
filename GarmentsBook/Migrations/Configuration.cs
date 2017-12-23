namespace GarmentSoft.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GarmentSoft.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GarmentSoft.Models.ApplicationDbContext context)
        {
            context.Tenants.AddOrUpdate(x => x.Id,
            new Models.Tenant() { Id = 1, Name = "Saksham Software" }
            );

            context.FinancialYears.AddOrUpdate(x => x.Id,
            new Models.FinancialYear() { Id = 1, Name = "2017-2018", StartDate= DateTime.ParseExact("01/04/2017", "dd/MM/yyyy", null), EndDate= DateTime.ParseExact("31/03/2018", "dd/MM/yyyy", null) }
            );

            context.Companies.AddOrUpdate(x => x.Id,
            new Models.Company() { Id = 1, Name = "Saksham Software",TenantId=1,currentFinYear_id=1 }
            );

            
            context.MeasuringUnits.AddOrUpdate(x => x.Id,
            new Models.MeasuringUnit() { Id = 1, Name = "Meter"},
            new Models.MeasuringUnit() { Id = 2, Name = "Pcs" }
            );

            context.VendorTypes.AddOrUpdate(x => x.Id,
            new Models.VendorType() { Id = 1, VendorTypeName = "Raw Matarial Vendor", Description = "Who supplies raw material to company" },
            new Models.VendorType() { Id = 2, VendorTypeName = "Printer", Description = "Who do the color processing of cloths" },
            new Models.VendorType() { Id = 3, VendorTypeName = "Fabricator", Description = "Who create product from cloths" }
            );

            context.ProductTypes.AddOrUpdate(x => x.Id,
            new Models.ProductType() { Id = 1, ProductTypeName = "Raw Matarial", Description = "", IsActive = true },
            new Models.ProductType() { Id = 2, ProductTypeName = "Finished Product", Description = "",IsActive=true }
            );

            context.acc_groups.AddOrUpdate(x => x.id,
            new Models.acc_group() { id = 1, name = "Branch / Divisions", base_group_id = null, is_base_group = true },
            new Models.acc_group() { id = 2, name = "Capital Account", base_group_id = null, is_base_group = true },
            new Models.acc_group() { id = 3, name = "Reserves & Surplus", base_group_id = 2, is_base_group = true },
            new Models.acc_group() { id = 4, name = "Current Assets", base_group_id = null, is_base_group = true },
            new Models.acc_group() { id = 5, name = "Bank Accounts", base_group_id = 4, is_base_group = true },
            new Models.acc_group() { id = 6, name = "Cash-in-hand", base_group_id = 4, is_base_group = true },
            new Models.acc_group() { id = 7, name = "Deposits (Asset)", base_group_id = 4, is_base_group = true },
            new Models.acc_group() { id = 8, name = "Loans & Advances (Asset)", base_group_id = 4, is_base_group = true },
            new Models.acc_group() { id = 9, name = "Stock-in-hand", base_group_id = 4, is_base_group = true },
            new Models.acc_group() { id = 10, name = "Sundry Debtors", base_group_id = 4, is_base_group = true },
            new Models.acc_group() { id = 11, name = "Current Liabilities", base_group_id = null, is_base_group = true },
            new Models.acc_group() { id = 12, name = "Duties & Taxes", base_group_id = 11, is_base_group = true },
            new Models.acc_group() { id = 13, name = "Provisions", base_group_id = 11, is_base_group = true },
            new Models.acc_group() { id = 14, name = "Sundry Creditors", base_group_id = 11, is_base_group = true },
            new Models.acc_group() { id = 15, name = "Direct Expenses", base_group_id = null, is_base_group = true },
            new Models.acc_group() { id = 16, name = "Direct Incomes", base_group_id = null, is_base_group = true },
            new Models.acc_group() { id = 17, name = "Fixed Assets", base_group_id = null, is_base_group = true },
            new Models.acc_group() { id = 18, name = "Indirect Expenses", base_group_id = null, is_base_group = true },
            new Models.acc_group() { id = 19, name = "Indirect Incomes", base_group_id = null, is_base_group = true },
            new Models.acc_group() { id = 20, name = "Investments", base_group_id = null, is_base_group = true },
            new Models.acc_group() { id = 21, name = "Loans (Liability)", base_group_id = null, is_base_group = true },
            new Models.acc_group() { id = 22, name = "Bank OD A/c", base_group_id = 21, is_base_group = true },
            new Models.acc_group() { id = 23, name = "Secured Loans", base_group_id = 21, is_base_group = true },
            new Models.acc_group() { id = 24, name = "Unsecured Loans", base_group_id = 21, is_base_group = true },
            new Models.acc_group() { id = 25, name = "Misc. Expenses (Asset)", base_group_id = 21, is_base_group = true },
            new Models.acc_group() { id = 26, name = "Purchase Accounts", base_group_id = 21, is_base_group = true },
            new Models.acc_group() { id = 27, name = "Sales Accounts", base_group_id = 21, is_base_group = true },
            new Models.acc_group() { id = 28, name = "Suspense A/c", base_group_id = 21, is_base_group = true }
            );

            
        }
    }
}
