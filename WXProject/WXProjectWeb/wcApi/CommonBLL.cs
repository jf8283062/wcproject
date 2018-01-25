using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}