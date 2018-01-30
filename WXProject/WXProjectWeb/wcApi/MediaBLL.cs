using Newtonsoft.Json;
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
    //接口详见：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1444738726

    /// <summary>
    /// 多媒体文件接口
    /// </summary>
    public class MediaBLL
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
                    JObject jo = (JObject)JsonConvert.DeserializeObject(content);
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


        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="type">媒体文件类型</param>
        /// <param name="fileName">文件名</param>
        /// <param name="inputStream">文件输入流</param>
        /// <returns>media_id</returns>
        public static string UploadTempMedia(string access_token, string type, string fileName, Stream inputStream)
        {
            string result = "";
            var url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", access_token, type.ToString());
            try
            {
                var content = HttpRequestPost(url, "media", fileName, inputStream);
                if (content.IndexOf("media_id") > -1)
                {
                    JObject jo = (JObject)JsonConvert.DeserializeObject(content);
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

        /// <summary>
        /// FORM表单POST方式上传一个多媒体文件
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="typeName"></param>
        /// <param name="fileName"></param>
        /// <param name="fs"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static string HttpRequestPost(string url, string typeName, string fileName, Stream fs, string encoding = "UTF-8")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Timeout = 10000;
            var postStream = new MemoryStream();
            #region 处理Form表单文件上传
            //通过表单上传文件
            string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            try
            {
                var formdata = string.Format(formdataTemplate, typeName, fileName);
                var formdataBytes = Encoding.ASCII.GetBytes(postStream.Length == 0 ? formdata.Substring(2, formdata.Length - 2) : formdata);//第一行不需要换行
                postStream.Write(formdataBytes, 0, formdataBytes.Length);

                //写入文件
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                {
                    postStream.Write(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //结尾
            var footer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            postStream.Write(footer, 0, footer.Length);
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            #endregion

            request.ContentLength = postStream != null ? postStream.Length : 0;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;

                //直接写入流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                postStream.Close();//关闭文件访问
            }
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.GetEncoding(encoding)))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }
        }
    }
}