﻿using GarmentSoft.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Validation;
using Microsoft.AspNet.Identity;

namespace GarmentSoft.Controllers
{
    [Authorize]
    public class TailorChalanController : Controller
    {
        private GarmentBooksEntities db = new GarmentBooksEntities();

        // GET: TailorChalan
        public ActionResult Index(int? id, int? page)
        {
            //var tailorChalans = db.TailorChalans.Include(p => p.Vendor).Where(p => p.IsGivenToTailor == (id == 1 ? true : false)).Include(p => p.TailorChalanDetails).Include(p => p.TailorChalanDetails1).OrderBy(x => x.ChalanDate);
            var year_id = Convert.ToInt32(Session["FinancialYearID"]);
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var tailorChalans = db.TailorChalans
                .Where(x => x.FinancialYear_Id == year_id && x.Company_Id == CompanyId)
                .Include(p => p.Vendor).Include(p => p.TailorChalanDetails).OrderBy(x => x.ChalanDate);
            ViewBag.Send = id;
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
            TailorChalan tailorChalan = db.TailorChalans.Where(x => x.Company_Id == CompanyId && x.Id == id).FirstOrDefault();
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
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName");
            ViewBag.MaterialId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName");
            ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3 && x.Company_Id == CompanyId), "Id", "VendorName");
            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");
            return View();
        }

        // POST: TailorChalan/Create       
        [HttpPost]
        public JsonResult Create(TailorChalan tailorChalan)
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

                    db.TailorChalans.Add(tailorChalan);
                    db.SaveChanges();
                    int scope_id = tailorChalan.Id;
                    transaction.Commit();
                    return Json(Convert.ToString(scope_id));
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                        }
                    }
                    transaction.Rollback();
                    ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3 && x.Company_Id == CompanyId), "Id", "VendorName", tailorChalan.vendor_id);
                    ViewBag.ProductId = new SelectList(db.Products.Where(x => x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName");
                    ViewBag.MaterialId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName");
                }
            }
            return Json("0");
        }
        
        //For visible true/false material Detail For Finished Product
        [HttpPost]
        public JsonResult IsFinishedproduct(int ProdId)
        {
            string strresult = "false";
            try
            {

                var prod = db.Products.Find(ProdId);
                if (prod.ProductTypeId == 2)
                {
                    strresult = "true";
                }
                else
                {
                    strresult = "false";
                }

                return Json(strresult);
            }
            catch (Exception)
            {

                return Json(strresult);
            }

        }

        // GET: TailorChalan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            TailorChalan tailorChalan = db.TailorChalans.Where(x => x.Company_Id == CompanyId && x.Id == id).FirstOrDefault();
            if (tailorChalan == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName");
            ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3 && x.Company_Id == CompanyId), "Id", "VendorName");
            ViewBag.MaterialId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName");
            return View(tailorChalan);
        }

        // POST: TailorChalan/Edit/5        
        [HttpPost]
        public JsonResult Edit(TailorChalan tailorChalan)
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var year_id = Convert.ToInt32(Session["FinancialYearID"]);
                    var creaded_by = User.Identity.GetUserId<int>();
                    DateTime dtDate = DateTime.Now;
                    tailorChalan.Updated = dtDate;
                    tailorChalan.created_by_id = creaded_by;
                    tailorChalan.FinancialYear_Id = year_id;
                    tailorChalan.Company_Id = CompanyId;

                    foreach (var objTailorDetails in tailorChalan.TailorChalanDetails)
                    {
                        if (objTailorDetails.Id == 0)
                        {
                            db.Entry(objTailorDetails).State = EntityState.Added;
                            db.SaveChanges();
                        }
                        else
                            db.Entry(objTailorDetails).State = EntityState.Modified;
                    }

                    foreach (var objTailorDetails in tailorChalan.TailorChalanDetails)
                    {
                        foreach (var objMat in objTailorDetails.TailorMaterialDetails)
                        {
                            objMat.TailorChalanDetailsId = objTailorDetails.Id;
                            if (objMat.Id == 0)
                            {
                                db.Entry(objMat).State = EntityState.Added;
                                db.SaveChanges();
                            }
                        }
                    }

                    while (tailorChalan.TailorChalanDetails.Where(x => x.Id == 0).Count() > 0)
                        tailorChalan.TailorChalanDetails.Remove(tailorChalan.TailorChalanDetails.Where(x => x.Id == 0).ToList()[0]);

                    db.Entry(tailorChalan).State = EntityState.Modified;
                    db.SaveChanges();

                    transaction.Commit();
                    return Json(Convert.ToString(tailorChalan.Id));
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ViewBag.vendor_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3 && x.Company_Id == CompanyId), "Id", "VendorName", tailorChalan.vendor_id);
                    ViewBag.MaterialId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName");
                    ViewBag.ProductId = new SelectList(db.Products.Where(x => x.IsActive == true && x.Company_Id == CompanyId), "Id", "ProductName");
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
            TailorChalan tailorChalan = db.TailorChalans.Where(x => x.Company_Id == CompanyId && x.Id == id).FirstOrDefault();
            db.TailorChalans.Remove(tailorChalan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return Json("0");
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            try
            {
                if (id != null && id != 0)
                {
                    db.TailorMaterialDetails.RemoveRange(db.TailorMaterialDetails.Where(x => x.TailorChalanDetailsId == id));
                    db.SaveChanges();
                    TailorChalanDetail tailorChalanDetail = (TailorChalanDetail)db.TailorChalanDetails.Where(x => x.Id == id).FirstOrDefault();
                    db.TailorChalanDetails.Remove(tailorChalanDetail);
                    db.SaveChanges();
                    return Json(id.ToString());
                }
            }
            catch
            {
                return Json("0");
            }
            return Json("0");
        }

        public JsonResult DeleteProductMatarial(int? id)
        {
            if (id == null)
            {
                return Json("0");
            }
            try
            {
                if (id != null && id != 0)
                {
                    TailorMaterialDetail tailorMDetail = (TailorMaterialDetail)db.TailorMaterialDetails.Where(x => x.Id == id).FirstOrDefault();
                    db.TailorMaterialDetails.Remove(tailorMDetail);
                    db.SaveChanges();
                    return Json(id.ToString());
                }
            }
            catch
            {
                return Json("0");
            }
            return Json("0");
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
