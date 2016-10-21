using International.BusinessEntities;
using International.BusinessEntities.Models;
using International.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using International.BusinessLogic.Managers;
using International.Web.Resources;

namespace International.Web.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet, Public]
        public ActionResult Login()
        {
            return View(new UserModel { });
        }

        [HttpPost, Public]
        public ActionResult Login(UserModel user)
        {
            var userDetail = ApplicationContext.UserManager.ValidateUser(user);
            if (userDetail != null)
            {
                if (Request.Cookies.Count > 0)
                {
                    HttpCookie myCookie = Request.Cookies[0];
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(myCookie);
                }
                userDetail.UserRights = ApplicationContext.UserManager.GetUserRights(userDetail.UserId);
                userDetail.UserMenu = ApplicationContext.UserManager.GetUserMenu(userDetail.UserRights);
                userDetail.UserModules = ApplicationContext.UserManager.GetUserModules(userDetail.UserMenu);
                var userToken = ApplicationContext.UserManager.SetCookie(userDetail);
                var cookie = ApplicationContext.UserManager.SaveCoikies(user, userToken, Request.Browser.Browser, Request.Browser.Platform);
                var authCookie = new HttpCookie(cookie);
                authCookie.HttpOnly = true;


                Response.Cookies.Add(authCookie);

                //var usr = LoggedInUser;
                //Session["LoggedInUserName"] = usr.UserName;
                return RedirectToAction("Welcome", "Dashboard");

            }
            else
            {
                ViewBag.ErrorMessage = "Invalid user name or password!";
                return View(user);
            }
        }

        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }

        [HttpGet, Public]
        public ActionResult ForgotPassword()
        {
            return View(new UserModel { });
        }

        [HttpPost, Public]
        public ActionResult ForgotPassword(UserModel user)
        {
            UtilityManager utility = new UtilityManager();
            var userDetail = ApplicationContext.UserManager.ValidateEmail(user);
            if (userDetail != null)
            {
                var newPassword = ApplicationContext.UserManager.ResetPassword(userDetail);
                utility.SendMail(NotificationResource.ForgotPasswordSubject, string.Format(NotificationResource.ForgotPasswordBody, userDetail.FirstName + ' ' + userDetail.LastName, userDetail.Email, newPassword, userDetail.IsActive == false ? "Inactive" : "Active"), userDetail.Email);
                ViewBag.SuccessMessage = NotificationResource.ForgotPasswordSuccessMessage;
              
            }
            else
            {
                ViewBag.ErrorMessage = NotificationResource.ForgotPasswordEmailNotMatched;
            }
            return View();
        }

        [Public]
        public ActionResult SignOut()
        {
            if (Request.Cookies.Count > 0)
            {
                HttpCookie myCookie = Request.Cookies[0];
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            return RedirectToAction("Login", "Home");
        }



        //[HttpGet]
        //public JsonResult Get()
        //{
        //    return Json(ApplicationContext.Master.GetBrokers(), JsonRequestBehavior.AllowGet);
        //}

 



        public ActionResult CreateSubmission()
        {
            return View();
        }
    }
}
