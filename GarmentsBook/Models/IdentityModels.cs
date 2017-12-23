using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GarmentSoft.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole,CustomUserClaim>
    {
        public int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole,int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public virtual DbSet<acc_group> acc_groups { get; set; }
        public virtual DbSet<acc_ledger> acc_ledgers { get; set; }
        public virtual DbSet<acc_transactions> acc_transactions { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<FinancialYear> FinancialYears { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual DbSet<InvoiceMaster> InvoiceMasters { get; set; }
        public virtual DbSet<MeasuringUnit> MeasuringUnits { get; set; }
        public virtual DbSet<PrinterChalan> PrinterChalans { get; set; }
        public virtual DbSet<PrinterChalanDetail> PrinterChalanDetails { get; set; }
        public virtual DbSet<PrintJobWorkReceived> PrintJobWorkReceiveds { get; set; }
        public virtual DbSet<PrintJobWorkReceivedDetail> PrintJobWorkReceivedDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<TailorChalan> TailorChalans { get; set; }
        public virtual DbSet<TailorChalanDetail> TailorChalanDetails { get; set; }
        public virtual DbSet<TailorChalanSend> TailorChalanSends { get; set; }
        public virtual DbSet<TailorChalanSendDetail> TailorChalanSendDetails { get; set; }
        public virtual DbSet<TailorMaterialDetail> TailorMaterialDetails { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<UserCompany> UserCompanies  { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<VendorType> VendorTypes { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, GarmentSoft.Migrations.Configuration>("DefaultConnection"));
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<InvoiceMaster>()
            .HasRequired(t => t.Customer)
            .WithMany(t => t.InvoiceMasters)
            .HasForeignKey(d => d.customer_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseOrder>()
            .HasRequired(t => t.Vendor)
            .WithMany(t => t.PurchaseOrders)
            .HasForeignKey(d => d.vendor_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<PrinterChalan>()
            .HasRequired(t => t.Vendor)
            .WithMany(t => t.PrinterChalans)
            .HasForeignKey(d => d.vendor_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<PrintJobWorkReceived>()
            .HasRequired(t => t.Vendor)
            .WithMany(t => t.PrintJobWorkReceiveds)
            .HasForeignKey(d => d.vendor_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<TailorChalan>()
            .HasRequired(t => t.Vendor)
            .WithMany(t => t.TailorChalans)
            .HasForeignKey(d => d.vendor_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<TailorChalanSend>()
            .HasRequired(t => t.Vendor)
            .WithMany(t => t.TailorChalanSends)
            .HasForeignKey(d => d.vendor_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<PrinterChalanDetail>()
            .HasRequired(t => t.Product)
            .WithMany(t => t.PrinterChalanDetails)
            .HasForeignKey(d => d.product_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<PrintJobWorkReceivedDetail>()
            .HasRequired(t => t.Product)
            .WithMany(t => t.PrintJobWorkReceivedDetails)
            .HasForeignKey(d => d.product_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<TailorChalanDetail>()
            .HasRequired(t => t.Product)
            .WithMany(t => t.TailorChalanDetails)
            .HasForeignKey(d => d.product_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<TailorChalanSendDetail>()
            .HasRequired(t => t.Product)
            .WithMany(t => t.TailorChalanSendDetails)
            .HasForeignKey(d => d.product_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<TailorMaterialDetail>()
            .HasRequired(t => t.Product)
            .WithMany(t => t.TailorMaterialDetails)
            .HasForeignKey(d => d.product_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceDetail>()
            .HasRequired(t => t.Product)
            .WithMany(t => t.InvoiceDetails)
            .HasForeignKey(d => d.product_id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
            .HasRequired(t => t.Company)
            .WithMany(t => t.Products)
            .HasForeignKey(d => d.Company_Id)
            .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);

            
        }
    }
}