using Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WXProjectWeb.wcApi
{
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
        public static EventBase CreateMessage(string xml)
        {
            if (_queue == null)
            {
                _queue = new List<BaseMsg>();
            }
            else if (_queue.Count >= 50)
            {
                _queue = _queue.Where(q => { return q.CreateTime.AddSeconds(20) > DateTime.Now; }).ToList();//保留20秒内未响应的消息
            }
            XElement xdoc = XElement.Parse(xml);
            var msgtype = xdoc.Element("MsgType").Value.ToUpper();
            var FromUserName = xdoc.Element("FromUserName").Value;
            var MsgId = xdoc.Element("MsgId").Value;
            var CreateTime = xdoc.Element("CreateTime").Value;
            MsgType type = (MsgType)Enum.Parse(typeof(MsgType), msgtype);
            if (type == MsgType.EVENT)
            {
                if (_queue.FirstOrDefault(m => { return m.MsgFlag == CreateTime; }) == null)
                {
                    _queue.Add(new BaseMsg
                    {
                        CreateTime = DateTime.Now,
                        FromUser = FromUserName,
                        MsgFlag = CreateTime
                    });
                }
                else
                {
                    return null;
                }
            }
            switch (type)
            {
                //case MsgType.TEXT: return CommonBLL.ConvertObj<TextMessage>(xml);
                //case MsgType.IMAGE: return CommonBLL.ConvertObj<ImgMessage>(xml);
                //case MsgType.VIDEO: return CommonBLL.ConvertObj<VideoMessage>(xml);
                //case MsgType.VOICE: return CommonBLL.ConvertObj<VoiceMessage>(xml);
                //case MsgType.LINK:
                //    return CommonBLL.ConvertObj<LinkMessage>(xml);
                //case MsgType.LOCATION:
                //    return CommonBLL.ConvertObj<LocationMessage>(xml);
                case MsgType.EVENT://事件类型
                    {
                        var eventtype = (Event)Enum.Parse(typeof(Event), xdoc.Element("Event").Value.ToUpper());
                        switch (eventtype)
                        {
                            //case Event.CLICK:
                            //    return CommonBLL.ConvertObj<NormalMenuEventMessage>(xml);
                            //case Event.VIEW: return CommonBLL.ConvertObj<NormalMenuEventMessage>(xml);
                            //case Event.LOCATION: return CommonBLL.ConvertObj<LocationEventMessage>(xml);
                            //case Event.LOCATION_SELECT: return CommonBLL.ConvertObj<LocationMenuEventMessage>(xml);
                            case Event.SCAN: return CommonBLL.ConvertObj<ScanEventMessage>(xml);
                            //case Event.SUBSCRIBE: return CommonBLL.ConvertObj<SubEventMessage>(xml);
                            //case Event.UNSUBSCRIBE: return CommonBLL.ConvertObj<SubEventMessage>(xml);
                            //case Event.SCANCODE_WAITMSG: return CommonBLL.ConvertObj<ScanMenuEventMessage>(xml);
                            default:
                                return CommonBLL.ConvertObj<EventBase>(xml);
                        }
                    }
                default:
                    return CommonBLL.ConvertObj<EventBase>(xml);
            }
        }


    }
}