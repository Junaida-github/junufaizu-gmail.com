using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using OnlineUsrToDoLst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using OnlineUsrToDoLst.Models;
using OnlineUsrToDoLst.Controllers;
using System.IO;

namespace OnlineUsrToDoLst.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<UserIdentityUser> userManager;
        private RoleManager<UserIdentityRole> roleManager;
        private OnlineUsersEntities db = new OnlineUsersEntities();

        public AccountController()
        {
            UserIdentityDbContext db = new UserIdentityDbContext();

            UserStore<UserIdentityUser> userStore = new UserStore<UserIdentityUser>(db);
            userManager = new UserManager<UserIdentityUser>(userStore);

            RoleStore<UserIdentityRole> roleStore = new RoleStore<UserIdentityRole>(db);
            roleManager = new RoleManager<UserIdentityRole>(roleStore);

        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {

                //check user existence...
                var user_exit = db.Users.Where(x => x.Email == model.Email).FirstOrDefault();
                if (user_exit == null)  //no user exist
                {
                    UserIdentityUser user = new UserIdentityUser();

                    user.DisplayName = model.DisplayName;
                    user.Email = model.Email;
                    user.Password = model.Password;

                    IdentityResult result = userManager.Create(user, model.Password);


                    //asp.net identity 

                    //if (result.Succeeded)
                    //{
                    //    userManager.AddToRole(user.Id, "Administrator");
                    //    return RedirectToAction("Login", "Account");
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("DisplayName", "Error while creating the user!");
                    //}


                    db.Users.Add(new User()
                    {
                        FullName = model.DisplayName,
                        Email = model.Email,
                        Password = model.Password,

                    });
                    int savecount = db.SaveChanges();
                    if (Request.IsAjaxRequest())
                        return Json(new { error = !(savecount > 0), view = RenderRazorViewToString("Login", null), message = "Registration did Sucessfully..!" });
                    else
                        return PartialView();

                    //return RedirectToAction("Login", "Account");
                }
                else { //user exist..
                    ModelState.AddModelError("Email", "Email Already Exist..");
                    return Json(new { error = true, view = RenderRazorViewToString("Register", model), message = "Email Already Exist.." });
                }

            }
            else
            {
                if (Request.IsAjaxRequest())
                    return Json(new { error = true, view = RenderRazorViewToString("Register", model), message = "Unable To Register Details! Please Check the Values!!!" });
                else
                    return PartialView(model);
                //return View(model);
            }
            
        }


        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                // UserIdentityUser user = userManager.Find(model.Email, model.Password);
                var user = db.Users.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    // IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    // authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    //  ClaimsIdentity identity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    //  AuthenticationProperties props = new AuthenticationProperties();
                    // props.IsPersistent = model.RememberMe;
                    // authenticationManager.SignIn(props, identity);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        Session["UserId"] = user.UserId.ToString();
                         return Json(new { error = false, url = Url.Action("Index", "Home"), message = "" });
                       
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return Json(new { error = true, view = "", message = "Invalid username or password." });
                }
            }
            else
            {
                if (Request.IsAjaxRequest())
                    return Json(new { error = true, view = "", message = "Unable To Login! Please Check the Values!!!" });
                else
                    return PartialView(model);
            }
        }

        public ActionResult RedirectRegistraion()
        {
            return Json(new { error = false, view = RenderRazorViewToString("Register", null), message = "" }, JsonRequestBehavior.AllowGet);
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
