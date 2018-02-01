using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GarmentSoft.App_Start
{
    
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["CompanyID"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/LogOff");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
    
}