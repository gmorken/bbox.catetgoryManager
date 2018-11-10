using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            var json = System.IO.File.ReadAllText(@"d:\masterlist.json");
            var icamasterlist = JsonConvert.DeserializeObject<List<RootObject>>(json);

            ViewBag.masterlist = json;
            return View();
            //syns?
            // syns2?
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

            var newMasterCategoryList = new List<RootObject>();

            // Get unique values in childs
            foreach (var parent in data)
            {
                

                var childList = parent.childs.Distinct().ToList();
                parent.childs = childList;
                newMasterCategoryList.Add(parent);
                //var uniqueChildren = parent.childs.GroupBy(n => n.)
                //foreach (var child in parent.childs)
                //{

                //}
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult uploadXml(HttpPostedFileBase xmlfile)
        {
            string xml = string.Empty;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/xml/"), fileName);
                    file.SaveAs(path);
                    xml = System.IO.File.ReadAllText(path);
                }
            }
            var json = System.IO.File.ReadAllText(@"d:\masterlist.json");
            var icamasterlist = JsonConvert.DeserializeObject<List<RootObject>>(json);

            ViewBag.masterlist = json;
            return Content(xml);
            //return null;// RedirectToAction("UploadDocument");
        }
    }
    public class RootObject
    {
        public string cat { get; set; }
        public List<string> childs { get; set; }
    }
}