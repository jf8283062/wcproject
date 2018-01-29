using Modal;
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
                if (eventmodel is EventBase)
                {
                    EventBase model = eventmodel as EventBase;
                    WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ContentRequest()
                    {
                        FromUserName = model.ToUserName,
                        ToUserName = model.FromUserName,
                        Content = "ToUserName:" + model.ToUserName + "    /r/n" + "FromUserName:" + model.FromUserName + "    /r/n" + "EventKey:" + model.Event
                    });
                }
            }
           



            //string token = CommonBLL.GetAccess_token("","");
            //string ticket = CommonBLL.GetQrcode(token, 123);
            //string path = CommonBLL.GetQrcodePic(ticket);


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
            #endregion






            return Content("ok");

        }


        /// <summary>
        /// 测试函数
        /// </summary>
        /// <returns></returns>
        public ActionResult Test()
        {

            /// hui_open id "oVWwA044l4_gH37FSmlyqvF04LX0"
            /// feng_open_id "oVWwA0x8AB3fkTdokUxBflTkVIZk"
            string openid = "oVWwA044l4_gH37FSmlyqvF04LX0";

            if (DbHelperSQL.Query("select * from UserInfo where openid='" + openid + "'").Tables[0].Rows.Count > 0)
            {
                UserBLL.UpdateUser(new UserInfo() { openid = openid });
            }
            else
            {
                UserBLL.AddUser(new UserInfo() { openid = openid });
            }
            



            //string token = CommonBLL.GetAccess_token();

            //UserInfo user = UserBLL.GetUserDetail(token, openid);



            //string ticket = CommonBLL.Get_QR_STR_SCENE_Qrcode(token, openid);
            //string path = CommonBLL.GetQrcodePic(ticket);

            //string media_id = MediaBLL.UploadMultimedia(token, "image", path);

            return Content(openid);
        }
    }
}
