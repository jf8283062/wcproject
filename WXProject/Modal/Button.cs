using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal
{
    [Table("Button")]
    public class Button
    {
        public int id { set; get; }

        public string name { set; get; }

        public string type { set; get; }

        public string value { set; get; }

        public int baseid { set; get; }
        
    }
}
