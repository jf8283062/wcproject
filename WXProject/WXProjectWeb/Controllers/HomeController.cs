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
     
        public HomeController()
        {
     
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

                string _token = CommonBLL.GetAccess_token();
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

                                var arr = model.EventKey.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                                string toUserOpenid = arr[0];
                                string key = arr[1];
                                var user = UserBLL.SaveUserShareCount(toUserOpenid,key);
                                //根据不同的活动添加/更新数据
                                var count = user.count;

                                
                                #region 生成分享活动不同生成不同的回复

                                string remarkvalue = CommonBLL.CreateSendFromMSG(key,count,fromUser);
                                #endregion
                                string content = CommonBLL.SendKeFuMsg(toUserOpenid, remarkvalue);

                            }

                            #endregion
                        }
                        resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ContentRequest()
                        {
                            FromUserName = model.ToUserName,
                            ToUserName = model.FromUserName,
                            Content = @"欢迎你，家长朋友。

我们是一帮小学教育工作者，

工作之余，

在这里分享教育心得。



领取提示：

热门资料，点击底部菜单获取
其它资料，回复相应关键词获取"
                        });
                        return Content(resStr);

                    }
                    //点击事件生成返回二维码
                    else if (eventmodel is Modal.WeiXinEvent.ClickEvent)
                    {
                        #region 点击button生成二维码
                        Modal.WeiXinEvent.ClickEvent model = eventmodel as Modal.WeiXinEvent.ClickEvent;
                        var fromUser = UserBLL.GetUserInfo(eventmodel.FromUserName);
                        if (fromUser == null)
                        {

                            fromUser = UserBLL.GetUserDetail(_token, eventmodel.FromUserName);
                            fromUser.count = 0;
                            UserBLL.SaveUsers(fromUser);
                        }
                        CommonBLL.SendWaitPicMsg(model, fromUser);

                        if (model.EventKey == "huodong1")
                        {
                            int count = UserBLL.FinishShareCount("huodong1");
                            var setHongdong1Count= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["huodong1"]);
                            if (count >= setHongdong1Count)
                            {
                                resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ContentRequest()
                                {
                                    FromUserName = model.ToUserName,
                                    ToUserName = model.FromUserName,
                                    Content = @"对不起，亲爱的；

1000套0元包邮【一、二年级语文期末复习资料】已经被抢光了。如果您还需要，请留言给我们，超过100人留言，教研室就加印哦。

心急的家长，也可以进入如下地址原价下单。https://weidian.com/?userid=880432647"
                                });
                                return Content(resStr);
                            }
                        }
                       
                        var ticket = QrcodeBLL.Get_QR_STR_SCENE_Qrcode(_token, model.FromUserName+"#"+ model.EventKey);
                        var QrStream = QrcodeBLL.GetQrcodeStream(ticket);
                        var touxiangStream = UserBLL.GetTouxiang(fromUser.headimgurl);
                        byte[] bg = null;
                        if (model.EventKey == "huodong1")
                        {
                             bg = ImgCom.ImgCommon.AddWaterPic(ImgCom.ImgCommon.GetBGImgMemoryStream(model.EventKey), touxiangStream, QrStream, "huodong1", null, "");
                        }
                        else
                        {
                             bg = ImgCom.ImgCommon.AddWaterPic(ImgCom.ImgCommon.GetBGImgMemoryStream(model.EventKey), touxiangStream, QrStream,null, null, "我领取了");

                        }
                        var x = MediaBLL.UploadMultimedia(_token, "image", model.ToUserName + ".jpg", bg);



                        resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ImageReuquest()
                        {
                            FromUserName = model.ToUserName,
                            ToUserName = model.FromUserName,
                            MediaId = x
                        });

                        return Content(resStr);
                        #endregion

                    }
                    //接受消息
                    else if (eventmodel is Modal.WeiXinEvent.TextMessage)
                    {
                        #region 接收消息处理
                        Modal.WeiXinEvent.TextMessage model = eventmodel as Modal.WeiXinEvent.TextMessage;
                        string content = "";
                        //获取后台添加的问答消息
                        AutoResponse item = wcApi.AutoResponseBLL.GetContentbyQuestion(model.Content);
                        if (item!=null&&item.type == "text")
                        {
                            content = item.ReplyContent;
                            resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ContentRequest()
                            {
                                FromUserName = model.ToUserName,
                                ToUserName = model.FromUserName,
                                Content = content
                            });
                        }else if (item != null && item.type == "image")
                        {
                           var x_bgpath = Server.MapPath(item.RoomImgPath);

                            FileStream fs = new FileStream(x_bgpath, FileMode.Open);
                            byte[] x_data = new byte[fs.Length];
                            fs.Read(x_data, 0, x_data.Length);
                            fs.Close();
                            fs.Dispose();

                            var x = MediaBLL.UploadMultimedia(_token, "image", model.ToUserName + ".jpg", x_data);

                            resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ImageReuquest()
                            {
                                FromUserName = model.ToUserName,
                                ToUserName = model.FromUserName,
                                MediaId = x
                            });
                        }
                        else
                        {
                            resStr = "";
                        }
                        return Content(resStr);
                        #endregion
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


        public ActionResult Test3()
        {
            string media_id = "";
            AutoResponse item = wcApi.AutoResponseBLL.GetContentbyQuestion("劳动节");

            string _token = CommonBLL.GetAccess_token();

            var bgpath = AppDomain.CurrentDomain.BaseDirectory + item.RoomImgPath;
            bgpath = Server.MapPath(item.RoomImgPath);

            if (item.type == "image")
            {
                var x = MediaBLL.UploadMultimedia(_token, "image", bgpath);
                media_id = x;
            }

            ///获取临时素材
            string path = HttpContext.Server.MapPath("~/img/");
            MediaBLL.GetMultimedia(_token, media_id, path);

            FileStream fs = new FileStream(bgpath, FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            fs.Dispose();

            var x1 = MediaBLL.UploadMultimedia(_token, "image","aa.jpg",data);
            media_id = x1;
           
            MediaBLL.GetMultimedia(_token, media_id, path);
            return Content(media_id);
        }


        public ActionResult Test4()
        {
            int count = UserBLL.FinishShareCount("huodong1");
            var setHongdong1Count = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["huodong1"]);

            return Content("count:"+count +  "    hond:"+setHongdong1Count);
        }
    }
}
