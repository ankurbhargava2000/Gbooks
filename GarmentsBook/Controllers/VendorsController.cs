﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GarmentSoft.Models;
using PagedList;
using GarmentSoft.App_Start;

namespace GarmentSoft.Controllers
{
    [SessionExpire]
    [Authorize]
    public class VendorsController : Controller
    {
        private GarmentBooksEntities db = new GarmentBooksEntities();

        // GET: Vendors
        public ActionResult Index()
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var vendors = db.Vendors.Include(v => v.VendorType)
                .Where(x => x.Company_Id == CompanyId)
                .ToList();
            return View(vendors);
        }

        // GET: Vendors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            Vendor vendor = db.Vendors.Where(x => x.Company_Id == CompanyId && x.Id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: Vendors/Create
        public ActionResult Create()
        {
            ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorTypeName");
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VendorTypeId,VendorName,PhoneNumber,Address,Description,IsActive,mobile,email,pan_number,gst_number,CompanyId")] Vendor vendor)
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            if (ModelState.IsValid)
            {
                vendor.Company_Id = CompanyId;
                db.Vendors.Add(vendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorTypeName", vendor.VendorTypeId);
            return View(vendor);
        }

        // GET: Vendors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            Vendor vendor = db.Vendors.Where(x => x.Company_Id == CompanyId && x.Id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorTypeName", vendor.VendorTypeId);
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VendorTypeId,VendorName,PhoneNumber,Address,Description,IsActive,mobile,email,pan_number,gst_number,CompanyId")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                var CompanyId = Convert.ToInt32(Session["CompanyID"]);
                vendor.Company_Id = CompanyId;
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorTypeName", vendor.VendorTypeId);
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            Vendor vendor = db.Vendors.Where(x => x.Company_Id == CompanyId && x.Id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            db.Vendors.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            Vendor vendor = db.Vendors.Where(x => x.Company_Id == CompanyId && x.Id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            db.Vendors.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index");
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
