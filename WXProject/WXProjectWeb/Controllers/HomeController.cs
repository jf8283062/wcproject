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


            #region 微信验证URL
            // 微信加密签名
            string signature = Request["SIGNATURE"];
            // 时间戮
            string timestamp = Request["TIMESTAMP"];
            // 随机数
            string nonce = Request["NONCE"];
            // 随机字符串
            string echostr = Request["echostr"];
            if (!string.IsNullOrWhiteSpace(signature) && !string.IsNullOrWhiteSpace(timestamp) && !string.IsNullOrWhiteSpace(nonce) && !string.IsNullOrWhiteSpace(echostr))
            {
                var re = WXMethdBLL.CheckURL(signature, timestamp, nonce, echostr);
                return Content(re);
            }
            #endregion
            else
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
                    if (eventmodel == null)
                    {
                        return Content("");
                    }
                    //关注事件
                    if (eventmodel is SubscribeEvent)
                    {
                        SubscribeEvent model = eventmodel as SubscribeEvent;
                        var fromUser = UserBLL.GetUserInfo(eventmodel.FromUserName);
                        if (fromUser == null)
                        {
                            #region 第一次关注
                            fromUser = UserBLL.GetUserDetail(_token, eventmodel.FromUserName);
                            fromUser.count = 0;
                            UserBLL.SaveUsers(fromUser);

                            if (!string.IsNullOrWhiteSpace(model.EventKey))
                            {
                                model.EventKey = model.EventKey.Substring(model.EventKey.IndexOf("_") + 1);
                                var user = UserBLL.GetUserInfo(model.EventKey);
                                user.count = user.count + 1;
                                UserBLL.UpdateUser(user);
                                if (user.count < 5)
                                {
                                    string firstvalue = "启禀少主！您有1位新朋友支持你啦!";
                                    string keyword1value = fromUser.nickname;
                                    string keyword2value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    string remarkvalue = "还需要" + (5 - user.count).ToString() + "位小伙伴扫码关注，就可以解锁领取【112份著名绘本及配套教案】";
                                    var data = new { first = new { value = firstvalue }, keyword1 = new { value = keyword1value }, keyword2 = new { value = keyword2value }, remark = new { value = remarkvalue } };
                                    string content = CommonBLL.SendTemplateMsg(model.EventKey, data);
                                }
                                else
                                {
                                    string firstvalue = "启禀少主！您有1位新朋友支持你啦!";
                                    string keyword1value = fromUser.nickname;
                                    string keyword2value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    string remarkvalue = "已经有" + user.count + "位小伙伴扫码关注，请联系管理员索要电子资料";
                                    var data = new { first = new { value = firstvalue }, keyword1 = new { value = keyword1value }, keyword2 = new { value = keyword2value }, remark = new { value = remarkvalue } };
                                    string content = CommonBLL.SendTemplateMsg(model.EventKey, data);
                                }
                            }
                            #endregion
                        }
                        resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ContentRequest()
                        {
                            FromUserName = model.ToUserName,
                            ToUserName = model.FromUserName,
                            Content = "感谢关注！！"
                        });
                        return Content(resStr);
                    }
                    //点击事件生成返回二维码
                    else if (eventmodel is Modal.WeiXinEvent.ClickEvent)
                    {

                        Modal.WeiXinEvent.ClickEvent model = eventmodel as Modal.WeiXinEvent.ClickEvent;

                        var ticket = QrcodeBLL.Get_QR_STR_SCENE_Qrcode(_token, model.FromUserName);
                        var QrStream = QrcodeBLL.GetQrcodeStream(ticket);
                        var user = UserBLL.GetUserInfo(model.FromUserName);
                        if (user == null)
                        {
                            user = UserBLL.GetUserDetail(_token, model.FromUserName);
                            UserBLL.AddUser(user);
                        }

                        var touxiangStream = UserBLL.GetTouxiang(user.headimgurl);

                        var bg = ImgCom.ImgCommon.AddWaterPic(ms, touxiangStream, QrStream);

                        var x = MediaBLL.UploadMultimedia(_token, "image", model.ToUserName + ".jpg", bg);

                        resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ImageReuquest()
                        {
                            FromUserName = model.ToUserName,
                            ToUserName = model.FromUserName,
                            MediaId = x
                        });
                        return Content(resStr);

                    }
                    //接受消息
                    else if (eventmodel is Modal.WeiXinEvent.TextMessage)
                    {
                        Modal.WeiXinEvent.TextMessage model = eventmodel as Modal.WeiXinEvent.TextMessage;
                        string content = "";
                        if (CommonBLL.dic.ContainsKey(model.Content))
                        {
                            content = CommonBLL.dic[model.Content];
                        }
                        if (model.Content.Contains("谢"))
                        {
                            content = "辛苦了 不客气，亲。";
                        }
                        if (model.Content.Contains("合作")|| model.Content.Contains("广告")|| model.Content.Contains("投放"))
                        {
                            content = "合作请加微信号：xiaochaokefu";
                        }
                        resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ContentRequest()
                        {
                            FromUserName = model.ToUserName,
                            ToUserName = model.FromUserName,
                            Content = content
                        });
                        return Content(resStr);
                    }

                }
                return Content("");
            }
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

        public ActionResult Test2()
        {
            /// hui_open id "oVWwA044l4_gH37FSmlyqvF04LX0"
            /// feng_open_id "oVWwA0x8AB3fkTdokUxBflTkVIZk"
            string openid = "oVWwA0x8AB3fkTdokUxBflTkVIZk";


            string token = CommonBLL.GetAccess_token();

            UserInfo model = UserBLL.GetUserDetail(token, openid);

            string firstvalue = "你有1位新朋友支持你啦!";
            string keyword1value = model.nickname;
            string keyword2value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string remarkvalue = "你还差3位小伙伴的支持可获得活动奖励";
            var data = new { first = new { value = firstvalue }, keyword1 = new { value = keyword1value }, keyword2 = new { value = keyword2value }, remark = new { value = remarkvalue } };

            string content = CommonBLL.SendTemplateMsg(openid, data);


            return Content(token);

        }

    }
}
