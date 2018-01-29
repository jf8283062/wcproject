﻿using Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            StreamReader sr = new StreamReader(Request.InputStream, Encoding.UTF8);
            string text = sr.ReadToEnd();
            if (!string.IsNullOrEmpty(text))
            {
                var eventmodel = WXMethdBLL.CreateMessage(text);
                if (eventmodel != null&& eventmodel is SubscribeEvent)
                {
                    SubscribeEvent model = eventmodel as SubscribeEvent;
                    var resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ContentRequest()
                    {
                        FromUserName = model.ToUserName,
                        ToUserName = model.FromUserName,
                        Content = model.Event +":"+ model.EventKey??"没有key"
                    });
                    return Content(resStr);
                }
                else
                {
                    EventBase model = eventmodel as EventBase;
                    var resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ContentRequest()
                    {
                        FromUserName = model.ToUserName,
                        ToUserName = model.FromUserName,
                        Content = "hi"
                    });
                    return Content(resStr);
                }
            }



            


            #region 微信验证URL
            // 微信加密签名
            string signature = Request["SIGNATURE"];
            // 时间戮
            string timestamp = Request["TIMESTAMP"];
            // 随机数
            string nonce = Request["NONCE"];
            // 随机字符串
            string echostr = Request["echostr"];
            var re = WXMethdBLL.CheckURL(signature, timestamp, nonce, echostr);
            return Content(re);
            #endregion            

        }

    }
}
