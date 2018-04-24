using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXProjectWeb.App_Start;

namespace WXProjectWeb.Controllers
{
    public class CMSController : Controller
    {
        public ActionResult Login()
        {
            ViewBag.HasCookies = Request.Cookies["user"] != null ;
            return View();
        }
        
        public JsonResult PostLogin(string username, string password, string remeber)
        {
            if (username == "admin" && password == "admin")
            {
                Response.Cookies.Add(new HttpCookie("user", "admin"));
            }           
            return Json(new
            {
                success = true
            },JsonRequestBehavior.AllowGet);
        }

        [AuthorizeFilterAttribute]
        public ActionResult Index()
        {
            ViewBag.Tab = "button";
            return View();
        }
    }
}