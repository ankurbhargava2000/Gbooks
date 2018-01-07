using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GarmentSoft.Models;

namespace GarmentSoft.Controllers
{
    [Authorize]
    public class SharedController : Controller
    {
        // GarmentBooksEntities db = new GarmentBooksEntities();
        [HttpPost]
        public ActionResult ChnageCompany(string CompanyId)
        {
            if (string.IsNullOrEmpty(CompanyId))
            {
                return Json(new { success = false, responseText = "Please select a company" }, JsonRequestBehavior.AllowGet);
            }
            var companyList = Session["companyListAssociatedWithUser"] as List<CompanyVM>;
            if (companyList == null)
            {
                return Json(new { success = false, responseText = "No Company Found." }, JsonRequestBehavior.AllowGet);
            }
            int Id = Convert.ToInt32(CompanyId);
            var company = companyList.Find(x => x.CompanyId == Id);

            //(from companyVm in companyList select company).ToList().ForEach(x => x.IsDefault = false);

            foreach (var companyObj in companyList)
            {
                companyObj.IsDefault = false;
            }

            companyList.Remove(company);
            company.IsDefault = true;
            companyList.Add(company);

            Session["companyListAssociatedWithUser"] = companyList;
            Session["CompanyName"] = company.CompanyName;
            Session["CompanyID"] = company.CompanyId;
            Session["FinancialYearID"] = company.YearId;
            return Json(new { success = true, responseText = "Default Company Chnaged successfully." }, JsonRequestBehavior.AllowGet);
        }

    }
}