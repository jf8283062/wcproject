using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modal
{    
    /// <summary>
    /// 事件二维码
    /// </summary>
    public class EventBase:BaseMsg
    {  
        /// <summary>
        /// 事件类型，subscribe(订阅)、unsubscribe(取消订阅)
        /// </summary>
        public string Event { get; set; }

    }
}
