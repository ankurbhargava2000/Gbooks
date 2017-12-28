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
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var Customers = db.Customers
                .Where(x => x.Company_Id == CompanyId)
                .ToList();
            return View(Customers);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            Customer vendor = db.Customers.Where(x => x.Company_Id == companyId && x.Id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            //ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorType1");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerName,PhoneNumber,Address,Description,IsActive,mobile,email,pan_number,gst_number,CompanyId")] Customer vendor)
        {
            if (ModelState.IsValid)
            {
                int companyId = Convert.ToInt32(Session["CompanyID"]);
                vendor.Company_Id = companyId;
                db.Customers.Add(vendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorType1", vendor.VendorTypeId);
            return View(vendor);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            Customer vendor = db.Customers.Where(x => x.Company_Id == companyId && x.Id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            //ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorType1", vendor.VendorTypeId);
            return View(vendor);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerName,PhoneNumber,Address,Description,IsActive,mobile,email,pan_number,gst_number,CompanyId")] Customer vendor)
        {
            if (ModelState.IsValid)
            {
                int companyId = Convert.ToInt32(Session["CompanyID"]);
                vendor.Company_Id = companyId;
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorType1", vendor.VendorTypeId);
            return View(vendor);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            Customer vendor = db.Customers.Where(x => x.Company_Id == companyId && x.Id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            db.Customers.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            Customer vendor = db.Customers.Where(x => x.Company_Id == companyId && x.Id == id).FirstOrDefault();
            db.Customers.Remove(vendor);
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
