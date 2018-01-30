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

        /// </summary>
        /// 临时素材media_id是可复用的
        /// <param name="access_token"></param>
        /// <param name="type">媒体文件类型</param>
        /// <param name="fileName">文件名</param>
        /// <param name="inputStream">文件输入流</param>
        /// <returns>媒体文件上传后，获取标识media_id	</returns>	</returns>
        public static string UploadMultimedia(string access_token, string Type, string filename, Stream inputStream)
        {
            string result = "";

            string url = "https://api.weixin.qq.com/cgi-bin/media/upload?access_token=" + access_token + "&type=" + Type;
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

                StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", filename));
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

                byte[] bArr = new byte[inputStream.Length];
                inputStream.Read(bArr, 0, bArr.Length);
                inputStream.Seek(0, SeekOrigin.Begin);

                Stream postStream = request.GetRequestStream();
                postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                postStream.Write(bArr, 0, bArr.Length);
                postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                postStream.Close();

                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();

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


        /// </summary>
        /// 临时素材media_id是可复用的
        /// <param name="access_token"></param>
        /// <param name="type">媒体文件类型</param>
        /// <param name="fileName">文件名</param>
        /// <param name="inputStream">文件输入流</param>
        /// <returns>媒体文件上传后，获取标识media_id	</returns>	</returns>
        public static string UploadMultimedia(string access_token, string Type, string filename, byte[] bArr)
        {
            string result = "";

            string url = "https://api.weixin.qq.com/cgi-bin/media/upload?access_token=" + access_token + "&type=" + Type;
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

                StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", filename));
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

                Stream postStream = request.GetRequestStream();
                postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                postStream.Write(bArr, 0, bArr.Length);
                postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                postStream.Close();

                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();

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

        ///
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
                    mywebclient.DownloadFile(strpath, savepath + media_id + ".jpg");

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