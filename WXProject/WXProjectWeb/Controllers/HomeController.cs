﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WXProjectWeb.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// sssss
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        //test
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
