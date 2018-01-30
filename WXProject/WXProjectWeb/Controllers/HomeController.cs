using Modal;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
                if (eventmodel != null && eventmodel is SubscribeEvent)
                {
                    SubscribeEvent model = eventmodel as SubscribeEvent;
                    var resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ContentRequest()
                    {
                        FromUserName = model.ToUserName,
                        ToUserName = model.FromUserName,
                        Content = model.Event + ":" + model.EventKey ?? "没有key"
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


            var bgpath = AppDomain.CurrentDomain.BaseDirectory + "\\img\\" + "backGroudImd.jpg";
            FileStream fs = new FileStream(bgpath, FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            fs.Dispose();
            MemoryStream ms = new MemoryStream(data);

            var touXiangPath = AppDomain.CurrentDomain.BaseDirectory + "\\img\\" + "touxiang.jpg";
            FileStream fsTouXiang = new FileStream(touXiangPath, FileMode.Open);
            byte[] dataTouXiang = new byte[fsTouXiang.Length];
            fsTouXiang.Read(dataTouXiang, 0, dataTouXiang.Length);
            fsTouXiang.Close();
            fsTouXiang.Dispose();
            MemoryStream mstouxiang = new MemoryStream(dataTouXiang);
            var erweima = AppDomain.CurrentDomain.BaseDirectory + "\\img\\" + "erweima.jpg";
            FileStream fserweima = new FileStream(erweima, FileMode.Open);
            byte[] dataerweima = new byte[fserweima.Length];
            fserweima.Read(dataerweima, 0, dataerweima.Length);
            fserweima.Close();
            fserweima.Dispose();
            MemoryStream mserweima = new MemoryStream(dataerweima);



            var outsteam = ImgCom.ImgCommon.AddWaterPic(ms, mstouxiang, mserweima, "张辉", "测试内容就是这样");
            

                return File(outsteam, "image/jpeg");

            





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
