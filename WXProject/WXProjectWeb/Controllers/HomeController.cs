using Modal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
        public static MemoryStream ms = null;

        public static Dictionary<string, DateTime> Access_token = null;


        public HomeController()
        {
            if (ms == null)
            {
                var bgpath = AppDomain.CurrentDomain.BaseDirectory + "\\img\\" + "backGroudImd.jpg";
                FileStream fs = new FileStream(bgpath, FileMode.Open);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();
                fs.Dispose();
                ms = new MemoryStream(data);
            }

        }
        /// <summary>
        /// sssss
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string _token = "";
            if (Access_token == null || Access_token.FirstOrDefault().Value < DateTime.Now)
            {
                var token = CommonBLL.GetAccess_token();
                Access_token = new Dictionary<string, DateTime>() {
                    { token, DateTime.Now.AddSeconds(7000) }
                    };
            }
            else
            {
                _token = Access_token.FirstOrDefault().Key;
            }
            StreamReader sr = new StreamReader(Request.InputStream, Encoding.UTF8);
            string text = sr.ReadToEnd();
            if (!string.IsNullOrEmpty(text))
            {
                string resStr = "";
                var eventmodel = WXMethdBLL.CreateMessage(text);
                if (eventmodel != null && eventmodel is SubscribeEvent)
                {
                    SubscribeEvent model = eventmodel as SubscribeEvent;
                    if (!string.IsNullOrWhiteSpace(model.EventKey))
                    {
                        var user = UserBLL.GetUserInfo(model.EventKey);
                        user.count = user.count + 1;
                        UserBLL.UpdateUser(user);
                    }
                    var fromUser = UserBLL.GetUserDetail(_token, eventmodel.FromUserName);
                    if (fromUser == null)
                    {
                        fromUser.count = 0;
                        UserBLL.SaveUsers(fromUser);

                    }
                    resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ContentRequest()
                    {
                        FromUserName = model.ToUserName,
                        ToUserName = model.FromUserName,
                        Content = "感谢关注！回复任意消息可以获得定制二维码！"
                    });
                    return Content(resStr);
                }
                else
                {

                    EventBase model = eventmodel as EventBase;

                    var ticket = QrcodeBLL.Get_QR_STR_SCENE_Qrcode(_token, model.FromUserName);
                    var QrStream = QrcodeBLL.GetQrcodeStream(ticket);
                    var user = UserBLL.GetUserInfo(model.FromUserName);
                    if (user==null)
                    {
                        user = UserBLL.GetUserDetail(_token, model.FromUserName);
                        UserBLL.AddUser(user);
                    }

                    var touxiangStream = UserBLL.GetTouxiang(user.headimgurl);

                    var bg = ImgCom.ImgCommon.AddWaterPic(ms, touxiangStream, QrStream);

                    var x = MediaBLL.UploadMultimedia(_token, "image", model.ToUserName+".jpg", bg);

                    resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ImageReuquest()
                    {
                        FromUserName = model.ToUserName,
                        ToUserName = model.FromUserName,
                        MediaId = x
                    });
                    return Content(resStr);

                }

            }
            else
            {
                return Content("");
            }




            //var touXiangPath = AppDomain.CurrentDomain.BaseDirectory + "\\img\\" + "touxiang.jpg";
            //FileStream fsTouXiang = new FileStream(touXiangPath, FileMode.Open);
            //byte[] dataTouXiang = new byte[fsTouXiang.Length];
            //fsTouXiang.Read(dataTouXiang, 0, dataTouXiang.Length);
            //fsTouXiang.Close();
            //fsTouXiang.Dispose();
            //MemoryStream mstouxiang = new MemoryStream(dataTouXiang);
            //var erweima = AppDomain.CurrentDomain.BaseDirectory + "\\img\\" + "erweima.jpg";
            //FileStream fserweima = new FileStream(erweima, FileMode.Open);
            //byte[] dataerweima = new byte[fserweima.Length];
            //fserweima.Read(dataerweima, 0, dataerweima.Length);
            //fserweima.Close();
            //fserweima.Dispose();
            //MemoryStream mserweima = new MemoryStream(dataerweima);



            //var outsteam = ImgCom.ImgCommon.AddWaterPic(ms, mstouxiang, mserweima, "张辉", "测试内容就是这样");


            //return File(outsteam, "image/jpeg");







            //#region 微信验证URL
            //// 微信加密签名
            //string signature = Request["SIGNATURE"];
            //// 时间戮
            //string timestamp = Request["TIMESTAMP"];
            //// 随机数
            //string nonce = Request["NONCE"];
            //// 随机字符串
            //string echostr = Request["echostr"];
            //var re = WXMethdBLL.CheckURL(signature, timestamp, nonce, echostr);
            //return Content("123");
            //#endregion

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


            string token = CommonBLL.GetAccess_token();

            UserInfo user = UserBLL.GetUserDetail(token, openid);


            UserInfo us = UserBLL.GetUserInfo(user.openid);

            if (us != null)
            {
                us.count = us.count + 1;
                UserBLL.ModifyUsers(us);
            }
            else
            {
                UserBLL.SaveUsers(user);
            }


            //DataSet ds = DbHelperSQL.Query("select * from UserInfo where openid='" + openid + "'");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    UserBLL.UpdateUser(new UserInfo() { openid = openid, count = int.Parse(ds.Tables[0].Rows[0]["count"].ToString()) + 1 });
            //}
            //else
            //{
            //    UserBLL.AddUser(new UserInfo() { openid = openid,count=0});
            //}

            ///头像流
            string touxiangUrl = user.headimgurl;
            Stream mstouxiang = UserBLL.GetTouxiang(touxiangUrl);

            ///二维码流
            string ticket = QrcodeBLL.Get_QR_STR_SCENE_Qrcode(token, openid);
            Stream mserweima = QrcodeBLL.GetQrcodeStream(ticket);

            ///背景图片流
            var bgpath = AppDomain.CurrentDomain.BaseDirectory + "\\img\\" + "backGroudImd.jpg";
            FileStream fs = new FileStream(bgpath, FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            fs.Dispose();
            MemoryStream ms = new MemoryStream(data);

            ///合成图片
            var outsteam = ImgCom.ImgCommon.AddWaterPic(ms, mstouxiang, mserweima, "张辉", "测试内容就是这样");

            ///上传图片
            string media_id = MediaBLL.UploadMultimedia(token, "image", "hui.jpg", outsteam);

            ///获取临时素材
            string path = HttpContext.Server.MapPath("~/img/");
            MediaBLL.GetMultimedia(token, media_id, path);

            return Content(media_id);
        }



    }
}
