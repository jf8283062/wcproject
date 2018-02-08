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
        public static Dictionary<string, string> dic = new Dictionary<string, string>() {
               { "B103",@"后续我们会提供更多实用的资料。 

人教版1-6年级语文上册期末测试卷+答案 ⑩
链接：https://pan.baidu.com/s/1qZzwX4o 
密码：abv5

注意：
1、资源7天有效期限，建议尽快转存到自己的网盘，如失效请加微信liruijuan628
2、首次点击可能报错，请再次点击，即可正常访问；建议用手机版QQ或网页版微信转发完整地址到电脑上。手打地址中间的大小写字母容易出错。"},

               {"A104",    @"后续我们会提供更多实用的资料。 

人教版1-6年级数学上册期末测试卷+答案⑩
链接：https://pan.baidu.com/s/1dlCvIu
密码：56zy

注意：
1、资源7天有效期限，建议尽快转存到自己的网盘，如失效请加微信liruijuan628 
2、首次点击可能报错，请再次点击，即可正常访问；建议用手机版QQ或网页版微信转发完整地址到电脑上。手打地址中间的大小写字母容易出错。"},
               {
"B105",    @"后续我们会提供更多实用的资料。 

外研版3-6年级英语上册期末测试卷+答案①
链接：https://pan.baidu.com/s/1kWG2hIB 
密码：vtre

注意：
1、资源7天有效期限，建议尽快转存到自己的网盘，如失效请加微信liruijuan628 
2、首次点击可能报错，请再次点击，即可正常访问；建议用手机版QQ或网页版微信转发完整地址到电脑上。手打地址中间的大小写字母容易出错。"},

               { "A106",@"后续我们会提供更多实用的资料。 

最全小学语文多音字汇总
链接：https://pan.baidu.com/s/1mjx36Da 
密码：v4qk

注意：
1、资源7天有效期限，建议尽快转存到自己的网盘，如失效请加微信liruijuan628 
2、首次点击可能报错，请再次点击，即可正常访问；建议用手机版QQ或网页版微信转发完整地址到电脑上。手打地址中间的大小写字母容易出错。"},
               { "B107",    @"后续我们会提供更多实用的资料。 

小学生写人作文四大技巧+1-6年级范文
链接：https://pan.baidu.com/s/1pNt9Syn 
密码：11v5

注意：
1、资源7天有效期限，建议尽快转存到自己的网盘，如失效请加微信liruijuan628 
2、首次点击可能报错，请再次点击，即可正常访问；建议用手机版QQ或网页版微信转发完整地址到电脑上。手打地址中间的大小写字母容易出错。"},
               { "A108",    @"后续我们会提供更多实用的资料。 

全套故事情节梗概＋必考文学常识＋小初高重点考题任意两份】礼包！
链接：https://pan.baidu.com/s/1i7nat3R 
密码：y7hs

注意：
1、资源7天有效期限，建议尽快转存到自己的网盘，如失效请加微信liruijuan628 
2、首次点击可能报错，请再次点击，即可正常访问；建议用手机版QQ或网页版微信转发完整地址到电脑上。手打地址中间的大小写字母容易出错。"},
               { "B109",    @"后续我们会提供更多实用的资料。

1-9年级语文必背古诗文135篇（含音频）+出自《论语》中的成语
详解链接：http://pan.baidu.com/s/1boQmcSB 
密码：gnld
 
 注意： 
1、资源7天有效期限，建议尽快转存到自己的网盘，如失效请加微信liruijuan628 
2、首次点击可能报错，请再次点击，即可正常访问；建议用手机版QQ或网页版微信转发完整地址到电脑上。手打地址中间的大小写字母容易出错。"},
               {"A110",    @"后续我们会提供更多实用的资料。

小学1-6年级奥数寒假班完整教材
链接: https://pan.baidu.com/s/1nwJgG1z 
密码: ewxn

 注意： 
1、资源7天有效期限，建议尽快转存到自己的网盘，如失效请加微信liruijuan628 
2、首次点击可能报错，请再次点击，即可正常访问；建议用手机版QQ或网页版微信转发完整地址到电脑上。手打地址中间的大小写字母容易出错。"},
               { "B111",    @"后续我们会提供更多实用的资料。

海尼曼分级阅读【全套绘本PDF+配套音频+打印版】
链接：https://pan.baidu.com/s/1snsk7Y9 
密码：s8dp

 注意： 
1、资源7天有效期限，建议尽快转存到自己的网盘，如失效请加微信liruijuan628 
2、首次点击可能报错，请再次点击，即可正常访问；建议用手机版QQ或网页版微信转发完整地址到电脑上。手打地址中间的大小写字母容易出错。"},

               {"谢谢, 感谢, 谢了","辛苦了 不客气，亲。" },
               {" 合作, 广告, 投放","  合作请加微信号：xiaochaokefu" }

            };
        public readonly static string Token = System.Configuration.ConfigurationManager.AppSettings["Token"];
        public readonly static string AppID = System.Configuration.ConfigurationManager.AppSettings["AppID"];
        public readonly static string Secret = System.Configuration.ConfigurationManager.AppSettings["Secret"];
        public readonly static string EncodingAESKey = System.Configuration.ConfigurationManager.AppSettings["EncodingAESKey"];
        /// <summary>
        /// 成员加入提醒模板
        /// </summary>
        public readonly static string Template_id = System.Configuration.ConfigurationManager.AppSettings["Template_id"];

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