using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bbox.catetgoryManager.Controllers
{
    public class HomeController : Controller
    {
        // test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ContentResult getXmlData()
        {
            string xml = System.IO.File.ReadAllText(@"c:\Temp\ica.xml");
            return Content(xml);
        }
        [HttpPost]
        public JsonResult xmlpost(List<RootObject> data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
    public class RootObject
    {
        public string cat { get; set; }
        public List<string> childs { get; set; }
    }
}