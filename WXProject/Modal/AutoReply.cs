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
    /// 自动回复消息
    /// </summary>
    [Table("AutoReply")]
    public class AutoReply
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string Question { get; set; }

        [StringLength(1000)]
        public string ReplyContent { get; set; }

        [StringLength(200)]
        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        [StringLength(200)]
        public string ModifiedBy { get; set; }

        public DateTime ModifiedTime { get; set; }
    }
}
