using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal
{
    [Table("CouponTable")]
    public class CouponTable
    {

        public int ID { set; get; }


        public string Coupon { get; set; }

        public string HuoDong { set; get; }
    }
}
