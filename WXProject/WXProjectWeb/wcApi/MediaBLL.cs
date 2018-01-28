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
    //接口详见：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1444738726

    /// <summary>
    /// 多媒体文件接口
    /// </summary>
    public class Media
    {
        /// <summary>
        /// 临时素材media_id是可复用的
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="Type"></param>
        /// <param name="path"></param>
        /// <returns>媒体文件上传后，获取标识media_id	</returns>
        public static string UploadMultimedia(string access_token, string Type, string path)
        {
            string result = "";

            string url = "https://api.weixin.qq.com/cgi-bin/media/upload?access_token=" + access_token + "&type=" + Type;

            string filepath = path.Replace("/", "\\");

            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                byte[] responseArray = myWebClient.UploadFile(url, filepath);
                string content = System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);
                if (content.IndexOf("media_id") > -1)
                {
                    JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                    result = jo["media_id"].ToString();
                }
                else
                {
                    JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                    result = jo["errmsg"].ToString();
                }
            }
            catch (Exception ex)
            {
                result = "Error:" + ex.Message;
            }

            return result;
        }

        public static bool GetMultimedia(string access_token, string media_id, string savepath)
        {
            string file = string.Empty;
            string content = string.Empty;
            string strpath = string.Empty;
            string url = "https://api.weixin.qq.com/cgi-bin/media/get?access_token=" + access_token + "&media_id=" + media_id;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                strpath = myResponse.ResponseUri.ToString();

                WebClient mywebclient = new WebClient();

                try
                {

                    mywebclient.DownloadFile(strpath, savepath + media_id + ".amr");

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
 
    }
}