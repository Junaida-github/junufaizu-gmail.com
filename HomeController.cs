using OnlineUsrToDoLst.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineUsrToDoLst.Controllers
{
    public class HomeController : Controller
    {
        
        private OnlineUsersEntities db = new OnlineUsersEntities();

        public ActionResult Index()
        {
            var model = (from d in db.ToDos
                 select new ToDo
                        {
                             PKId = d.PKId,
                            Title = d.Title,
                            DueDate = d.DueDate
                        });
             
            //return View(model.ToList());
            return View();
        }

        [HttpPost]

        public ActionResult ToDoSave(ToDos model)
        {
            if (ModelState.IsValid)
            {

                ToDos todo = new ToDos();

                todo.Title = model.Title;
                todo.DueDate = model.DueDate;
                int uId = Convert.ToInt32(Session["UserId"].ToString());
                db.ToDos.Add(new ToDo()
                {
                    Title = model.Title,
                    DueDate = model.DueDate,
                    UserId = uId,

                });
                int savecount = db.SaveChanges();
                if (Request.IsAjaxRequest())
                    return Json(new { error = !(savecount > 0), view = RenderRazorViewToString("Index", db.ToDos.ToList()) });
                else
                    return PartialView();



            }
            else
            {
                if (Request.IsAjaxRequest())
                    return Json(new { error = true, message = "Unable To Save Details!" });
                else
                    return PartialView(model);
            }

        }
       
         #region Common Controller Methos
         public string RenderRazorViewToString(string viewName, object model)
         {
             ViewData.Model = model;
             using (var sw = new StringWriter())
             {
                 var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                          viewName);
                 var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                              ViewData, TempData, sw);
                 viewResult.View.Render(viewContext, sw);
                 viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                 return sw.GetStringBuilder().ToString();
             }
         }
         #endregion
    }
}
