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
    public class UserProfileController : Controller
    {
        private GarmentBooksEntities db = new GarmentBooksEntities();
       
        public ActionResult Edit()
        {

            if (Session["UserId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspnetuser = db.AspNetUsers.Find(Session["UserId"]);
            if (aspnetuser == null)
            {
                return HttpNotFound();
            }
            if(aspnetuser.Country == null)
            {
                ViewBag.Country = "India";
            }
            else
            {
                ViewBag.Country = aspnetuser.Country;
            }
            return View(aspnetuser);
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection FrmColl, string hdnCountry, [Bind(Include = "Id,PhoneNumber,Email,City,State,Country,PIN,Address")] AspNetUser aspnetuser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UserProfile = db.AspNetUsers.FirstOrDefault(x => x.Id == aspnetuser.Id);
                    if (UserProfile == null)
                    {
                        return HttpNotFound();
                    }

                    UserProfile.PhoneNumber = aspnetuser.PhoneNumber;
                    UserProfile.Email = aspnetuser.Email;
                    UserProfile.City = aspnetuser.City;
                    UserProfile.State = aspnetuser.State;
                    UserProfile.Country = hdnCountry;
                    UserProfile.PIN = aspnetuser.PIN;
                    UserProfile.Address = aspnetuser.Address;
                    db.SaveChanges();
                }
                ViewBag.Country = hdnCountry;
                ViewBag.msg="Profile Updated Successfully.";
                return View(aspnetuser);
            }
            catch (Exception)
            {
                
               ViewBag.Country = hdnCountry;
               ViewBag.msg = "Problem in Updating Profile.";
                return View(aspnetuser);
            }
            
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
