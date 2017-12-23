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
    public class AccLedgerController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: accGroup
        public ActionResult Index()
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var accGroup = db.acc_ledgers
                .Where(x => x.Company_Id == CompanyId)
                .ToList();
            return View(accGroup);
        }

        // GET: accGroup/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            acc_ledger vendor = db.acc_ledgers.Where(x => x.Company_Id == companyId && x.id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: accGroup/Create
        public ActionResult Create()
        {
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            ViewBag.group_id = new SelectList(db.acc_groups.Where(x => x.is_base_group == true || x.Company_Id== companyId), "id", "name");
            return View();
        }

        // POST: accGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,description,group_id")] acc_ledger vendor)
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            if (ModelState.IsValid)
            {
                db.acc_ledgers.Add(vendor);
                vendor.Company_Id = CompanyId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.group_id = new SelectList(db.acc_groups.Where(x => x.is_base_group == true || x.Company_Id == CompanyId), "id", "name");
            return View(vendor);
        }

        // GET: accGroup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            acc_ledger vendor = db.acc_ledgers.Where(x => x.Company_Id == companyId && x.id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            ViewBag.group_id = new SelectList(db.acc_groups.Where(x => x.is_base_group == true || x.Company_Id == companyId), "id", "name",vendor.group_id);
            return View(vendor);
        }

        // POST: accGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,description,group_id")] acc_ledger vendor)
        {
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                vendor.Company_Id = companyId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.group_id = new SelectList(db.acc_groups.Where(x => x.is_base_group == true || x.Company_Id == companyId), "id", "name");
            return View(vendor);
        }

        // GET: accGroup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            acc_ledger vendor = db.acc_ledgers.Where(x => x.Company_Id == companyId && x.id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            db.acc_ledgers.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: accGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            acc_ledger vendor = db.acc_ledgers.Where(x => x.Company_Id == companyId && x.id == id).FirstOrDefault();
            db.acc_ledgers.Remove(vendor);
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
