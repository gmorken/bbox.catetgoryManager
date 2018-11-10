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
        public JsonResult newPost(List<ClublandCategories> data)
        {
            var currentMasterListJson = System.IO.File.ReadAllText(@"d:\masterlist.json");
            var currentMasterList = JsonConvert.DeserializeObject<List<ClublandCategories>>(currentMasterListJson);
            bool masterlistChanged = false;

            //var addedItems = data.Where(i => i.CategoryItems.Any(y => y.Status == 1)).Select(s => s.CategoryItems.ToList());

            foreach (var incomingListItem in data)
            {
                // check for added
                
                // check for none categoryitems here
                // get all added items in this category
                var addedItems = incomingListItem.CategoryItems.Where(s => s.Status == 1);
                if(addedItems.Any())
                {
                    

                    foreach (var addedItem in addedItems)
                    {
                        // get current category in masterlist
                        var masterlistCategory = currentMasterList.Where(m => m.ClublandCategoryName == incomingListItem.ClublandCategoryName).FirstOrDefault();
                        if(masterlistCategory != null)
                        {
                            // make sure we only add the childitem once!
                            if(!masterlistCategory.CategoryItems.Any(item => item.ItemDescription == addedItem.ItemDescription))
                            {
                                addedItem.Status = 0;
                                masterlistCategory.CategoryItems.Add(addedItem);
                                //currentMasterList.Add(masterlistCategory);
                                masterlistChanged = true;
                            }

                            //if(!masterlistCategory.CategoryItems.Contains(addedItem))
                            //{
                            //    // reset status
                            //    addedItem.Status = 0;
                            //    masterlistCategory.CategoryItems.Add(addedItem);
                            //    //currentMasterList.Add(masterlistCategory);
                            //    masterlistChanged = true;
                            //}
                           


                        }
                        //currentMasterList.Add(incomingListItem);

                        // add all added items to masterlist
                        //var cat = new CategoryItems
                        //{
                        //    ItemDescription = addedItem.ItemDescription,
                        //    // set status to zero so that its ok next time
                        //    Status = 0
                        //};
                        // get clublandcategory so that we can add the child
                        //incomingListItem.CategoryItems.ad
                    }
                    
                }
               





                //var itemDesc = incomingListItem.ClublandCategoryName;
                //var categoryExistsInMasterlist = currentMasterList.Where(i => i.ClublandCategoryName == itemDesc).ToList();
                //if(categoryExistsInMasterlist.Any())
                //{
                //    var incomingChilds = incomingListItem.CategoryItems;
                //    var masterListChilds = categoryExistsInMasterlist.First().CategoryItems;

                //    foreach (var item in incomingChilds)
                //    {
                //        var itemExistsInMasterChilds = masterListChilds.Where(s => s.ItemDescription == item.ItemDescription).FirstOrDefault();
                //        if(itemExistsInMasterChilds == null)
                //        {
                //            masterListChilds.Add(item);
                //        }


                //    }
                //}

            }
            if(masterlistChanged)
            {
                var newMasterlistJson = JsonConvert.SerializeObject(currentMasterList);
                System.IO.File.WriteAllText(@"d:\masterlist.json", newMasterlistJson);
            }

            var x = 0;

            //var currentMasterList = new List<ClublandCategories>();

            // Get unique values in childs
            //foreach (var parent in data)
            //{


            //    var childList = parent.childs.Distinct().ToList();
            //    parent.childs = childList;
            //    newMasterCategoryList.Add(parent);
            //    //var uniqueChildren = parent.childs.GroupBy(n => n.)
            //    //foreach (var child in parent.childs)
            //    //{

            //    //}
            //}

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
    public class ClublandCategories
    {
        public string ClublandCategoryName { get; set; }
        public List<CategoryItems> CategoryItems { get; set; }
    }

    public class CategoryItems
    {
        // 0 not modified
        // 1 added
        // -1 removed
        public int Status { get; set; }
        public string ItemDescription { get; set; }

    }
    public class Child2
    {
        public string nameTomas { get; set; }
        public int added { get; set; }
    }

    public class RootObject2
    {
        public string cat { get; set; }
        public List<Child2> childs { get; set; }
    }

}