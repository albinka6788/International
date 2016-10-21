using International.BusinessEntities.Models;
using International.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace International.Web.Controllers
{
    public class UserManagementController : BaseController
    {
        //
        // GET: /UserManagement/

        #region User Section

        public ActionResult Users()
        {
            UserModel userModel = new UserModel();
            return View(userModel);
        }


        [HttpGet, Public]
        public new ActionResult User(int? UserId, bool isView = false)
        {
            ViewBag.isView = isView;
            return View(new UserModel() { UserId = UserId ?? 0 });

        }

        [HttpPost, Public]
        public JsonResult SaveUser(UserModel model)
        {
            var validMsg = ApplicationContext.UserManager.ValidateUsers(model);
            if (!String.IsNullOrEmpty(validMsg))
                return Json(new { status = false, message = validMsg }, JsonRequestBehavior.AllowGet);
            ApplicationContext.UserManager.SaveUser(model, ApplicationContext.LoggedInUser.UserId);
            return Json(new { status = true, message = validMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Public]
        public JsonResult GetUser(int userId)
        {
            return Json(ApplicationContext.UserManager.GetUser(userId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Public]
        public JsonResult GetUsers(string UserName, string Status, string FromDate = "", string ToDate = "")
        {
            UserModel model = new UserModel();
            model.UserName = UserName;
            model.FilterStatus = Status == "All" ? (int?)null : Convert.ToInt32(Status);
            model.FromDate = FromDate == "" ? (DateTime?)null : Convert.ToDateTime(FromDate);
            model.ToDate = ToDate == "" ? (DateTime?)null : Convert.ToDateTime(ToDate);
            return Json(ApplicationContext.UserManager.GetUsers(model), JsonRequestBehavior.AllowGet);
        }


        [HttpGet, Public]
        public Response UpdateUserStatus(int id, bool status)
        {
            ApplicationContext.UserManager.UpdateUserStatus(id, status, ApplicationContext.LoggedInUser.UserId);
            return new Response { status = true, message = "success" };
        }


        #endregion





        #region Group
        public ActionResult Groups()
        {
            GroupFilterModel groupModel = new GroupFilterModel();
            return View(groupModel);
        }

        [HttpGet, Public]
        public ActionResult Group(int? GroupId, bool isView = false)
        {
            ViewBag.isView = isView;
            return View(new GroupModel() { Id = GroupId ?? 0 });

        }

        [HttpPost, Public]
        public JsonResult SaveGroup(GroupModel model)
        {
            var validMsg = ApplicationContext.UserManager.ValidateGroup(model);
            if (!String.IsNullOrEmpty(validMsg))
                return Json(new { status = false, message = validMsg }, JsonRequestBehavior.AllowGet);
            //ApplicationContext.UserManager.SaveUser(model, ApplicationContext.LoggedInUser.UserId);
            ApplicationContext.UserManager.SaveGroup(model, ApplicationContext.LoggedInUser.UserId);
            ApplicationContext.SaveChanges();
            return Json(new { status = true, message = validMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Public]
        public JsonResult GetGroup(int groupId)
        {
            return Json(ApplicationContext.UserManager.GetGroup(groupId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Public]
        public JsonResult GetGroups(string GroupName, string Status, string FromDate = "", string ToDate = "")
        {
            GroupFilterModel model = new GroupFilterModel();
            model.GroupName = GroupName;
            model.Status = Status == "All" ? (int?)null : Convert.ToInt32(Status);
            model.FromDate = FromDate == "" ? (DateTime?)null : Convert.ToDateTime(FromDate);
            model.ToDate = ToDate == "" ? (DateTime?)null : Convert.ToDateTime(ToDate);
            return Json(ApplicationContext.UserManager.GetGroups(model), JsonRequestBehavior.AllowGet);

        }




        [HttpGet, Public]
        public Response UpdateGroupStatus(int id, bool status)
        {
            ApplicationContext.UserManager.UpdateGroupStatus(id, status, ApplicationContext.LoggedInUser.UserId);
            return new Response { status = true, message = "success" };
        }

        #endregion

    }
}
