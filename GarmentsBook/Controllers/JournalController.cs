using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GarmentSoft.Models;
using Microsoft.AspNet.Identity;

namespace GarmentSoft.Controllers
{
    public class JournalController : Controller
    {
        private GarmentBooksEntities db = new GarmentBooksEntities();

        // GET: /Journal/
        public ActionResult Index()
        {
            var year_id = Convert.ToInt32(Session["FinancialYearID"]);
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var acc_transactions = db.acc_transactions
                .Where(x => x.Company_Id == CompanyId && x.FinancialYear_Id == year_id)
                .Include(p => p.VoucherType).OrderBy(x => x.voucher_date ); ;
            return View(acc_transactions.ToList());
        }

        // GET: /Journal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            acc_transactions acc_transactions = db.acc_transactions.Where(x => x.Company_Id == CompanyId && x.id == id).FirstOrDefault();
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
            ViewBag.voucher_type = new SelectList(db.VoucherTypes, "Id", "VoucherName");
            ViewBag.acc_ledger_id = new SelectList(db.acc_ledger.Where(x => x.Company_Id == CompanyId), "Id", "name");           
            return View();
        }

        // POST: /Journal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public JsonResult Create(acc_transactions acc_transactions)
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var year_id = Convert.ToInt32(Session["FinancialYearID"]);

                    var creaded_by = User.Identity.GetUserId<int>();
                    DateTime dtDate = DateTime.Now;
                    acc_transactions.FinancialYear_Id = year_id;
                    acc_transactions.Company_Id = CompanyId;

                    db.acc_transactions.Add(acc_transactions);
                    db.SaveChanges();
                    int scope_id = acc_transactions.id;
                    transaction.Commit();
                    return Json(Convert.ToString(scope_id));
                }
                catch (Exception e)
                {
                    
                    transaction.Rollback();
                    ViewBag.FinancialYear_Id = new SelectList(db.FinancialYears, "Id", "Name");
                    ViewBag.voucher_type = new SelectList(db.VoucherTypes, "Id", "VoucherName");
                    ViewBag.acc_ledger_id = new SelectList(db.acc_ledger.Where(x => x.Company_Id == CompanyId), "Id", "name");           
                }
            }
            return Json("0");
        }

        // GET: /Journal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            acc_transactions acc_transactions = db.acc_transactions.Where(x => x.Company_Id == CompanyId && x.id == id).FirstOrDefault();
            if (acc_transactions == null)
            {
                return HttpNotFound();
            }
            ViewBag.Company_Id = new SelectList(db.Companies, "Id", "Name");
            ViewBag.FinancialYear_Id = new SelectList(db.FinancialYears, "Id", "Name");
            ViewBag.voucher_type = new SelectList(db.VoucherTypes, "Id", "VoucherName");
            ViewBag.voucherDate = acc_transactions.voucher_date;
            ViewBag.acc_ledger_id = new SelectList(db.acc_ledger.Where(x => x.Company_Id == CompanyId), "Id", "name");           
            return View(acc_transactions);
        }

        // POST: /Journal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public JsonResult Edit(acc_transactions acc_transactions)
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var year_id = Convert.ToInt32(Session["FinancialYearID"]);
                    var creaded_by = User.Identity.GetUserId<int>();
                    DateTime dtDate = DateTime.Now;
                    acc_transactions.FinancialYear_Id = year_id;
                    acc_transactions.Company_Id = CompanyId;

                    foreach (var objdtl in acc_transactions.acc_transactions_details )
                    {
                        if (objdtl.id == 0)
                        {
                            db.Entry(objdtl).State = EntityState.Added;
                            db.SaveChanges();
                        }
                        else
                            db.Entry(objdtl).State = EntityState.Modified;
                    }

                    

                    while (acc_transactions.acc_transactions_details.Where(x => x.id == 0).Count() > 0)
                        acc_transactions.acc_transactions_details.Remove(acc_transactions.acc_transactions_details.Where(x => x.id == 0).ToList()[0]);

                    db.Entry(acc_transactions).State = EntityState.Modified;
                    db.SaveChanges();

                    transaction.Commit();
                    return Json(Convert.ToString(acc_transactions.id));
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ViewBag.FinancialYear_Id = new SelectList(db.FinancialYears, "Id", "Name");
                    ViewBag.voucher_type = new SelectList(db.VoucherTypes, "Id", "VoucherName");
                    ViewBag.acc_ledger_id = new SelectList(db.acc_ledger.Where(x => x.Company_Id == CompanyId), "Id", "name");           
                }
            }
            return Json("0");
        }

        [HttpPost]
        public JsonResult DeleteItem(int? id)
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

                    acc_transactions_details acc_trnsdtl = (acc_transactions_details)db.acc_transactions_details.Where(x => x.id == id).FirstOrDefault();
                    db.acc_transactions_details.Remove(acc_trnsdtl);
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
 
        
        public ActionResult DeleteConfirmed(int id)
        {
            acc_transactions acc_transactions = db.acc_transactions.Find(id);

            if (acc_transactions == null)
            {
                return HttpNotFound();
            }

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
