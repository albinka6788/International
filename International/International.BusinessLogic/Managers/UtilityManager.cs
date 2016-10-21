using International.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using International.BusinessEntities.Models;
using International.BusinessLogic.Classes;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Configuration;
using System.Net.Http;
using System.Web.Http.Filters;


namespace International.BusinessLogic.Managers
{
    public class UtilityManager
    {
        #region Config

        private InternationalSubmissionEntities context { get; set; }

        public UtilityManager(InternationalSubmissionEntities applicationContext)
        {
            this.context = applicationContext;
        }

        public UtilityManager()
        {
            this.context = new InternationalSubmissionEntities();
        }

        #endregion

        private Response<int> LogError(AppException ex)
        {
            var txtError = new JavaScriptSerializer().Serialize(ex);
            var errorObj = new ErrorLog
            {
                Detail = txtError,
                LoggedAt = DateTime.Now
            };

            try
            {
                this.context.ErrorLogs.Add(errorObj);
                this.context.SaveChanges();
            }
            catch { errorObj.ID = 0; }


            try
            {
                MailMessage msg = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                msg.From = new MailAddress(ConfigurationManager.AppSettings["SmtpUserId"]);
                msg.To.Add("sudeep.sehgal@xceedance.com");
                msg.To.Add("puja.kumari@xceedance.com");
                msg.To.Add("arun.kumar@xceedance.com");
                msg.To.Add("anil.kumar@xceedance.com");

                msg.Subject = string.Format("International {0} Error # {1}", ConfigurationManager.AppSettings["ServerType"], errorObj.ID);
                msg.Body = string.Format("{0}{1}http://json.parser.online.fr/", txtError, Environment.NewLine);

                smtp.Host = ConfigurationManager.AppSettings["SmtpHost"];
                smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["SmtpPort"]);

                smtp.Credentials = new System.Net.NetworkCredential(
                        ConfigurationManager.AppSettings["SmtpUserId"],
                        ConfigurationManager.AppSettings["SmtpPassword"]);

                smtp.EnableSsl = true;
                smtp.Send(msg);
            }
            catch { }

            return new Response<int>
            {
                data = errorObj.ID,
                status = false,
            };
        }

        public Response<int> LogError(ExceptionContext filterContext)
        {
            try
            {
                var ex = filterContext.Exception;
                var request = filterContext.RequestContext.HttpContext.Request;

                var errorDetails = new AppException
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = new List<string>(),
                    ServerVariables = new Dictionary<string, string>(),
                    QueryVariables = new Dictionary<string, string>(),
                    FormVariables = new Dictionary<string, string>()
                };

                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    errorDetails.InnerException.Add(innerEx.Message);
                    innerEx = innerEx.InnerException;
                }

                request.ServerVariables.AllKeys.ToList().ForEach(o => errorDetails.ServerVariables.Add(o, request.ServerVariables[o]));
                request.QueryString.AllKeys.ToList().ForEach(o => errorDetails.QueryVariables.Add(o, request.QueryString[o]));
                request.Form.AllKeys.ToList().ForEach(o => errorDetails.FormVariables.Add(o, request.Form[o]));

                return LogError(errorDetails);
            }
            catch
            {
                return new Response<int>
                {
                    data = -1,
                    status = false,
                };
            }
        }

        public Response<int> LogError(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                var ex = actionExecutedContext.Exception;
                var request = actionExecutedContext.Request;

                var errorDetails = new AppException
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = new List<string>(),
                    ServerVariables = new Dictionary<string, string>(),
                    QueryVariables = new Dictionary<string, string>(),
                    FormVariables = new Dictionary<string, string>()
                };

                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    errorDetails.InnerException.Add(innerEx.Message);
                    innerEx = innerEx.InnerException;
                }

                return LogError(errorDetails);
            }
            catch
            {
                return new Response<int>
                {
                    data = -1,
                    status = false,
                };
            }
        }

        public void SendMail(string Subject, string Body, string To, string CC = null)
        {


            try
            {
                MailMessage msg = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                msg.From = new MailAddress(ConfigurationManager.AppSettings["SmtpUserId"]);
                msg.To.Add(To);
                if (CC != null)
                    msg.CC.Add(CC);
                msg.Subject = Subject;
                msg.Body = Body;

                smtp.Host = ConfigurationManager.AppSettings["SmtpHost"];
                smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["SmtpPort"]);

                smtp.Credentials = new System.Net.NetworkCredential(
                        ConfigurationManager.AppSettings["SmtpUserId"],
                        ConfigurationManager.AppSettings["SmtpPassword"]);

                smtp.EnableSsl = true;
                msg.IsBodyHtml = true;
                smtp.Send(msg);
            }
            catch { }

        }
    }
}
