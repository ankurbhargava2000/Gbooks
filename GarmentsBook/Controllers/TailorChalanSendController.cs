using GarmentSoft.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;

namespace GarmentSoft.Controllers
{
    [Authorize]
    public class TailorChalanSendController : Controller
    {
        private GarmentBooksEntities db = new GarmentBooksEntities();

        // GET: TailorChalan
        public ActionResult Index(int? page)
        {
            var year_id = Convert.ToInt32(Session["FinancialYearID"]);
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var tailorChalans = db.TailorChalanSends
                .Where(x => x.FinancialYear_Id == year_id && x.Company_Id == CompanyId)
                .Include(p => p.Vendor)
                .Include(p => p.TailorChalanSendDetails).OrderBy(x => x.ChalanDate);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(tailorChalans.ToPagedList(pageNumber, pageSize));
        }

        // GET: TailorChalan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            TailorChalanSend tailorChalan = db.TailorChalanSends.Where(x => x.Company_Id == CompanyId && x.Id == id).FirstOrDefault();
            if (tailorChalan == null)
            {
                return HttpNotFound();
            }
            return View(tailorChalan);
        }

        // GET: TailorChalan/Create
        public ActionResult Create()
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName");
            ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3 && x.Company_Id == CompanyId), "Id", "VendorName");

            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");

            var last = db.TailorChalanSends.OrderByDescending(o => o.ChalanNo).FirstOrDefault();

            if (last == null)
            {
                ViewBag.ChalanNo = 1;
            }
            else
            {
                ViewBag.ChalanNo = last.ChalanNo + 1;
            }
            return View();
        }

        // POST: TailorChalan/Create       
        [HttpPost]
        public JsonResult Create(TailorChalanSend tailorChalan)
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var year_id = Convert.ToInt32(Session["FinancialYearID"]);

                    var creaded_by = User.Identity.GetUserId<int>();
                    DateTime dtDate = DateTime.Now;
                    tailorChalan.Created = dtDate;
                    tailorChalan.Updated = dtDate;
                    tailorChalan.created_by_id = creaded_by;
                    tailorChalan.FinancialYear_Id = year_id;
                    tailorChalan.Company_Id = CompanyId;

                    db.TailorChalanSends.Add(tailorChalan);
                    db.SaveChanges();
                    int scope_id = tailorChalan.Id;
                    transaction.Commit();
                    return Json(Convert.ToString(scope_id));
                }
                catch
                {
                    transaction.Rollback();
                    ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3 && x.Company_Id == CompanyId), "Id", "VendorName", tailorChalan.vendor_id);
                    ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName");
                }
            }
            return Json("0");
        }

        public JsonResult GetTransactionBuyingRate(int? ProdId)
        {
            //var currFrom = db.Currencys.Where(u = > u.CurrencyId == eventIdB).First();
            return Json(Convert.ToString("true") , JsonRequestBehavior.AllowGet);
        }


        // GET: TailorChalan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            TailorChalanSend tailorChalan = db.TailorChalanSends.Where(x => x.Company_Id == CompanyId && x.Id == id).FirstOrDefault();
            if (tailorChalan == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName", tailorChalan.vendor_id);
            ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3 && x.Company_Id == CompanyId), "Id", "VendorName", tailorChalan.vendor_id);
            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");
            return View(tailorChalan);
        }

        // POST: TailorChalan/Edit/5        
        [HttpPost]
        public JsonResult Edit(TailorChalanSend tailorChalan)
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    foreach (var objTailorDetails in tailorChalan.TailorChalanSendDetails)
                    {
                        if (objTailorDetails.Id == 0)
                        {
                            db.Entry(objTailorDetails).State = EntityState.Added;
                            db.SaveChanges();
                        }
                        else
                            db.Entry(objTailorDetails).State = EntityState.Modified;
                    }

                    while (tailorChalan.TailorChalanSendDetails.Where(x => x.Id == 0).Count() > 0)
                        tailorChalan.TailorChalanSendDetails.Remove(tailorChalan.TailorChalanSendDetails.Where(x => x.Id == 0).ToList()[0]);


                    DateTime dtDate = DateTime.Now;
                    tailorChalan.Updated = dtDate;
                    db.Entry(tailorChalan).State = EntityState.Modified;
                    db.SaveChanges();

                    transaction.Commit();
                    return Json(Convert.ToString(tailorChalan.Id));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3 && x.Company_Id == CompanyId), "Id", "VendorName", tailorChalan.vendor_id);
                    ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName");
                }
            }
            return Json("0");
        }

        // GET: TailorChalan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            TailorChalanSend tailorChalan = db.TailorChalanSends.Where(x => x.Company_Id == CompanyId && x.Id == id).FirstOrDefault();
            db.TailorChalanSends.Remove(tailorChalan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return Json("Error in deleting product.");
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            try
            {
                if (id != null && id != 0)
                {
                    TailorChalanSendDetail tailorChalanDetail = (TailorChalanSendDetail)db.TailorChalanSendDetails.Where(x => x.Id == id).FirstOrDefault();
                    db.TailorChalanSendDetails.Remove(tailorChalanDetail);
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
