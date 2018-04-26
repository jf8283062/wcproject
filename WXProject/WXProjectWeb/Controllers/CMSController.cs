using Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        #region
        [AuthorizeFilterAttribute]
        public ActionResult AutoReply()
        {
            ViewBag.Tab = "自动回复";
            return View();
        }

        /// <summary>
        /// 获取回复命令
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAutoReplyInfo()
        {
            var p = new List<Modal.AutoReply>();
            p.Add(new Modal.AutoReply() { ID = 1, Question = "nihao", ReplyContent = "ni hao too!" });
            return Json(new { status = 1, data = p }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 更新回复命令
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int UpdateAutoReplyInfo(string data)
        {

            AutoReply bs = new AutoReply();

            bs.ID = 1;
            bs.ModifiedTime = DateTime.Now;
            return wcApi.AutoReplyBLL.UpdateAutoReplyInfo(bs);
        }

        /// <summary>
        /// 添加回复命令
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult AddNewAutoReplyInfo(string question, string replaycontent)
        {
            
            AutoReply bs = new AutoReply();
            bs.ID = 2;
            bs.Question = "33";
            bs.ReplyContent = "4444";
            var p = wcApi.AutoReplyBLL.AddNewAutoReplyInfo(bs);
            return Json(new { status = 1, data = p });
        }

        #endregion
    }
}