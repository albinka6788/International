using International.BusinessEntities;
using International.BusinessEntities.Models;
using International.BusinessLogic.Classes;
using International.BusinessLogic.Managers;
using International.Web.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace International.Web.Controllers
{
    public class SubmissionController : BaseController
    {
        //
        // GET: /Submission/

        public ActionResult Index()
        {
            return View();
        }

        [Public]
        public ActionResult ChooseRegion()
        {
            var region = ApplicationContext.Submission.GetUserRegions(ApplicationContext.LoggedInUser.UserId);
            return View(region);
        }

        [Public]
        public JsonResult GetRegionList()
        {
            return Json(ApplicationContext.Submission.GetUserRegions(ApplicationContext.LoggedInUser.UserId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Public]
        public JsonResult SetSubmissionRegion(int regionId)
        {
            var request = HttpContext.Request;
            var region = ApplicationContext.Master.GetRegion(regionId);
            var loggedInUser = ApplicationContext.LoggedInUser;
            loggedInUser.SubmissionRegion = region;
            var authCookie = request.Cookies.AllKeys[0].ToString();
            ApplicationContext.Submission.UpdateCookie(authCookie, loggedInUser);
            return Json("success", JsonRequestBehavior.AllowGet);
        }



        [Public]
        public ActionResult CreateSubmission(Guid? submissionId = null, bool IsView = false, Enums.SubmissionProcess currentProcess = Enums.SubmissionProcess.CreateSubmission)
        {
            SubmissionModel submissionModel = new SubmissionModel() { SubmissionId = submissionId ?? Guid.Empty, CurrentProcess = currentProcess };
            ViewBag.Currentprocess = currentProcess;
            ViewBag.PageTitle = Common.GetDescription((Enums.SubmissionProcess)currentProcess);
            ViewBag.isView = IsView;
            return View(submissionModel);
        }

        [HttpPost]
        [Public]
        public JsonResult SaveSubmission(SubmissionModel submissionModel)
        {
            ApplicationContext.Submission.SaveSubmission(submissionModel, ApplicationContext.LoggedInUser.UserId, ApplicationContext.LoggedInUser);
            ApplicationContext.SaveChanges();
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [Public]
        public ActionResult GetSubmissionModel(Guid? submissionId, Enums.SubmissionProcess currentProcess = Enums.SubmissionProcess.CreateSubmission)
        {
            var submission = ApplicationContext.Submission.GetSubmissionModel(submissionId, currentProcess);
            var list = JsonConvert.SerializeObject(submission, Formatting.None, new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                            });
            return Content(list, "application/json");
            // return Json(submission, JsonRequestBehavior.AllowGet);
        }

        [Public]
        public JsonResult GetSelectionList(Guid? submissionId, Enums.SubmissionProcess currentProcess = Enums.SubmissionProcess.CreateSubmission)
        {
            return Json(ApplicationContext.Submission.GetSelectionList(ApplicationContext.LoggedInUser, submissionId, currentProcess), JsonRequestBehavior.AllowGet);
        }



        public ActionResult Submissions(SubmissionFilters filters)
        {
            if (Request.IsAjaxRequest()) return Json(ApplicationContext.Submission.GetSubmissions(filters, ApplicationContext.LoggedInUser.SubmissionRegion.RegionId), JsonRequestBehavior.AllowGet);
            else return View();
        }

        [Public]
        public JsonResult GetSubmissionFilters()
        {
            return Json(ApplicationContext.Submission.GetSubmissionFilters(), JsonRequestBehavior.AllowGet);
        }

        [Public]
        public JsonResult checkSubmissionNumberExist(string sixDigitNo)
        {
            return Json(ApplicationContext.Submission.checkSubmissionNumberExist(sixDigitNo), JsonRequestBehavior.AllowGet);
        }

        #region QC Submission and QC Amendment

        [Public]
        public ActionResult QCSubmission(SubmissionFilters filters)
        {
            if (Request.IsAjaxRequest()) return Json(ApplicationContext.Submission.GetSubmissionQC(filters, ApplicationContext.LoggedInUser.SubmissionRegion.RegionId), JsonRequestBehavior.AllowGet);
            else return View();
        }



        [Public]
        public ActionResult QCAmendment(SubmissionFilters filters)
        {
            if (Request.IsAjaxRequest()) return Json(ApplicationContext.Submission.GetAmendmentQC(filters, ApplicationContext.LoggedInUser.SubmissionRegion.RegionId), JsonRequestBehavior.AllowGet);
            else return View();
        }


        [HttpPost]
        [Public]
        public JsonResult UpdateQCStatus(Guid SubmissionId, int CurrentStatusId, int? QCStatusId, string QCRemark)
        {
            ApplicationContext.Submission.UpdateQCStatus(SubmissionId, CurrentStatusId, QCStatusId, QCRemark, ApplicationContext.LoggedInUser.UserId, ApplicationContext.LoggedInUser);
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        #endregion


        [Public]
        public JsonResult checkPolicyNumberExist(string PolicyNumber)
        {
            return Json(ApplicationContext.Submission.checkPolicyNumberExist(PolicyNumber), JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Public]
        public JsonResult compareEndorsePremium(PremiumDetailModel premiumModel, Guid ParentSubmissionId)
        {
            return Json(ApplicationContext.Submission.CompareEndorsePremium(premiumModel, ParentSubmissionId), JsonRequestBehavior.AllowGet);
        }

        [Public]
        public JsonResult CreateReversal(Guid submissionId)
        {
            ApplicationContext.Submission.CreateReversal(submissionId, ApplicationContext.LoggedInUser.UserId);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Public]
        public void Export()
        {
            var data = ApplicationContext.Submission.GetExportData(ApplicationContext.LoggedInUser.SubmissionRegion.RegionId);            
            GridView gvDetails = new GridView();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Submission Log " + DateTime.Now.ToString() + ".xls"));
            Response.ContentType = "application/ms-excel";
            //Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            //gvDetails.AllowPaging = false;
            gvDetails.DataSource = data;
            gvDetails.DataBind();
            for (int i = 0; i < gvDetails.HeaderRow.Cells.Count; i++)
            {
                gvDetails.HeaderRow.Cells[i].Style.Add("background-color", "#05A09A");
            }

            foreach (GridViewRow r in gvDetails.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    for (int columnIndex = 0; columnIndex < r.Cells.Count; columnIndex++)
                    {

                        if (columnIndex == 51 || columnIndex == 55 || columnIndex == 74 || columnIndex == 89)
                        {
                            r.Cells[columnIndex].Attributes.Add("class", "Decimal5");
                        }
                        else if (columnIndex == 52 || columnIndex == 53 || columnIndex == 54 || columnIndex == 56 || columnIndex == 57 || columnIndex == 58 || columnIndex == 59 || columnIndex == 60 || columnIndex == 61 || columnIndex == 62
                            || columnIndex == 63 || columnIndex == 64 || columnIndex == 65 || columnIndex == 66 || columnIndex == 67 || columnIndex == 68 || columnIndex == 69 || columnIndex == 70 || columnIndex == 71
                             || columnIndex == 72 || columnIndex == 75 || columnIndex == 76 || columnIndex == 77 || columnIndex == 78 || columnIndex == 79 || columnIndex == 80 || columnIndex == 81
                            || columnIndex == 82 || columnIndex == 83 || columnIndex == 84 || columnIndex == 85 || columnIndex == 86 || columnIndex == 87 || columnIndex == 88 || columnIndex == 89 || columnIndex == 90 || columnIndex == 91 || columnIndex == 92
                            || columnIndex == 93 || columnIndex == 94 || columnIndex == 95 || columnIndex == 96 || columnIndex == 97 || columnIndex == 98 || columnIndex == 99 || columnIndex == 100 || columnIndex == 101 || columnIndex == 102 || columnIndex == 103)
                        {
                            r.Cells[columnIndex].Attributes.Add("class", "Decimal2");
                        }

                        else if (columnIndex == 145 || columnIndex == 146)
                        {
                            r.Cells[columnIndex].Attributes.Add("class", "Decimal7");
                        }

                    }
                }
            }
            gvDetails.RenderControl(htw);
            string style = @"<style> .datett { mso-number-format:mmm\\-dd\\-yyyy; } </style> ";
            style += @"<style> .Decimal5 { mso-number-format:'0\.00000'; } </style> ";
            style += @"<style> .Decimal2 { mso-number-format:'0\.00'; } </style> ";
            style += @"<style> .Decimal7 { mso-number-format:'0\.0000000'; } </style> ";
            Response.Write(style);
            //if (isCount)
            //    Response.Write("<style> TABLE { border:dotted 1px #999; } TD { border:none 0px #D5D5D5; text-align:center } </style>");
            Response.Write(sw.ToString());
            Response.End();
        }

    }
}
