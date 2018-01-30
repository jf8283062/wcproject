using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal.WeiXinRequest
{
    /// <summary>
    /// 回复文本消息实体
    /// </summary>
    public class ContentRequest : WXRequestBase
    {
        public ContentRequest()
        {
            base.MsgType = "text";
        }
        /// <summary>
        /// 回复的消息内容（换行：在content中能够换行，微信客户端就支持换行显示）
        /// </summary>
        public string Content { set; get; }


    }
}
