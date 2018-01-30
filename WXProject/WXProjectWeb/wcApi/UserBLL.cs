using Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

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

        /// <summary>
        /// 添加关注用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int AddUser(UserInfo user)
        {
            string sql = "";
            sql = string.Format("Insert Into UserInfo (openid,count) Values ('{0}',{1})", user.openid, user.count);
            int count = DbHelperSQL.ExecuteSql( sql);
            return count;
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
            strSql.Append("count=count+1");
            strSql.Append(" where openid=@openid");
            SqlParameter[] parameters = {
                    new SqlParameter("@openid", SqlDbType.VarChar,500)};
            parameters[0].Value = user.openid;
            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        public int AddOperateLog(string OpenID, string SharedOpenID)
        {
            int returnVal = 0;

            return returnVal;
        }
    }
}