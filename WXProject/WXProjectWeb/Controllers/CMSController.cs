using Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WXProjectWeb.App_Start;
using WXProjectWeb.wcApi;
using System.Runtime.Serialization.Json;

namespace WXProjectWeb.Controllers
{
    public class CMSController : Controller
    {
        public ActionResult Login()
        {
            ViewBag.HasCookies = Request.Cookies["user"] != null ;
            return View();
        }
        
        public JsonResult PostLogin(string username, string password, string remeber)
        {
            if (username == "admin" && password == "admin")
            {
                Response.Cookies.Add(new HttpCookie("user", "admin"));
            }           
            return Json(new
            {
                success = true
            },JsonRequestBehavior.AllowGet);
        }

        [AuthorizeFilterAttribute]
        public ActionResult Index()
        {
            ViewBag.Tab = "button";
            return View();
        }

        [AuthorizeFilterAttribute]
        public ActionResult EditButton()
        {
            ViewBag.Tab = "button";
            var list = ButtonBLL.GetBaseButton();
            ViewBag.List = list;
            return View();
        }

        [AuthorizeFilterAttribute]
        public ActionResult DeleteButton(int id)
        {
            ViewBag.Tab = "button";
            ButtonBLL.Delete(id);
            return Json(new { status = 1 });
        }

        [AuthorizeFilterAttribute]
        public ActionResult EditBaseButton(int id)
        {
            ViewBag.Tab = "button";
            var model = ButtonBLL.Get(id);
            if (model == null)
            {
                model = new Button();
            }
            ViewBag.Entity = model;
            return View();
        }
        [AuthorizeFilterAttribute]
        public ActionResult PostButtonData(int id, string type, string name,string value,int baseid)
        {
            if (id!=0)
            {
                var btn = ButtonBLL.Get(id);
                btn.baseid = baseid;
                btn.value = value;
                btn.type = type;
                btn.name = name;

                ButtonBLL.Modify(btn);
            }
            else
            {
                var btn = new Button();
                btn.baseid = baseid;
                btn.value = value;
                btn.type = type;
                btn.name = name;
                ButtonBLL.Save(btn);
            }
            return Json(new { status = 1 });
        }
        [AuthorizeFilterAttribute]
        public ActionResult EditSubButton(int id)
        {
            var model = ButtonBLL.Get(id);
            var list = ButtonBLL.GetSubButton(id);
            if (model == null)
            {
                return RedirectToAction("EditButton");
            }
            ViewBag.Entity = model;
            ViewBag.List = list;
            return View();
        }
        public ActionResult EditSubButtondetail(int id  = 0 ,int baseid = 0)
        {
            ViewBag.Tab = "button";
            var model = ButtonBLL.Get(id);
            if (model == null)
            {
                model = new Button() { baseid = baseid };
            }
            ViewBag.Entity = model;
            return View();
        }

        #region AutoReply
        [AuthorizeFilterAttribute]
        public ActionResult AutoReply()
        {
            ViewBag.Tab = "自动回复";
            return View();
        }

        /// <summary>
        /// 获取回复命令
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAutoReplyInfo()
        {
            var p = AutoReplyBLL.GetAutoReplyInfo();
            return Json(new { status = 1, data = p }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 更新回复命令
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int UpdateAutoReplyInfo(string data)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
            DataContractJsonSerializer djs = new DataContractJsonSerializer(typeof(AutoReply));
            AutoReply bs = (AutoReply)djs.ReadObject(ms);
            bs.ModifiedTime = DateTime.Now;
            return wcApi.AutoReplyBLL.UpdateAutoReplyInfo(bs);
        }

        /// <summary>
        /// 添加回复命令
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult AddNewAutoReplyInfo(string question, string replaycontent)
        {
            AutoReply bs = new AutoReply();
            bs.Question = question;
            bs.ReplyContent = replaycontent ;
            bs.Flag = 0;
            bs.CreatedTime = DateTime.Now;
            var p = wcApi.AutoReplyBLL.AddNewAutoReplyInfo(bs);
            return Json(new { status = 1, data = p });
        }

        #endregion


        #region
        [AuthorizeFilterAttribute]
        public ActionResult EditResponse()
        {

            ViewBag.Tab = "response";
            var list = AutoResponseBLL.GetBaseResponse();
            ViewBag.List = list;
            return View();

            //ViewBag.Tab = "button";
            //var list = new List<AutoResponse>();
            //list.Add(new AutoResponse() { ID = 1, type="text", Question="question", ReplyContent="ansere1" });
            //list.Add(new AutoResponse() { ID = 1, type = "image", Question = "question", ReplyContent = "ansere2", RoomImgPath= "../img/getpica2.jpg" });
            //list.Add(new AutoResponse() { ID = 1, type = "image", Question = "question", ReplyContent = "ansere2", RoomImgPath = "/img/getpica2.jpg" });
            //ViewBag.List = list;
            //return View();
        }


        [AuthorizeFilterAttribute]
        public ActionResult DeleteResponse(int id)
        {
            ViewBag.Tab = "autoresponse";
            AutoResponseBLL.Delete(id);
            return Json(new { status = 1 });
        }


        [AuthorizeFilterAttribute]
        public ActionResult EditBaseResponse(int id)
        {
            ViewBag.Tab = "autoresponse";
            var model = AutoResponseBLL.Get(id);
            if (model == null)
            {
                model = new AutoResponse();
            }
            ViewBag.Entity = model;
            return View();
        }

        [AuthorizeFilterAttribute]
        public ActionResult PostResponseData(int id, string type, string Question, string ReplyContent, string RoomImgPath)
        {
            if (id != 0)
            {
                var item = AutoResponseBLL.Get(id);
               
                item.ReplyContent = ReplyContent;
                item.type = type;
                item.Question = Question;
                item.RoomImgPath = RoomImgPath;

                AutoResponseBLL.Modify(item);
            }
            else
            {
                var item = new AutoResponse();
                item.RoomImgPath = RoomImgPath;
                item.ReplyContent = ReplyContent;
                item.type = type;
                item.Question = Question;
                AutoResponseBLL.Save(item);
            }
            return Json(new { status = 1 });
        }

        public JsonResult JsonFile()
        {
            string upPath = "/Upload/";  //上传文件路径
            string fileContentType = Request.Files[0].ContentType;
            string name = Request.Files[0].FileName;                  // 客户端文件路径

            FileInfo file = new FileInfo(name);

            string fileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + file.Extension; // 文件名称，当前时间（yyyyMMddhhmmssfff）
            string webFilePath = Server.MapPath(upPath) + fileName;        // 服务器端文件路径
            string FilePath = upPath + fileName;   //页面中使用的路径
            int status = 1;
            try
            {
                Request.Files[0].SaveAs(webFilePath);
            }
            catch (Exception ex)
            {
                status = 0;
            }
            return Json(new { filepath = FilePath, status = status });
        }
        #endregion
    }
}