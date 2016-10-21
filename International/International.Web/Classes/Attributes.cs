using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using International.BusinessEntities;
using System.Web.Security;
using System.Web.Script.Serialization;
using International.BusinessEntities.Models;
using System.Web.Http;
using International.Web.Controllers;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Web.Http.Filters;
using International.BusinessLogic.Managers;

namespace International.Web.Classes
{

    #region Web Controller Attrubutes

    public class SecuredWebController : System.Web.Mvc.AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var controller = (BaseController)filterContext.Controller;
            controller.ApplicationContext.LoggedInUser = new LoggedInUser { IsAuthenticated = false };

            var isCookie = request.Cookies.AllKeys.Count();
            if (isCookie > 0)
            {
               
                    var authCookie = request.Cookies.AllKeys[0].ToString();
                    try
                    {
                        var CurrentSession = controller.ApplicationContext.UserManager.GetSession(authCookie);
                        //var ticket = FormsAuthentication.Decrypt(CurrentSession);
                        if (CurrentSession != string.Empty)
                        {
                            var loggedInUser = new JavaScriptSerializer().Deserialize<LoggedInUser>(CurrentSession);
                            if (loggedInUser.ApplicationKey == ApplicationConstants.ApplicationKey)
                            {
                                controller.ViewBag.UserToken = authCookie;
                                controller.ApplicationContext.LoggedInUser = loggedInUser;

                                bool IsView = false;
                                var queryStrings = request.QueryString;
                                if (queryStrings.Count > 0)
                                    if (queryStrings["IsView"] != null && queryStrings["IsView"].ToString().ToLower() == "true")
                                    {
                                        IsView = true;
                                    }
                                if (!filterContext.ActionDescriptor.GetCustomAttributes(false).Any(o => o.GetType() == typeof(Public)))
                                {
                                    var urlHelper = new UrlHelper(filterContext.RequestContext);
                                    bool isPermitted = false;
                                    foreach (var items in loggedInUser.UserRights)
                                    {
                                        if (items.Controller.ToLower() == filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower() && items.Action.ToLower() == filterContext.ActionDescriptor.ActionName.ToLower() && items.Rights > (int)Enums.Right.None)
                                        {
                                            isPermitted = true;
                                            if (items.Rights < items.MinRight)
                                            {
                                                if (items.Rights == (int)Enums.Right.View && IsView == false)
                                                    filterContext.Result = new RedirectResult(urlHelper.Action("welcome", "Dashboard"));
                                            }

                                        }
                                    }
                                    if (!isPermitted && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower() != "dashboard")
                                    {
                                        filterContext.Result = new RedirectResult(urlHelper.Action("welcome", "Dashboard"));
                                    }
                                }

                            }
                        }

                    }

                    catch { }
                
            }

            if (!(controller.ApplicationContext.LoggedInUser ?? new LoggedInUser { IsAuthenticated = false }).IsAuthenticated)
            {
                if (!filterContext.ActionDescriptor.GetCustomAttributes(false).Any(o => o.GetType() == typeof(Public)))
                {
                    if (request.IsAjaxRequest())
                    {
                        controller.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        controller.Response.StatusDescription = "Request could not be authenticated!";
                        filterContext.Result = new EmptyResult();
                    }
                    else
                    {
                        var urlHelper = new UrlHelper(filterContext.RequestContext);
                        filterContext.Result = new RedirectResult(urlHelper.Action("login", "home"));
                    }
                }
            }

            controller.ViewBag.LoggedInUser = controller.ApplicationContext.LoggedInUser;
            controller.ViewBag.UserName = controller.ApplicationContext.LoggedInUser.UserName;
        }
    }

    public class WebControllerExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            if (filterContext.Controller.GetType().IsSubclassOf(typeof(BaseController)))
            {
                var controller = ((BaseController)filterContext.Controller);
                var errorObj = controller.ApplicationContext.Utilities.LogError(filterContext);

                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    controller.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    controller.Response.StatusDescription = string.Format("Unexpected Error # {0}", errorObj.data);
                    filterContext.Result = new EmptyResult();
                }
                else
                {
                    controller.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    controller.Response.StatusDescription = string.Format("Unexpected Error # {0}", errorObj.data);
                    filterContext.Result = new EmptyResult();
                }
            }

            filterContext.ExceptionHandled = true;
        }
    }

    #endregion

    #region Common Attributes

    public class Public : Attribute { }

    #endregion
}
