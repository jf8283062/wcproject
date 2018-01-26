using Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WXProjectWeb.wcApi;

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
            //string token = CommonBLL.GetAccess_token("","");
            //string ticket = CommonBLL.GetQrcode(token, "hello");
            //string path = CommonBLL.GetQrcodePic(ticket);


            #region 微信验证URL

            //// 微信加密签名
            //string signature = Request["SIGNATURE"];
            //// 时间戮
            //string timestamp = Request["TIMESTAMP"];
            //// 随机数
            //string nonce = Request["NONCE"];
            //// 随机字符串
            //string echostr = Request["echostr"];
            //var re = WXMethdBLL.CheckURL(signature, timestamp, nonce, echostr);
            #endregion


 



            return Content("ok");

        }

    }
}
