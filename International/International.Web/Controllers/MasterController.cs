using International.BusinessEntities;
using International.Web.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace International.Web.Controllers
{
    public class MasterController : BaseController
    {
        //
        // GET: /Master/

        public ActionResult Index()
        {
            return View();
        }
         [Public]
        public JsonResult stateByCountry(short countryId)
        {
            return Json(ApplicationContext.Master.GetState(countryId, 0), JsonRequestBehavior.AllowGet);
        }

         [Public]
        public JsonResult CityByState(short stateId)
        {
            return Json(ApplicationContext.Master.GetCity(stateId, 0), JsonRequestBehavior.AllowGet);
        }
         [Public]
        public JsonResult SectionCode(short productLineSubTypeId)
        {
            return Json(ApplicationContext.Master.GetSectionCodeList(productLineSubTypeId, 0), JsonRequestBehavior.AllowGet);
        }
         [Public]
        public JsonResult ProfitCode(short sectionCodeId)
        {
            return Json(ApplicationContext.Master.GetProfitCodeList(sectionCodeId, 0), JsonRequestBehavior.AllowGet);
        }
         [Public]
         public JsonResult PolicyTypeList(string attachmentType, int productLineId, int productLineSubTypeId)
        {
            return Json(ApplicationContext.Master.GetPolicyTypeList(attachmentType, productLineId, productLineSubTypeId,0), JsonRequestBehavior.AllowGet);
        }
         [Public]
        public JsonResult CoverageCodeList(short policyTypeId)
        {
            return Json(ApplicationContext.Master.GetCoverageCodeList(policyTypeId, 0), JsonRequestBehavior.AllowGet);
        }

        [Public]
        public JsonResult CurrentStatusList(int currentStatusId, string newRenewalCode , Enums.SubmissionProcess currentProcess)
        {
            return Json(ApplicationContext.Master.GetCurrentStatusList(currentStatusId, newRenewalCode, currentProcess), JsonRequestBehavior.AllowGet);
        }


        #region Underwriter
         [Public]
        public JsonResult GetUnderwriterList(string key)
        {
            return Json(ApplicationContext.Master.GetPCUnderwriterList(key), JsonRequestBehavior.AllowGet);

        }

        [Public]
        public JsonResult GetPCUnderwriterList(string key, int underwriterId = 0)
        {
            //return Json(ApplicationContext.Master.GetPCUnderwriterList(key, ApplicationContext.LoggedInUser.RegionId, underwriterId), JsonRequestBehavior.AllowGet);
            return Json(ApplicationContext.Master.GetPCUnderwriterList(key, ApplicationContext.LoggedInUser.SubmissionRegion!=null ? ApplicationContext.LoggedInUser.SubmissionRegion.RegionId:0, underwriterId), JsonRequestBehavior.AllowGet);

        }
         [Public]
        public JsonResult GetUWProductSubLine(int UnderwriterId, short productLineId)
        {
              return Json(  new 
                            {
                                prductSubLine = ApplicationContext.Master.GetUWProductLineSubType(UnderwriterId, productLineId),
                                formtype = ApplicationContext.Master.GetFormTypeList(productLineId,0)
                            }, JsonRequestBehavior.AllowGet);               
        }
         [Public]
        public JsonResult GetUWProductLine(int UnderwriterId)
        {
            return Json(ApplicationContext.Master.GetUWProductLine(UnderwriterId), JsonRequestBehavior.AllowGet);
        }

         [Public]
         public JsonResult GetProfitCenterOffice(short UnderwriterId)
         {
             return Json(ApplicationContext.Master.GetProfitCentreOffice("", ApplicationContext.LoggedInUser.SubmissionRegion.RegionId), JsonRequestBehavior.AllowGet);
         }

         [Public]
        public JsonResult GetProductSubLine(short productLineId)
        {
            return Json(ApplicationContext.Master.GetProductLineSubType(productLineId, 0), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Insured
         [Public]
         public JsonResult InsuredList(string term, Enums.InsuredSearch type , Guid? insuredPartyId = null)
         {
             var insuredlist = ApplicationContext.Master.GetInsuredList(term, insuredPartyId, type);
             return Json(ApplicationContext.Master.GetInsuredList(term, insuredPartyId, type), JsonRequestBehavior.AllowGet);           

         }

         [Public]
         public JsonResult InsuredAddressList(Guid? insuredPartyId)
         {
             return Json(ApplicationContext.Master.GetInsuredList("",insuredPartyId, Enums.InsuredSearch.InsuredAddress), JsonRequestBehavior.AllowGet);
         }
        #endregion

        #region Broker
         [Public]
        public JsonResult SearchBroker(string key, short productLineId, short productlineSubTypeId, DateTime? EffectiveDate, DateTime? ExpiryDate)
        {
            return Json(ApplicationContext.Master.SearchBroker(key, productLineId, productlineSubTypeId, null, EffectiveDate, ExpiryDate), JsonRequestBehavior.AllowGet);
        }
         [Public]
        public JsonResult SearchBrokerEntity(string key, short productLineId, short productlineSubTypeId, Guid brokerId)
        {        
            return Json(ApplicationContext.Master.SearchBrokerEntity(key, productLineId, productlineSubTypeId, brokerId, null), JsonRequestBehavior.AllowGet);
        }
         [Public]
        public JsonResult SearchBrokerContact(string key, Guid? brokerEntityId)
        {
            return Json(ApplicationContext.Master.SearchBrokerContact(key, brokerEntityId, null), JsonRequestBehavior.AllowGet);
           // return AutoMapper.Mapper.DynamicMap<List<SearchBrokerContact_Model>>(mdmContext.SearchBrokerContact(key, brokerEntityId, ContactPersonId).ToList());
        }

        #endregion Broker


         //[Public]
         //public JsonResult GetPolicySybolList(int ProductLineTypeId)
         //{
         //    return Json(ApplicationContext.Master.GetPolicySymbolList(ProductLineTypeId), JsonRequestBehavior.AllowGet);
         //}

         [Public]
         public JsonResult GetFormTypeList(int ProductLineTypeId)
         {
             return Json(ApplicationContext.Master.GetFormTypeList(ProductLineTypeId, 0), JsonRequestBehavior.AllowGet);
         }
    }
}
