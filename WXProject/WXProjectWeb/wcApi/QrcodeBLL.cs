using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace WXProjectWeb.wcApi
{
    public class QrcodeBLL
    {
        /// <summary>
        /// 获取临时二维码的ticket
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="scene_str"></param>
        /// <returns></returns>
        public static string Get_QR_STR_SCENE_Qrcode(string access_token, string scene_str)
        {
            string ticket = "";
            string qrcodeUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            qrcodeUrl = string.Format(qrcodeUrl, access_token);

            var data = new { expire_seconds = 604800, action_name = "QR_STR_SCENE", action_info = new { scene = new { scene_str = scene_str } } };
            var json = JsonConvert.SerializeObject(data);

            string content = CommonBLL.GetInfomation(qrcodeUrl, json);

            if (content.IndexOf("ticket") > -1)
            {
                JObject job = (JObject)JsonConvert.DeserializeObject(content);
                ticket = job["ticket"].ToString();
                ticket = Uri.EscapeDataString(ticket);
            }
            return ticket;
        }

        /// <summary>
        /// 获取临时二维码的ticket
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="scene_id"></param>
        /// <returns></returns>
        public static string GetQrcode(string access_token, int scene_id)
        {
            string ticket = "";
            string qrcodeUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            qrcodeUrl = string.Format(qrcodeUrl, access_token);

            var data = new { expire_seconds = 604800, action_name = "QR_SCENE", action_info = new { scene = new { scene_id = scene_id } } };
            var json = JsonConvert.SerializeObject(data);

            string content = CommonBLL.GetInfomation(qrcodeUrl, json);

            if (content.IndexOf("ticket") > -1)
            {
                JObject job = (JObject)JsonConvert.DeserializeObject(content);
                ticket = job["ticket"].ToString();
                ticket = Uri.EscapeDataString(ticket);
            }
            return ticket;
        }

        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="ticket">二维码的ticket</param>
        /// <returns>返回二维码路径</returns>
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


        /// <summary>
        /// 生成二维码流
        /// </summary>
        /// <param name="ticket">二维码的ticket</param>
        /// <returns>返回生成的二维码流</returns>
        public static Stream GetQrcodeStream(string ticket)
        {
            string Picurl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket + "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Picurl);
            req.Proxy = null;
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:11.0) Gecko/20100101 Firefox/11.0";
            req.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.8,zh-hk;q=0.6,ja;q=0.4,zh;q=0.2");
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            return req.GetResponse().GetResponseStream();
        }
    }
}