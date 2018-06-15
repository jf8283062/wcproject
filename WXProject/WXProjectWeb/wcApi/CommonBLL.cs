using Modal;
using Modal.WeiXinEvent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace WXProjectWeb.wcApi
{
    public class CommonBLL
    {
       
        public readonly static string Token = System.Configuration.ConfigurationManager.AppSettings["Token"];
        public readonly static string AppID = System.Configuration.ConfigurationManager.AppSettings["AppID"];
        public readonly static string Secret = System.Configuration.ConfigurationManager.AppSettings["Secret"];
        public readonly static string EncodingAESKey = System.Configuration.ConfigurationManager.AppSettings["EncodingAESKey"];
        /// <summary>
        /// 成员加入提醒模板
        /// </summary>
        public readonly static string Template_id = System.Configuration.ConfigurationManager.AppSettings["Template_id"];


        public static void SendWaitPicMsg(Modal.WeiXinEvent.ClickEvent model, UserInfo fromUser)
        {
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
                //获取活动
                case "huodong1":
                    CommonBLL.SendKeFuMsg(model.FromUserName, fromUser.nickname + @"
正在为您生成专属奖状。

把奖状分享到家长群，
1人扫码可获得1朵小红花，
集齐10朵小红花，
即可成为学习标兵，
获得由教研室整理的纸质包邮资料
【一、二年级语文期末复习资料】


【教研室郑重承诺】：
本活动真实有效
老师们整理这套资料花了蛮多时间
印刷质量也蛮好的
希望对孩子有帮助");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 加密验证
        /// </summary>
        /// <param name="signature">源</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">nonce</param>
        /// <param name="token">自定义Token</param>
        /// <returns></returns>
        public static bool CheckSignature(string signature, string timestamp, string nonce)
        {
            List<string> list = new List<string>();
            list.Add(Token);
            list.Add(timestamp);
            list.Add(nonce);
            list.Sort();
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item);
            }
            System.Security.Cryptography.HashAlgorithm SHA_1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] str = Encoding.UTF8.GetBytes(sb.ToString());
            str = SHA_1.ComputeHash(str);
            StringBuilder strSB = new StringBuilder();
            foreach (var item in str)
            {
                strSB.AppendFormat("{0:x2}", item);
            }
            return strSB.ToString() == signature;

        }

        public static string CreateSendFromMSG(string key, int count,UserInfo fromUser)
        {
            string remarkvalue = "";
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
                case "huodong1":
                    if (count < 10)
                    {
                        remarkvalue = @"恭喜：
您的好友" + fromUser.nickname + @"送你1朵小红花!
还需" + (10 - count) + @"位家长扫码送花
成为期末学习标兵
获取由教研室整理的：
【一、二年级语文期末复习资料】";
                    }
                    else if (count == 10)
                    {
                        remarkvalue = @"恭喜：你的人缘不错噢，
已有10朵小红花。正式成为学习标兵！

还有3步即可完成：
1、复制右侧优惠码：" + GetCouponTable("huodong1") + @"
2、进入如下地址，使用优惠码下单，即可1元包邮（为什么不是0元？请看下文）
https://weidian.com/?userid=880432647
3、加微信xuexi005,发送订单截图，值班老师发还1元红包给你


有疑问的看这里：
【问】：为什么是1元包邮，不是0元吗？
【答】：我们也很无奈，因为微店无法设置0元商品，所以1元下单后，请加老师的微信(xuexi005)，发订单截图，我们退还1元红包给你。顺便说一下，诸如为什么不直接设置1分钱售价等问题，我们都考虑过了，存在其它问题，感兴趣的牛爸牛妈可以一起探讨。

【问】：为什么要在店里下单？
【答】：因为教研室是一群老师，不是专业的互联网公司，没有能力开发送书管理系统。为了方便家长们跟踪快递，也为了方便教研室进行发货管理，我们直接使用了腾讯投资的微店系统。";
                    }

                    break;
                default:
                    break;
            }
            return remarkvalue;
        }

        public static Dictionary<string, DateTime> Access_token = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid">开发者ID</param>
        /// <param name="appsecret">开发者密码</param>
        /// <returns></returns>
        public static string GetAccess_token()
        {
            string access_token = ""; //获取的access_token;
            
            if (Access_token == null || Access_token.FirstOrDefault().Value < DateTime.Now)
            {

                string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppID + "&secret=" + Secret;

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                req.Method = "GET";

                using (WebResponse wr = req.GetResponse())
                {
                    StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    string content = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                    //在这里对Access_token 赋值  
                    JObject job = (JObject)JsonConvert.DeserializeObject(content);
                    access_token = job["access_token"].ToString();
                }

                Access_token = new Dictionary<string, DateTime>() {
                    { access_token, DateTime.Now.AddHours(1) }
                    };
            }
            else
            {
                access_token = Access_token.FirstOrDefault().Key;
            }


            return access_token;
        }

        public static string GetCouponTable(string huodong)
        {
            var count = UserBLL.FinishShareCount(huodong);
            var model = UserBLL.Coupon(huodong, count + 1);
            //var model = UserBLL.GetCoupon(count + 1);
            if (model != null)
            {
                return model.Coupon;
            }
            else
            {
                return "";
            }

        }
        /// <summary>根据URL获取信息
        /// 根据URL获取信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns>返回一组json数据</returns>
        public static string GetInfomation(string url)
        {

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "Get";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            string content = sr.ReadToEnd();

            return content;
        }

        /// <summary> Post方式获取信息
        /// Post方式获取信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string GetInfomation(string url, string postData)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            request.Method = "Post";
            byte[] buffer = null;
            Encoding postencoding = Encoding.UTF8;
            buffer = postencoding.GetBytes(postData);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            string content = sr.ReadToEnd();

            return content;
        }


        /// <summary>
        /// 给指定用户发送模板消息  
        /// 使用成员加入提醒模板
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static string SendTemplateMsg(string openId, object data)
        {
            string accesstoken = GetAccess_token();
            string template_id = Template_id;
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", accesstoken);

            var postData = new { touser = openId, template_id = template_id, url = "", data = data };

            var json = JsonConvert.SerializeObject(postData);

            string content = CommonBLL.GetInfomation(url, json);

            return content;
        }



        /// <summary>
        /// 给指定用户发送模板消息  
        /// 使用成员加入提醒模板
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static string SendKeFuMsg(string openId, object data)
        {
            string accesstoken = GetAccess_token();

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", accesstoken);

            var postData = new { touser = openId, msgtype = "text", text = new { content = data } };

            var json = JsonConvert.SerializeObject(postData);

            string content = CommonBLL.GetInfomation(url, json);

            return content;
        }

        /// <summary>
        /// 给指定用户发送模板消息  
        /// 使用成员加入提醒模板
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static string SendTemplateMsg(string openId)
        {
            string accesstoken = GetAccess_token();
            string template_id = Template_id;
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", accesstoken);

            string firstvalue = "你有1位新朋友支持你啦!";
            string keyword1value = "辉";
            string keyword2value = "2014年9月22日 10:10:10";
            string remarkvalue = "你还差3位小伙伴的支持可获得活动奖励";
            var postData = new { touser = openId, template_id = template_id, url = "", data = new { first = new { value = firstvalue }, keyword1 = new { value = keyword1value }, keyword2 = new { value = keyword2value }, remark = new { value = remarkvalue } } };

            var json = JsonConvert.SerializeObject(postData);

            string content = CommonBLL.GetInfomation(url, json);

            return content;
        }


        /// <summary>
        /// 转换xml到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static T ConvertObj<T>(string xmlstr)
        {
            XElement xdoc = XElement.Parse(xmlstr);
            var type = typeof(T);
            var t = Activator.CreateInstance<T>();
            foreach (XElement element in xdoc.Elements())
            {
                var pr = type.GetProperty(element.Name.ToString());
                if (element.HasElements)
                {//这里主要是兼容微信新添加的菜单类型。nnd，竟然有子属性，所以这里就做了个子属性的处理
                    foreach (var ele in element.Elements())
                    {
                        pr = type.GetProperty(ele.Name.ToString());
                        pr.SetValue(t, Convert.ChangeType(ele.Value, pr.PropertyType), null);
                    }
                    continue;
                }
                if (pr != null && pr.PropertyType != null)
                {
                    if (pr.PropertyType.Name == "MsgType")//获取消息模型
                    {
                        pr.SetValue(t, (MsgType)Enum.Parse(typeof(MsgType), element.Value.ToUpper()), null);
                        continue;
                    }
                    if (pr.PropertyType.Name == "Event")//获取事件类型。
                    {
                        pr.SetValue(t, (EventEnum)Enum.Parse(typeof(EventEnum), element.Value.ToUpper()), null);
                        continue;
                    }
                    pr.SetValue(t, Convert.ChangeType(element.Value, pr.PropertyType), null);
                }
            }
            return t;
        }
    }
}