using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GarmentSoft.Models;
using PagedList;

namespace GarmentSoft.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index(int? page)
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var products = db.Products.Include(p => p.ProductType).Include(p => p.MeasuringUnit)
                .Where(x => x.Company_Id == CompanyId)
                .ToList();
            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            Product product = db.Products.Where(x => x.Company_Id == companyId && x.Id == id).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1");
            ViewBag.unit_id = new SelectList(db.MeasuringUnits, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            ViewBag.unit_id = new SelectList(db.MeasuringUnits, "Id", "Name", product.unit_id);

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            Product product = db.Products.Where(x => x.Company_Id == companyId && x.Id == id).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            ViewBag.unit_id = new SelectList(db.MeasuringUnits, "Id", "Name", product.unit_id);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            ViewBag.unit_id = new SelectList(db.MeasuringUnits, "Id", "Name", product.unit_id);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            Product product = db.Products.Where(x => x.Company_Id == companyId && x.Id == id).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            Product product = db.Products.Where(x => x.Company_Id == companyId && x.Id == id).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateOpeningStock()
        {
            var model = new OpeningStockVM();
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            model.Products = db.Products
                                .Where(x => x.IsActive == true && x.Company_Id == companyId)
                                .ToList();

            var year_id = Convert.ToInt32(Session["FinancialYearID"]);
            model.FinancialYear = db.FinancialYears
                .Where(x => x.Id == year_id)
                .FirstOrDefault();

            model.Transactions = db.Transactions
                .Where(x => x.FinancialYear_Id == year_id)
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOpeningStock(List<Transaction> Transactions)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var CompanyId = Convert.ToInt32(Session["CompanyID"]);
                    var year_id = Convert.ToInt32(Session["FinancialYearID"]);

                    var t = db.Transactions
                    .Where(x => x.FinancialYear_Id == year_id)
                    .ToList();

                    foreach (var item in Transactions)
                    {

                        var product = t
                        .Where(x => x.product_id == item.product_id)
                        .Where(x => x.FinancialYear_Id == year_id)
                        .FirstOrDefault();

                        if (product == null)
                        {
                            db.Transactions.Add(new Transaction()
                            {
                                product_id = item.product_id,
                                Type = "opening stock",
                                FinancialYear_Id = year_id,
                                Company_Id = CompanyId,
                                Created = DateTime.Now,
                                Updated = DateTime.Now,
                                Quantity = item.Quantity
                            });
                        }
                        else
                        {
                            product.Quantity = item.Quantity;
                            product.Updated = DateTime.Now;
                            db.Entry(product).State = EntityState.Modified;
                        }

                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch
                {

                }
            }

            var model = new OpeningStockVM();
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            model.Products = db.Products
                                .Where(x => x.IsActive == true && x.Company_Id == companyId)
                                .ToList();

            var yid = Convert.ToInt32(Session["FinancialYearID"]);
            model.FinancialYear = db.FinancialYears
                .Where(x => x.Id == yid)
                .FirstOrDefault();

            model.Transactions = db.Transactions
                .Where(x => x.FinancialYear_Id == yid)
                .ToList();

            return View(model);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
