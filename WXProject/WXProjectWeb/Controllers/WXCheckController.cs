using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WXProjectWeb.wcApi;

namespace WXProjectWeb.Controllers
{
    public class WXCheckController : Controller
    {
        public string WeiXin()
        {
            StreamReader sr = new StreamReader("", Encoding.Default);
            string jsons = sr.ReadToEnd();
            var res = CommonBLL.GetInfomation("https://api.weixin.qq.com/cgi-bin/menu/create?access_token="+HomeController.Access_token.FirstOrDefault().Key, jsons);
            return res;
        }

    }
}