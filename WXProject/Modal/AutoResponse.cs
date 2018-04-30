using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Modal
{
    [Table("AutoResponse")]
    public partial class AutoResponse
    {
        public int ID { get; set; }

        [StringLength(200)]
        public string Question { get; set; }

        [StringLength(500)]
        public string ReplyContent { get; set; }


        [StringLength(500)]
        public string RoomImgPath { get; set; }

        [StringLength(100)]
        public string RoomImgName { get; set; }

        [StringLength(50)]
        public string type { set; get; }

    }
}
