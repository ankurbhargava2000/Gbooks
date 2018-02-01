using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using GarmentSoft.Models;
using GarmentSoft.App_Start;

namespace GarmentSoft.Controllers
{
    [SessionExpire]
    public class UserProfileController : Controller
    {
        private GarmentBooksEntities db = new GarmentBooksEntities();
        private ApplicationUserManager _userManager;

        //public UserProfileController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //{
        //    UserManager = userManager;
        //}


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        public ActionResult Edit()
        {

            if (Session["UserId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspnetuser = db.AspNetUsers.Find(Session["UserId"]);
            ChangeProfileViewModel changeProfile = new ChangeProfileViewModel();
            if (aspnetuser == null)
            {
                return HttpNotFound();
            }
            changeProfile.UserProfile = UserProfile(aspnetuser);
            changeProfile.ChangePassword = new ChangePasswordViewModel
            {
                OldPassword = "",
                NewPassword = "",
                ConfirmPassword = ""
            };
            if (aspnetuser.Country == null)
            {
                ViewBag.Country = "India";
            }
            else
            {
                ViewBag.Country = aspnetuser.Country;
            }
            return View(changeProfile);
        }

        
        public UserProfileViewModel UserProfile(AspNetUser aspnetuser)
        {

            UserProfileViewModel bModel = new UserProfileViewModel()
            {
                Id = aspnetuser.Id,
                Email = aspnetuser.Email,
                PhoneNumber = aspnetuser.PhoneNumber,
                City = aspnetuser.City,
                State = aspnetuser.State,
                PIN = aspnetuser.PIN,
                Address = aspnetuser.Address
            };
            return bModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string btnSubmit, string hdnCountry, ChangeProfileViewModel aspnetuser)
        {
            try
            {
                var UserProfile = db.AspNetUsers.FirstOrDefault(x => x.Id == aspnetuser.UserProfile.Id);
                if (UserProfile == null)
                {
                    return HttpNotFound();
                }

                if (btnSubmit == "Change Password")
                {
                    IdentityResult result = UserManager.ChangePassword(aspnetuser.UserProfile.Id, aspnetuser.ChangePassword.OldPassword.ToString(), aspnetuser.ChangePassword.NewPassword.ToString());
                    if (result.Succeeded )
                    {
                        ViewBag.msg = "Password Changed Successfully.";
                        aspnetuser.ChangePassword = new ChangePasswordViewModel
                        {
                            OldPassword = "",
                            NewPassword = "",
                            ConfirmPassword = ""
                        };
                    }
                    else
                    {
                        ViewBag.msg = ((string[])(result.Errors))[0];
                    }                 
                 }
                else
                {
                    UserProfile.PhoneNumber = aspnetuser.UserProfile.PhoneNumber;
                    UserProfile.Email = aspnetuser.UserProfile.Email;
                    UserProfile.City = aspnetuser.UserProfile.City;
                    UserProfile.State = aspnetuser.UserProfile.State;
                    UserProfile.Country = hdnCountry;
                    UserProfile.PIN = aspnetuser.UserProfile.PIN;
                    UserProfile.Address = aspnetuser.UserProfile.Address;
                    db.SaveChanges();
                    ViewBag.msg = "Profile Updated Successfully.";
                }
                ViewBag.Country = hdnCountry;
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
