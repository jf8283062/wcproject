using Modal;
using Modal.WeiXinEvent;
using Modal.WeiXinRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace WXProjectWeb.wcApi
{
    /// <summary>
    /// 微信帮助类
    /// </summary>
    public class WXMethdBLL
    {
        /// <summary>
        /// 事件列表
        /// </summary>
        public static List<BaseMsg> _queue = new List<BaseMsg>();

        /// <summary>
        /// 微信认证URL
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        /// <returns></returns>
        public static string CheckURL(string signature, string timestamp, string nonce, string echostr)
        {
            var re = CommonBLL.CheckSignature(signature, timestamp, nonce) ? echostr : "";
            return re;
        }

        /// <summary>
        /// 将XML 转入事件列表
        /// </summary>
        /// <param name="xml"></param>
        public static BaseMsg CreateMessage(string xml)
        {
            if (_queue == null)
            {
                _queue = new List<BaseMsg>();
            }
            else if (_queue.Count >= 50)
            {
                _queue = _queue.OrderByDescending(o=>o.CreateTime).ToList();//保留20秒内未响应的消息
            }
            XElement xdoc = XElement.Parse(xml);
            var msgtype = xdoc.Element("MsgType").Value.ToUpper();
            var FromUserName = xdoc.Element("FromUserName").Value;
            var CreateTime = xdoc.Element("CreateTime").Value;
            MsgType type = (MsgType)Enum.Parse(typeof(MsgType), msgtype);

            switch (type)
            {
                case MsgType.TEXT: return CommonBLL.ConvertObj<TextMessage>(xml);
                //case MsgType.IMAGE: return CommonBLL.ConvertObj<ImgMessage>(xml);
                //case MsgType.VIDEO: return CommonBLL.ConvertObj<VideoMessage>(xml);
                //case MsgType.VOICE: return CommonBLL.ConvertObj<VoiceMessage>(xml);
                //case MsgType.LINK:
                //    return CommonBLL.ConvertObj<LinkMessage>(xml);
                //case MsgType.LOCATION:
                //    return CommonBLL.ConvertObj<LocationMessage>(xml);
                case MsgType.EVENT://事件类型
                    {
                        var eventtype = (EventEnum)Enum.Parse(typeof(EventEnum), xdoc.Element("Event").Value.ToUpper());
                        switch (eventtype)
                        {
                            case EventEnum.CLICK:
                                return CommonBLL.ConvertObj<ClickEvent>(xml);
                            //case Event.VIEW: return CommonBLL.ConvertObj<NormalMenuEventMessage>(xml);
                            //case Event.LOCATION: return CommonBLL.ConvertObj<LocationEventMessage>(xml);
                            //case Event.LOCATION_SELECT: return CommonBLL.ConvertObj<LocationMenuEventMessage>(xml);
                            //扫描二维码
                            //case EventEnum.SCAN: return CommonBLL.ConvertObj<ScanEventMessage>(xml);
                            //关注公众号，或者扫描二维码关注
                            case EventEnum.SUBSCRIBE: return CommonBLL.ConvertObj<SubscribeEvent>(xml);
                            //取消关注
                            case EventEnum.UNSUBSCRIBE: return CommonBLL.ConvertObj<UnsubscribeEvent>(xml);
                            //case Event.SCANCODE_WAITMSG: return CommonBLL.ConvertObj<ScanMenuEventMessage>(xml);
                            default:
                                return CommonBLL.ConvertObj<EventBase>(xml);
                        }
                    }
                default:
                    return null;
            }


        }


        /// <summary>
        /// 回复消息
        /// </summary>
        /// <param name="weixinXML"></param>
        public static string ResponseMsg(WXRequestBase request)
        {
            string responseContent = String.Empty;
            switch (request.MsgType)
            {
                case "text":
                    {
                        var x = request as ContentRequest;
                        responseContent = FormatTextXML(request.FromUserName, request.ToUserName, "text", x.Content, null);
                    }
                    break;
                case "image":
                    {
                        var x = request as ImageReuquest;
                        responseContent = FormatTextXML(request.FromUserName, request.ToUserName, "image", null, x.MediaId);
                    }
                    break;
                default:
                    {
                        var x = request as ContentRequest;
                        responseContent = FormatTextXML(request.FromUserName, request.ToUserName, request.MsgType, x.Content, null);
                    }
                    break;
            }
            return responseContent;
        }

        //返回格式化文本XML内容
        private static String FormatTextXML(string fromUserName, string toUserName, string MsgType, string content, string MediaId)
        {

            StringBuilder sb = new StringBuilder();

            //            @"<xml><ToUserName><![CDATA[gh_46aebec19e92]]></ToUserName>
            //<FromUserName><![CDATA[oVWwA0x8AB3fkTdokUxBflTkVIZk]]></FromUserName>
            //<CreateTime>1517205478</CreateTime>
            //<MsgType><![CDATA[text]]></MsgType> <Content><![CDATA[你好！！]]></Content></xml>"

            sb.Append("<xml>");
            sb.Append("<ToUserName><![CDATA[" + toUserName + "]]></ToUserName>");
            sb.Append("<FromUserName><![CDATA[" + fromUserName + "]]></FromUserName>");
            sb.Append("<CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime>");
            sb.Append("<MsgType><![CDATA[" + MsgType + "]]></MsgType>");
            if (!string.IsNullOrEmpty(content))
            {
                sb.Append("<Content><![CDATA[" + content + "]]></Content>");
            }
            if (!string.IsNullOrEmpty(MediaId))
            {
                sb.Append("<Image><MediaId><![CDATA[" + MediaId + "]]></MediaId></Image>");
            }

            sb.Append("</xml>");

            return sb.ToString();
        }

        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

    }
}