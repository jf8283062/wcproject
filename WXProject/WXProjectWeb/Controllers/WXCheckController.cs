using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXProjectWeb.wcApi;

namespace WXProjectWeb.Controllers
{
    public class WXCheckController : Controller
    {
        public string WeiXin()
        {
            // 微信加密签名
            string signature = Request["SIGNATURE"];
            // 时间戮
            string timestamp = Request["TIMESTAMP"];
            // 随机数
            string nonce = Request["NONCE"];
            // 随机字符串
            string echostr = Request["echostr"];
            return CommonBLL.CheckSignature(signature, timestamp, nonce) ? echostr : "";
        }

    }
}