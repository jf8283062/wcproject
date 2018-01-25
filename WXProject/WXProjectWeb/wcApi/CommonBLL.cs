﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WXProjectWeb.wcApi
{
    public class CommonBLL
    {
        public readonly static string Token = System.Configuration.ConfigurationManager.AppSettings["Token"];
        public readonly static string AppID = System.Configuration.ConfigurationManager.AppSettings["AppID"];
        public readonly string EncodingAESKey = System.Configuration.ConfigurationManager.AppSettings["EncodingAESKey"]; 

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
        public static string GetAccess_token(string appid, string secret)
        {
            string access_token = ""; //获取的access_token;
             appid =string.IsNullOrEmpty(appid)?"wxc4c7440bca3101a6":appid; //公众号appid
             secret = string.IsNullOrEmpty(secret) ? "381010e26151aedf8ae531500d668513":secret;  //公众号appsecret
            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;

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
        private static string GetInfomation(string url)
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
        private static string GetInfomation(string url, string postData)
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
        /// 获取永久二维码的ticket
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="scene_str"></param>
        /// <returns></returns>
        public static string GetQrcode(string access_token, string scene_str)
        {
            string ticket = "";
            string qrcodeUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            qrcodeUrl = string.Format(qrcodeUrl, access_token);

            var data = new { expire_seconds = 1800, action_name = "QR_LIMIT_STR_SCENE", action_info = new { scene = new { scene_str = scene_str } } };
            var json = JsonConvert.SerializeObject(data);

            string content = GetInfomation(qrcodeUrl, json);

            if (content.IndexOf("ticket") > -1)
            {
                JObject job = (JObject)JsonConvert.DeserializeObject(content);
                ticket = job["ticket"].ToString();
                ticket = Uri.EscapeDataString(ticket);
            }
            return ticket;
        }


        public static string GetQrcodePic(string ticket)
        {
            string Picurl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket + "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Picurl);
            req.Proxy = null;
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:11.0) Gecko/20100101 Firefox/11.0";
            req.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.8,zh-hk;q=0.6,ja;q=0.4,zh;q=0.2");
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
 

            System.Drawing.Image img = System.Drawing.Image.FromStream(req.GetResponse().GetResponseStream());
            string newfilename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
            string path = HttpContext.Current.Server.MapPath("~/img/" + newfilename);
            img.Save(path);
            return path;
        }
    }
}