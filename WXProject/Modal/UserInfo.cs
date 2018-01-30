using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Modal
{

    /// <summary>
    /// 根据OpenID获取用户基本信息，包括昵称、头像、性别、所在城市、语言和关注时间。
    /// </summary>
    [Table("UserInfo")]
    public class UserInfo 
    {
       public int ID { get; set; }
        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public int subscribe { get; set; }

        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        [StringLength(500)]
        public string openid { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        [StringLength(500)]
        public string nickname { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int sex { get; set; }

        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        [StringLength(50)]
        public string language { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        [StringLength(50)]
        public string city { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        [StringLength(50)]
        public string province { get; set; }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        [StringLength(50)]
        public string country { get; set; }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        [StringLength(500)]
        public string headimgurl { get; set; }

        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public long subscribe_time { get; set; }

        /// <summary>
        /// 通过分享二维码后关注用户的数量
        /// </summary>
        public int count { get; set; }
    }
}
