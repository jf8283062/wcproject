using Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.Entity;
using System.IO;
using Modal;

namespace WXProjectWeb.wcApi
{
    public class AutoReplyBLL
    {


        /// <summary>
        /// 获取回复内容信息
        /// </summary>
        /// <returns></returns>
        public static List<AutoReply> GetBasicServiceInfo()
        {
            List<AutoReply> list = new List<AutoReply>();
            list.Add(new AutoReply() { ID = 1, Question = "1", ReplyContent = "1111" });
            return list;
        }

        /// <summary>
        /// 更新回复内容
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static int UpdateAutoReplyInfo(Modal.AutoReply bs)
        {
            //return dal.UpdateBasicServiceInfo(bs);
           // return SimpleHelp.Update<T_BasicServiceInfo>(bs);
            return 1;
        }

        /// <summary>
        /// 添加回复内容
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static AutoReply AddNewAutoReplyInfo(AutoReply bs)
        {
            //return dal.AddNewBasicServiceInfo(bs);
            // return SimpleHelp.Add(bs);

            return new AutoReply() { ID = 2, Question = "2", ReplyContent = "222" };
        }
    }
}