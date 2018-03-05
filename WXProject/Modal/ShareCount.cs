using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal
{
    [Table("UserInfo")]
    public class ShareCount
    {
        /// <summary>
        /// id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string openid { set; get; }
        /// <summary>
        /// 项目
        /// </summary>
        public string type { set; get; }
        /// <summary>
        /// 数量
        /// </summary>
        public int count { set; get; }

    }
}
