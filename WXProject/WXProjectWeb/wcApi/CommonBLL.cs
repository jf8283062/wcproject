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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid">开发者ID</param>
        /// <param name="appsecret">开发者密码</param>
        /// <returns></returns>
        public static string GetAccess_token()
        {
            string access_token = ""; //获取的access_token;
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
                if (pr!=null&&pr.PropertyType!=null)
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