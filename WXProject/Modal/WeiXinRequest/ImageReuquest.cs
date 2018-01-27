using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal.WeiXinRequest
{
    /// <summary>
    /// 返回图片实体
    /// </summary>
    public class ImageReuquest : WXRequestBase
    {
        public ImageReuquest()
        {
            base.MsgType = "image";
        }
        /// <summary>
        /// 通过素材管理中的接口上传多媒体文件，得到的id。
        /// </summary>
        public string MediaId { set; get; }
    }
}
