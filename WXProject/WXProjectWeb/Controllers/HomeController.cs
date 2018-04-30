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


                                string remarkvalue = "";
                                #region 生成分享活动不同生成不同的回复
                                switch (key)
                                {
                                    case "getpica":
                                        if (count < 5)
                                        {
                                            remarkvalue = @"恭喜：
您的好友" + fromUser.nickname + @"来支持你啦!
亲,还需" + (5 - count) + @"位小伙伴扫码支持
就可以免费领取价值千元的：
【幼升小英语启蒙课：学字母记单词】";

                                        }
                                        else if (count == 5)
                                        {
                                            remarkvalue = @"你的人缘不错噢，已经有5人来支持你。
你是一位重视教育的好家长，
孩子一定会越来越棒！

幼升小英语启蒙课
链接: https://pan.baidu.com/s/1o9e36l0 
密码: i4ps


后续我们还会提供更多实用的免费资料。

提醒：
1、请尽快转存到自己的网盘，如失效请加学习助手微信xuexi005；
2、建议转发完整地址到电脑上操作,手打地址容易出错。";
                                        }
                                        break;
                                    case "getpica1":
                                        if (count < 5)
                                        {
                                            remarkvalue = @"恭喜：
您的好友" + fromUser.nickname + @"来支持你啦!
亲,还需" + (5 - count) + @"位小伙伴扫码支持
就可以免费领取：：
【满分阅读51套答题公式】";

                                        }
                                        else if (count == 5)
                                        {
                                            remarkvalue = @"你的人缘不错噢，已经有5人来支持你。
你是一位重视教育的好家长，
孩子一定会越来越棒！

满分阅读51套答题公式
链接：https://pan.baidu.com/s/1pLZbWpl 
密码：7mot


后续我们还会提供更多实用的免费资料。

提醒：
1、请尽快转存到自己的网盘，如失效请加学习助手微信xuexi005；
2、建议转发完整地址到电脑上操作,手打地址容易出错。";
                                        }
                                        break;
                                    case "getpica2":
                                        if (count < 5)
                                        {
                                            remarkvalue = @"恭喜：
您的好友" + fromUser.nickname + @"来支持你啦!
亲,还需" + (5 - count) + @"位小伙伴扫码支持
就可以免费领取：：
【苏教版小学语文资料】";

                                        }
                                        else if (count == 5)
                                        {
                                            remarkvalue = @"你的人缘不错噢，已经有5人来支持你。
你是一位重视教育的好家长，
孩子一定会越来越棒！

苏教版小学语文资料
链接：https://pan.baidu.com/s/1VX-ujFY1dCKHm3S1vDVc7A 密码：bpaw


后续我们还会提供更多实用的免费资料。

提醒：
1、请尽快转存到自己的网盘，如失效请加学习助手微信xuexi005；
2、建议转发完整地址到电脑上操作,手打地址容易出错。。";
                                        }
                                        break;
                                    default:
                                        break;
                                }
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
热门资料，点击底部菜单获取。
其它资料，请回复相应的关键词获取。"
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
                        switch (model.EventKey)
                        {
                            case "getpica":
                                CommonBLL.SendKeFuMsg(model.FromUserName, fromUser.nickname + @"
欢迎来到小学生微学习。
正在为您生成专属任务海报。

把海报分享给家长朋友，
获5人扫码即可免费领取价值千元的【幼升小英语启蒙课】

我们郑重承诺：本活动真实有效。");
                                break;
                            case "getpica1":
                                CommonBLL.SendKeFuMsg(model.FromUserName, fromUser.nickname + @"
正在为您生成专属任务海报。

把海报分享到朋友圈，
获5人扫码即可免费领取经典资料【满分阅读51套答题公式】

我们郑重承诺：本活动真实有效。");
                                break;
                            case "getpica2":
                                CommonBLL.SendKeFuMsg(model.FromUserName, fromUser.nickname + @"
正在为您生成领资料海报。

把海报分享到朋友圈，获朋友扫码支持，
即可免费领取苏教版语文资料。

备注：
如果不方便分享，也可以加老师xuexi005
发资料名称，即可索取。
因人多，老师回复慢，请见谅。
我们郑重承诺：本活动真实有效。");
                                break;
                            default:
                                break;
                        }

                        var ticket = QrcodeBLL.Get_QR_STR_SCENE_Qrcode(_token, model.FromUserName+"#"+ model.EventKey);
                        var QrStream = QrcodeBLL.GetQrcodeStream(ticket);
                        var touxiangStream = UserBLL.GetTouxiang(fromUser.headimgurl);
                        var bg = ImgCom.ImgCommon.AddWaterPic(ImgCom.ImgCommon.GetBGImgMemoryStream(model.EventKey), touxiangStream, QrStream, null, "我领取了");
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
                        if (model.Content.Contains("合作") || model.Content.Contains("广告") || model.Content.Contains("投放"))
                        {
                            content = "合作请加微信号：xiaochaokefu";
                        }
                        //获取后台添加的问答消息
                        AutoResponse item = wcApi.AutoResponseBLL.GetContentbyQuestion(model.Content);
                        if (content == "")
                        {
                            return Content("");

                        }
                        if (item.type == "text")
                        {
                            content = item.ReplyContent;
                        }
                        resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ContentRequest()
                        {
                            FromUserName = model.ToUserName,
                            ToUserName = model.FromUserName,
                            Content = content
                        });

                        if (item.type == "image")
                        {
                            var x = MediaBLL.UploadMultimedia(_token, "image",item.RoomImgPath);
                            resStr = WXMethdBLL.ResponseMsg(new Modal.WeiXinRequest.ImageReuquest()
                            {
                                FromUserName = model.ToUserName,
                                ToUserName = model.FromUserName,
                                MediaId = x
                            });
                        }
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


        public ActionResult Test3()
        {
            string media_id = "";
            AutoResponse item = wcApi.AutoResponseBLL.GetContentbyQuestion("儿童节");

            string _token = CommonBLL.GetAccess_token();

            if (item.type == "image")
            {
                var x = MediaBLL.UploadMultimedia(_token, "image", item.RoomImgPath);
                media_id = x;
            }

            var bgpath = AppDomain.CurrentDomain.BaseDirectory + item.RoomImgPath;
            FileStream fs = new FileStream(bgpath, FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            fs.Dispose();

            var x1 = MediaBLL.UploadMultimedia(_token, "image","",data);
            return Content(media_id);
        }
    }
}
