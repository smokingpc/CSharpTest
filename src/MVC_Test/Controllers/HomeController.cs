using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Test.Models;

namespace MVC_Test.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.AppName = "Web MVC App...";

            if (Session["Token"]!=null && Session["Token"].Equals(true))
            {
                ViewBag.Message = "LOGGED IN!!!";
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.AppName = "Web MVC App...";
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.AppName = "Web MVC App...";
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.AppName = "Web MVC App...";
            ViewBag.Title = "LOGIN!!";
            ViewBag.Message = "This is my login Page";
            return View();
        }

        [HttpPost]
        public ActionResult LoginSubmit(FormCollection post)
        {
        //每次跳轉route，ViewBag好像就會清空？  所以AppName每次都要設定不然這一頁要顯示的
        //ViewBag內容會沒資料.....
            ViewBag.AppName = "Web MVC App...";
            MyLoginResult data = new MyLoginResult()
            {
                UID = post["UID"],
                PWD = post["PWD"],
            };
            //每個CONTROLLER ROUTE 要有對應的model
            //在render view時塞進去的資料會在CSHTML版面上以 @Model 物件存取
            //所以每個頁面要分別弄一個Data Model當container帶所有資料進去
            //return View(data);

            Session["Token"] = true;
            //Application["Token"]=true;
            //redirect back to Index page
            return RedirectToAction("Index", "Home");
        }
    }
}