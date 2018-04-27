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

namespace WXProjectWeb.wcApi
{
    public class UserBLL
    {

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="access_token">接口凭证</param>
        /// <param name="openId">普通用户的标识，对当前公众号唯一</param>
        public static UserInfo GetUserDetail(string access_token, string openId)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN",
                   access_token, openId);

            string content = CommonBLL.GetInfomation(url);

            UserInfo user = JsonConvert.DeserializeObject<UserInfo>(content);

            return user;
        }


        public static Stream GetTouxiang(string url)
        {
            System.Net.WebRequest webreq = System.Net.WebRequest.Create(url);
            System.Net.WebResponse webres = webreq.GetResponse();
            System.IO.Stream stream = webres.GetResponseStream();

            return stream;
        }


        public static UserInfo GetUserInfo(string openid)
        {
            using (EFDbContext db = new EFDbContext())
            {
                return db.UserInfos.Where(o => o.openid == openid).FirstOrDefault();
            }
        }

        public static UserInfo SaveUsers(UserInfo user)
        {
            using (EFDbContext db = new EFDbContext())
            {
                var modal = db.UserInfos.Add(user);
                int row = db.SaveChanges();
                return modal;
            }
        }

        public static int ModifyUsers(UserInfo user)
        {
            using (EFDbContext db = new EFDbContext())
            {
                db.UserInfos.Attach(user);
                db.Entry(user).State = EntityState.Modified;
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// 添加关注用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int AddUser(UserInfo user)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserInfo(");
            strSql.Append("openid,subscribe,nickname,sex,language,city,province,country,headimgurl,subscribe_time,count)");
            strSql.Append(" values (");
            strSql.Append("@openid,@subscribe,@nickname,@sex,@language,@city,@province,@country,@headimgurl,@subscribe_time,@count)");
            SqlParameter[] parameters = {
                    new SqlParameter("@openid", SqlDbType.VarChar,500),
                    new SqlParameter("@subscribe", SqlDbType.Int,4),
                    new SqlParameter("@nickname", SqlDbType.VarChar,50),
                    new SqlParameter("@sex", SqlDbType.Int,4),
                    new SqlParameter("@language", SqlDbType.VarChar,50),
                    new SqlParameter("@city", SqlDbType.VarChar,50),
                    new SqlParameter("@province", SqlDbType.VarChar,50),
                    new SqlParameter("@country", SqlDbType.VarChar,50),
                    new SqlParameter("@headimgurl", SqlDbType.VarChar,500),
                    new SqlParameter("@subscribe_time", SqlDbType.BigInt,4),
                    new SqlParameter("@count",SqlDbType.Int,4)};
            parameters[0].Value = user.openid;
            parameters[1].Value = user.subscribe;
            parameters[2].Value = user.nickname;
            parameters[3].Value = user.sex;
            parameters[4].Value = user.language;
            parameters[5].Value = user.city;
            parameters[6].Value = user.province;
            parameters[7].Value = user.country;
            parameters[8].Value = user.headimgurl;
            parameters[9].Value = user.subscribe_time;
            parameters[10].Value = user.count;
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            return rows;
        }

        /// <summary>
        /// 更新用户分享数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int UpdateUser(UserInfo user)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserInfo set ");
            strSql.Append("count=@count");
            strSql.Append(" where openid=@openid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@count", SqlDbType.Int,4),
                    new SqlParameter("@openid",SqlDbType.VarChar,500)};
            parameters[0].Value = user.count;
            parameters[1].Value = user.openid;
            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        public int AddOperateLog(string OpenID, string SharedOpenID)
        {
            int returnVal = 0;

            return returnVal;
        }

        public static ShareCount SaveShareCount(ShareCount shareCount)
        {
            using (EFDbContext db = new EFDbContext())
            {
                var modal = db.ShareCount.Add(shareCount);
                int row = db.SaveChanges();
                return modal;
            }
        }

        public static ShareCount Update(ShareCount shareCount)
        {
            using (EFDbContext db = new EFDbContext())
            {
                var modal = db.ShareCount.Where(o => o.ID == shareCount.ID).FirstOrDefault();
                modal.count = shareCount.count;
                modal.type = shareCount.type;                
                int row = db.SaveChanges();
                return modal;
            }
        }
        public static ShareCount GetUserShareCount(string useropenid, string type)
        {
            using (EFDbContext db = new EFDbContext())
            {
                return db.ShareCount.Where(o => o.openid == useropenid && o.type == type).FirstOrDefault();
            }
        }
        public static ShareCount SaveUserShareCount(string useropenid, string type)
        {
            var model = GetUserShareCount(useropenid, type);
            if (model != null)
            {
                model.count = model.count + 1;
            }
            else
            {
                model = new ShareCount()
                {
                    count = 1,
                    type = type,
                    openid = useropenid
                };
                SaveShareCount(model);

            }
            SaveShareCount(model);

            return model;
        }
    }


    public class EFDbContext : DbContext
    {
        public EFDbContext() : base("name=EFDbContext")
        {
        }
        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<ShareCount> ShareCount { get; set; }

        public DbSet<AutoReply> AutoReply { get; set; }

    }
}