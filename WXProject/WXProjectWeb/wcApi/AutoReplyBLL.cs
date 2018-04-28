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
        /// 获取所有回复内容信息
        /// </summary>
        /// <returns></returns>
        public static List<AutoReply> GetAutoReplyInfo()
        {
            using (EFDbContext db = new EFDbContext())
            {
                return db.AutoReply.Where(o => o.Flag == 0).ToList();
            }
        }

        /// <summary>
        /// 更新回复内容
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        public static int UpdateAutoReplyInfo(Modal.AutoReply modal)
        {

            using (EFDbContext db = new EFDbContext())
            {
                db.AutoReply.Attach(modal);
                db.Entry(modal).State = EntityState.Modified;
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// 添加回复内容
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        public static AutoReply AddNewAutoReplyInfo(AutoReply modal)
        {
            using (EFDbContext db = new EFDbContext())
            {
                var entiy = db.AutoReply.Add(modal);
                int row = db.SaveChanges();
                return entiy;
            }
        }


        /// <summary>
        /// 查询自动回复的内容
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static string GetContentbyQuestion(string question)
        {
            List<AutoReply> entityList = new List<AutoReply>();
            using (EFDbContext db = new EFDbContext())
            {
                entityList = db.AutoReply.Where(o => o.Question.Contains(question)).ToList();
            }

            string sendMsg = "";
            try
            {
                if (entityList.Count > 0)
                {
                    sendMsg += entityList[0].ReplyContent + "\n";
                }
                else
                {
                    sendMsg = "没有符合问题的内容";
                }

            }
            catch
            {
                sendMsg = "系统异常!!!请稍后查询";
            }
            return sendMsg;
        }
    }
}