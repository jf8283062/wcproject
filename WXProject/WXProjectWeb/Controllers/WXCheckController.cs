using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WXProjectWeb.wcApi;
using Modal;

namespace WXProjectWeb.Controllers
{
    public class WXCheckController : Controller
    {
        public ActionResult WeiXin()
        {
            //var bgpath = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\" + "button.json";

            //StreamReader sr = new StreamReader(bgpath, Encoding.Default);


            //string jsons = sr.ReadToEnd();
            //

            List<Button> list = ButtonBLL.GetBaseButton();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"button\": [");
            foreach (var item in list)
            {
                sb.Append(" {");
                sb.Append("\"name\": \""+item.name+"\",");
                if (item.type!= "base")
                {
                    sb.Append("\"type\": \"" + item.type + "\",");
                    if (item.type == "view")
                    {
                        sb.Append("\"url\": \"" + item.value + "\"");
                    }
                    else
                    {
                        sb.Append("\"key\": \"" + item.value + "\"");
                    };
                }
                else
                {
                    var x = ButtonBLL.GetSubButton(item.id);
                    List<Newtonsoft.Json.Linq.JObject> subList = new List<Newtonsoft.Json.Linq.JObject>();
                    foreach (var subBtn in x)
                    {
                        Newtonsoft.Json.Linq.JObject subt = new Newtonsoft.Json.Linq.JObject();
                        subt.Add("name", subBtn.name);
                        subt.Add("type", subBtn.type);
                        if (subBtn.type == "view")
                        {
                            subt.Add("url", subBtn.value);
                        }
                        else {
                            subt.Add("key", subBtn.value);
                        };
                        subList.Add(subt);
                    }
                    sb.Append("\"sub_button\":"+JsonConvert.SerializeObject(subList));
                }
                sb.Append("}");
                if (item.id != list.Last().id)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]}");
            var res = CommonBLL.GetInfomation("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + CommonBLL.GetAccess_token(), sb.ToString());
            return Json(res);
        }

    }
}