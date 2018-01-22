using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GarmentSoft.Models;

namespace GarmentSoft.Controllers
{
    public class JournalController : Controller
    {
        private GarmentBooksEntities db = new GarmentBooksEntities();

        // GET: /Journal/
        public ActionResult Index()
        {
            var acc_transactions = db.acc_transactions.Include(a => a.Company).Include(a => a.FinancialYear);
            return View(acc_transactions.ToList());
        }

        // GET: /Journal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            acc_transactions acc_transactions = db.acc_transactions.Find(id);
            if (acc_transactions == null)
            {
                return HttpNotFound();
            }
            return View(acc_transactions);
        }

        // GET: /Journal/Create
        public ActionResult Create()
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            ViewBag.Company_Id = new SelectList(db.Companies, "Id", "Name");
            ViewBag.FinancialYear_Id = new SelectList(db.FinancialYears, "Id", "Name");
            ViewBag.voucher_type = new SelectList(db.Customers.Where(x => x.Company_Id == CompanyId), "Id", "CustomerName");
           ViewBag.acc_ledger_id = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3 && x.Company_Id == CompanyId), "Id", "VendorName");           
            return View();
        }

        // POST: /Journal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,voucher_type,voucher_no,voucher_date,description,Company_Id,FinancialYear_Id")] acc_transactions acc_transactions)
        {
            if (ModelState.IsValid)
            {
                db.acc_transactions.Add(acc_transactions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Company_Id = new SelectList(db.Companies, "Id", "Name", acc_transactions.Company_Id);
            ViewBag.FinancialYear_Id = new SelectList(db.FinancialYears, "Id", "Name", acc_transactions.FinancialYear_Id);
            return View(acc_transactions);
        }

        // GET: /Journal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            acc_transactions acc_transactions = db.acc_transactions.Find(id);
            if (acc_transactions == null)
            {
                return HttpNotFound();
            }
            ViewBag.Company_Id = new SelectList(db.Companies, "Id", "Name", acc_transactions.Company_Id);
            ViewBag.FinancialYear_Id = new SelectList(db.FinancialYears, "Id", "Name", acc_transactions.FinancialYear_Id);
            return View(acc_transactions);
        }

        // POST: /Journal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,voucher_type,voucher_no,voucher_date,description,Company_Id,FinancialYear_Id")] acc_transactions acc_transactions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acc_transactions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Company_Id = new SelectList(db.Companies, "Id", "Name", acc_transactions.Company_Id);
            ViewBag.FinancialYear_Id = new SelectList(db.FinancialYears, "Id", "Name", acc_transactions.FinancialYear_Id);
            return View(acc_transactions);
        }

        // GET: /Journal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            acc_transactions acc_transactions = db.acc_transactions.Find(id);
            if (acc_transactions == null)
            {
                return HttpNotFound();
            }
            return View(acc_transactions);
        }

        // POST: /Journal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            acc_transactions acc_transactions = db.acc_transactions.Find(id);
            db.acc_transactions.Remove(acc_transactions);
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
