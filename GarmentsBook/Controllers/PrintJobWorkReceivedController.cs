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
using Microsoft.AspNet.Identity;

namespace GarmentSoft.Controllers
{
    [Authorize]
    public class PrintJobWorkReceivedController : Controller
    {
        private GarmentBooksEntities db = new GarmentBooksEntities();

        // GET: PrinterChalans
        public ActionResult Index(int? page)
        {
            var year_id = Convert.ToInt32(Session["FinancialYearID"]);
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var printerChalans = db.PrintJobWorkReceiveds
                .Where(x => x.FinancialYear_Id == year_id && x.Company_Id == CompanyId)
                .Include(p => p.Vendor).Include(p => p.PrintJobWorkReceivedDetails).OrderBy(x => x.ChalanDate);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(printerChalans.ToPagedList(pageNumber, pageSize));
        }

        // GET: PrinterChalans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            PrintJobWorkReceived printerChalan = db.PrintJobWorkReceiveds.Where(x => x.Company_Id == companyId && x.Id == id).FirstOrDefault();
            if (printerChalan == null)
            {
                return HttpNotFound();
            }
            return View(printerChalan);
        }

        // GET: PrinterChalans/Create
        public ActionResult Create()
        {
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == companyId), "Id", "ProductName");
            ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 2 && x.Company_Id == companyId), "Id", "VendorName");
            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");
            return View();
        }

        // POST: PrinterChalans/Create       
        [HttpPost]
        public JsonResult Create(PrintJobWorkReceived printerChalan)
        {
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var year_id = Convert.ToInt32(Session["FinancialYearID"]);
                    var creaded_by = User.Identity.GetUserId<int>();
                    DateTime dtDate = DateTime.Now;
                    printerChalan.Created = dtDate;
                    printerChalan.Updated = dtDate;
                    printerChalan.created_by_id = creaded_by;
                    printerChalan.FinancialYear_Id = year_id;
                    printerChalan.Company_Id = companyId;

                    db.PrintJobWorkReceiveds.Add(printerChalan);
                    db.SaveChanges();
                    int scope_id = printerChalan.Id;
                    transaction.Commit();
                    return Json(Convert.ToString(scope_id));
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 2 && x.Company_Id == companyId), "Id", "VendorName", printerChalan.vendor_id);
                    ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == companyId), "Id", "ProductName");
                }
            }
            return Json("0");
        }

        // GET: PrinterChalans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            PrintJobWorkReceived printerChalan = db.PrintJobWorkReceiveds.Where(x => x.Company_Id == companyId && x.Id == id).FirstOrDefault();
            if (printerChalan == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == companyId), "Id", "ProductName", printerChalan.vendor_id);
            ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 2 && x.Company_Id == companyId), "Id", "VendorName", printerChalan.vendor_id);
            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");
            return View(printerChalan);
        }

        // POST: PrinterChalans/Edit/5        
        [HttpPost]
        public JsonResult Edit(PrintJobWorkReceived printerChalan)
        {
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var objPurchaseDetails in printerChalan.PrintJobWorkReceivedDetails)
                    {
                        if (objPurchaseDetails.Id == 0)
                        {
                            db.Entry(objPurchaseDetails).State = EntityState.Added;
                            db.SaveChanges();
                        }
                        else
                            db.Entry(objPurchaseDetails).State = EntityState.Modified;
                    }

                    while (printerChalan.PrintJobWorkReceivedDetails.Where(x => x.Id == 0).Count() > 0)
                        printerChalan.PrintJobWorkReceivedDetails.Remove(printerChalan.PrintJobWorkReceivedDetails.Where(x => x.Id == 0).ToList()[0]);

                    DateTime dtDate = DateTime.Now;
                    printerChalan.Updated = dtDate;
                    db.Entry(printerChalan).State = EntityState.Modified;
                    db.SaveChanges();

                    transaction.Commit();
                    return Json(Convert.ToString(printerChalan.Id));
                }
                catch
                {
                    transaction.Rollback();
                    ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 2 && x.Company_Id == companyId), "Id", "VendorName", printerChalan.vendor_id);
                    ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == companyId), "Id", "ProductName");
                }
            }
            return Json("0");
        }

        // GET: PrinterChalans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            PrintJobWorkReceived printerChalan = db.PrintJobWorkReceiveds.Where(x => x.Company_Id == companyId && x.Id == id).FirstOrDefault();
            db.PrintJobWorkReceiveds.Remove(printerChalan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return Json("Error in deleting product.");
            }
            try
            {
                if (id != null && id != 0)
                {
                    PrintJobWorkReceivedDetail printerChalanDetail = (PrintJobWorkReceivedDetail)db.PrintJobWorkReceivedDetails.Where(x => x.Id == id).FirstOrDefault();
                    db.PrintJobWorkReceivedDetails.Remove(printerChalanDetail);
                    db.SaveChanges();
                    return Json("product deleted Successfully.");
                }
            }
            catch
            {
            }
            return Json("Error in deleting product.");
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
