using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal.WeiXinRequest
{
    /// <summary>
    /// 微信请求类
    /// </summary>
    public class WXRequestBase
    {
        /// <summary>
        /// 接收方帐号（收到的OpenID）
        /// </summary>
        public string ToUserName { set; get; }
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { set; get; }
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public string CreateTime
        {
            get {
                return (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds.ToString();
            }           
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get { return "image"; } }
    }
}
