using International.BusinessEntities;
using International.BusinessEntities.Models;
using International.Entities;
using International.Entities.MDMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using International.BusinessLogic.Classes;
using System.Data.Entity.Core.Objects;
using AutoMapper;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Data;


namespace International.BusinessLogic.Managers
{
    public class SubmissionManager : BaseManager
    {
        private MasterManager masterManager { get; set; }
                
        public SubmissionManager(InternationalSubmissionEntities Context, MDMEntities mdmContext)
            : base(Context, mdmContext)
        {
            //  this.masterManager = new MasterManager(Context, mdmContext);
        }
        
        /// <summary>
        /// This methed is going to be used for get all master data ......
        /// </summary>
        /// <param name="loggedInUser"></param>
        /// <param name="submissionId"></param>
        /// <param name="currentProcess"></param>
        /// <returns></returns>
        public SelectionListModel GetSelectionList(LoggedInUser loggedInUser, Guid? submissionId = null, Enums.SubmissionProcess currentProcess = Enums.SubmissionProcess.CreateSubmission)
        {
            var selectionListModel = new SelectionListModel();
            selectionListModel.NewRenewalList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.NewRenewal.ToString(), "");
            selectionListModel.RenewableList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.Renewable.ToString(), "");
            selectionListModel.AttachmentTypeList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.AttachmentType.ToString(), "");
            selectionListModel.CountryList = ApplicationContext.Master.GetCountry(0);
            selectionListModel.DirectAssumedList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.DirectAssumed.ToString(), "");
            selectionListModel.AssumedEntityList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.AssumedEntity.ToString(), "");            
            selectionListModel.IncumbentAttackingBrokerList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.BrokerAttribute.ToString(), "");
            selectionListModel.ProfitCentreOfficeList = ApplicationContext.Master.GetProfitCentreOffice("", loggedInUser.SubmissionRegion.RegionId);
            selectionListModel.ReasonList = ApplicationContext.Master.GetReasonCodes(null);
            selectionListModel.AdmittedNotAdmittedList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.AdmittedNotAdmitted.ToString(), "");
            selectionListModel.CompanyPaperList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.CompanyPaper.ToString(), "");
            selectionListModel.CompanyPaperNumberList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.CompanyPaperNumber.ToString(), "");
            selectionListModel.PolicySymbolList = ApplicationContext.Master.GetPolicySymbolList(loggedInUser.SubmissionRegion.RegionId);
            selectionListModel.SuffixCodeList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.Suffix.ToString(), "");
            selectionListModel.OriginalCurrencyList = ApplicationContext.Master.GetOrginalCurrencyList("", loggedInUser.SubmissionRegion.RegionId);
            selectionListModel.TransactionalCurrencyList = ApplicationContext.Master.GetTransactionalCurrencyList("", loggedInUser.SubmissionRegion.RegionId);
            selectionListModel.JurCurrencyList = ApplicationContext.Master.GetJurCurrencyList("", loggedInUser.SubmissionRegion.RegionId);

            selectionListModel.AffliationList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.Affiliations.ToString(), "");
            selectionListModel.MarketSegmentList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.MarketSegment.ToString(), "");
            selectionListModel.OffOnShoreList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.OffshoreOnshore.ToString(), "");
            //selectionListModel.SubmissionCoverageList = ApplicationContext.Master.GetTypeMaster(Enums.Typecategory.OffshoreOnshore.ToString(), "");

            if (submissionId != null && submissionId != Guid.Empty)
            {
                if (currentProcess == Enums.SubmissionProcess.CreateAmendment)
                {
                    submissionId = GetCurrentSubmissionId(submissionId);
                }
                var submission = Context.Submissions
                                .Include("ProjectDetail")
                                .Where(x => x.SubmissionId == submissionId).FirstOrDefault();
                if (submission != null)
                {
                    selectionListModel.ProductLine = ApplicationContext.Master.GetUWProductLine(submission.ProfitCenterUnderWriterId, submission.ProductLineTypeId);
                    selectionListModel.ProductLineSubType = ApplicationContext.Master.GetUWProductLineSubType(submission.ProfitCenterUnderWriterId, submission.ProductLineTypeId);
                    selectionListModel.SectionCodeList = ApplicationContext.Master.GetSectionCodeList(submission.ProductLineSubTypeId, submission.SectionCodeId);
                    selectionListModel.ProfitCodeList = ApplicationContext.Master.GetProfitCodeList(submission.SectionCodeId, submission.ProfitCodeId);

                    selectionListModel.PolicyTypeList = ApplicationContext.Master.GetPolicyTypeList(submission.AttachmentTypeCode, submission.ProductLineTypeId, submission.ProductLineSubTypeId, 0);

                    selectionListModel.CoverageCodeList = ApplicationContext.Master.GetCoverageCodeList(submission.PolicyTypeID ?? 0, submission.CoverageID ?? 0);
                    selectionListModel.LayerLevelCoverageCodeList = ApplicationContext.Master.GetLayerLevelCoverageCodeList(submission.CoverageID ?? 0);
                    selectionListModel.StatusList = ApplicationContext.Master.GetCurrentStatusList(submission.CurrentStatusId, "", currentProcess);
                    var project = submission.ProjectDetail ?? new ProjectDetail();
                    if (project != null)
                    {
                        selectionListModel.StateList = ApplicationContext.Master.GetState(project.CountryId ?? 0, project.StateId ?? 0);
                        selectionListModel.CityList = ApplicationContext.Master.GetCity(project.StateId ?? 0, project.CityId);
                    }

                    if (submission.DomicileStateId != null && submission.DomicileStateId>0)
                    {
                        var domacileCountryId = mdmContext.States.Where(x => x.StateId == submission.DomicileStateId).FirstOrDefault().CountryId;
                        selectionListModel.DomacileStateList = ApplicationContext.Master.GetState(domacileCountryId, submission.DomicileStateId);
                    }
                    selectionListModel.FormTypeList = ApplicationContext.Master.GetFormTypeList(submission.ProductLineTypeId, 0);
                }

            }

            Extensions.SelectionListModels = selectionListModel;
            return selectionListModel;
        }


        public SubmissionModel GetProjectDetails(SubmissionModel submissionModel)
        {
            //submissionModel.cou = masterManager.GetProductLine(submissionModel.ProductLineTypeId);
            return submissionModel;
        }



        #region Save Submission

        /// <summary>
        /// This methed is going to use for save submission model data in case of insert new record and update.....
        /// </summary>
        /// <param name="submissionModel"></param>
        /// <param name="userId"></param>
        /// <param name="loggedInUser"></param>
        public void SaveSubmission(SubmissionModel submissionModel, int userId, LoggedInUser loggedInUser)
        {
            var regionCode = loggedInUser.SubmissionRegion.RegionCode;
            var submissionHeader = Context.SubmissionHeaders.Include("Submissions")
                .Where(x => x.SubmissionHeaderId == submissionModel.SubmissionHeaderId).FirstOrDefault();

            var processIdentifier = Guid.NewGuid();

            submissionModel.ProcessIdentifier = processIdentifier;
            submissionModel.CedantName = submissionModel.CedantName.RemoveExtraWhiteSpace().Trim();
            submissionModel.QCStatusId = (int)Enums.QCStatus.Pending;
            submissionModel.LastModifedOnDate = DateTime.UtcNow;

            if (submissionHeader == null)
            {
                var submission = NewSubmission(submissionModel, userId, 1, regionCode, loggedInUser.SubmissionRegion.RegionId);

                submissionHeader = new SubmissionHeader
                {
                    SubmissionHeaderId = Guid.NewGuid(),
                    Submissions = new List<Submission>() { submission },
                    CreatedOnDate = DateTime.UtcNow,
                    CreatedByUserId = userId,
                    SubmissionNumber = submission.SubmissionNumber,
                    SourceSystemName = ApplicationConstants.SourceSystemName,
                    ProcessIdentifier = submissionModel.ProcessIdentifier
                };
                Context.SubmissionHeaders.Add(submissionHeader);
            }
            else
            {

                submissionHeader.ProcessIdentifier = processIdentifier;
                var submission = Context.Submissions.Include("ProjectDetail")
                    .Include("SubmissionCoverages")
                    .Include("AdditionalCedants")
                    .Include("AdditionalInsureds")
                    .Include("PremiumDetail")
                    .Include("PolicyDetail")
                    .Include("LayerDetails")
                    .Include("LayerDetails.CoverageDetails")
                    .Include("LayerDetails.CoverageDetails.PremiumDetail")
                    .Where(x => x.SubmissionId == submissionModel.SubmissionId).FirstOrDefault();

                if (submission == null || submissionModel.CurrentProcess == Enums.SubmissionProcess.CreateAmendment)
                {
                    var immediateParentId = submissionModel.SubmissionId;
                    submission = NewSubmission(submissionModel, userId, submissionHeader.Submissions.Count + 1, regionCode, loggedInUser.SubmissionRegion.RegionId);
                    submission.ImmediateParentId = immediateParentId;
                    submissionHeader.Submissions.Add(submission);
                }
                else
                {
                    submissionModel.LastModifedOnDate = DateTime.UtcNow;

                    if (submissionModel.PolicyDetail != null) submissionModel.PolicyDetail.CreatedByUserId = userId;
                    if (submissionModel.PremiumDetail != null) submissionModel.PremiumDetail.CreatedByUserId = userId;

                    //  Mapper.DynamicMap(submissionModel, submission);
                    submission = SubmissionMap(submissionModel, submission, userId);

                }


            }


        }
               

        public void UpdateQCStatus(Guid SubmissionId, int CurrentStatusId, int? QCStatusId, string QCRemark, int userId, LoggedInUser loggedInUser)
        {
            Context.USP_UpdateQCStatus(SubmissionId, CurrentStatusId, QCStatusId, QCRemark, DateTime.UtcNow, (int?)userId);
        }

        public Submission NewSubmission(SubmissionModel submissionModel, int userId, int submissionCount, string regionCode, short RegionId)
        {
            var submission = new Submission();
            submissionModel.SixDigitSubmissionNumber =submissionModel.SixDigitNumber;

            if (submissionModel.CurrentProcess == Enums.SubmissionProcess.CreateAmendment)
            {
                int i = submissionModel.SubmissionNumber.LastIndexOf("-");
                if (i >= 0)
                    submissionModel.SubmissionNumber = submissionModel.SubmissionNumber.Substring(0, i + 1) + (submissionCount.ToString().Length < 2 ? submissionCount.ToString().PadLeft(2, '0') : submissionCount.ToString());
                submissionModel.PolicyId = null;
                submissionModel.PremiumId = null;
                var submissionAmendment = new Submission();
                submissionAmendment = SubmissionPolicyDetailMap(submissionModel, submissionAmendment, userId);
            }
            else
            {
                submissionModel.SubmissionNumber = string.Format("{0}-{1}-{2}-{3}-{4}",
                                                                    DateTime.Today.Year.ToString().Substring(Math.Max(0, DateTime.Today.Year.ToString().Length - 2)),
                                                                 DateTime.Today.Month.ToString().Length < 2 ? DateTime.Today.Month.ToString().PadLeft(2, '0') : DateTime.Today.Month.ToString(),
                                                                 regionCode.Length < 2 ? regionCode.PadLeft(2, '0') : regionCode.Substring(0, 2),
                                                                    submissionModel.SixDigitNumber,
                                                                    submissionCount.ToString().Length < 2 ? submissionCount.ToString().PadLeft(2, '0') : submissionCount.ToString());
            }

            submissionModel.RegionId = RegionId;
            submissionModel.SubmissionId = Guid.NewGuid();
            submissionModel.CreatedOnDate = DateTime.UtcNow;
            submissionModel.CreatedByUserId = userId;
            submissionModel.SubmissionCoverages = null;
            submission = AutoMapper.Mapper.DynamicMap<Submission>(submissionModel);

            submissionModel.SelectedSubmissionCoverageList.ToList().ForEach(c =>
            {
                var SubmissionCoverage = new SubmissionCoverage()
                {
                    SubmissionId = submissionModel.SubmissionId,
                    CoverageId = c.id,
                    CreatedOnDate = DateTime.UtcNow,
                    CreatedByUserId = userId,
                    ProcessIdentifier = submissionModel.ProcessIdentifier

                };
                submission.SubmissionCoverages.Add(SubmissionCoverage);
            });

            if (submission.PremiumDetail != null)
            {
                submission.PremiumDetail.CreatedOnDate = DateTime.UtcNow;
                submission.PremiumDetail.CreatedByUserId = userId;
                submission.PremiumDetail.ProcessIdentifier = submissionModel.ProcessIdentifier;
            }

            if (submission.ProjectDetail != null)
            {
                submission.ProjectDetail.CreatedOnDate = DateTime.UtcNow;
                submission.ProjectDetail.CreatedByUserId = userId;
                submission.ProjectDetail.ProcessIdentifier = submissionModel.ProcessIdentifier;
                submission.ProjectDetail.ProjectName = submission.ProjectDetail.ProjectName.RemoveExtraWhiteSpace().Trim();
                submission.ProjectDetail.GeneralContractorName = submission.ProjectDetail.GeneralContractorName.RemoveExtraWhiteSpace().Trim();
                submission.ProjectDetail.ProjectOwnerName = submission.ProjectDetail.ProjectOwnerName.RemoveExtraWhiteSpace().Trim();
                submission.ProjectDetail.ProjectStreetAddress = submission.ProjectDetail.ProjectStreetAddress.RemoveExtraWhiteSpace().Trim();
            }
            else
            {
                submission.ProjectDetail = new ProjectDetail();
                submission.ProjectDetail.CreatedOnDate = DateTime.UtcNow;
                submission.ProjectDetail.CreatedByUserId = userId;
                submission.ProjectDetail.ProcessIdentifier = submissionModel.ProcessIdentifier;
            }
            submission.AdditionalCedants.ToList().ForEach(x =>
            {
                x.CreatedOnDate = DateTime.UtcNow; x.CreatedByUserId = userId;
                x.ProcessIdentifier = submissionModel.ProcessIdentifier;
                x.CedantName = x.CedantName.RemoveExtraWhiteSpace().Trim();
            });

            submission.AdditionalInsureds.ToList().ForEach(x =>
            {
                x.CreatedOnDate = DateTime.UtcNow;
                x.CreatedByUserId = userId;
                x.ProcessIdentifier = submissionModel.ProcessIdentifier;
                x.InsuredName = x.InsuredName.RemoveExtraWhiteSpace().Trim();
            });

            if (submissionModel.CurrentProcess == Enums.SubmissionProcess.CreateAmendment)
            {
                submission.ParentSubmissionId = null;
                submissionModel.PolicyId = null;
                submissionModel.PremiumId = null;
                var submissionAmendment = new Submission();
                submissionAmendment = SubmissionPolicyDetailMap(submissionModel, submissionAmendment, userId);
                submissionAmendment = SubmissionPremiumMap(submissionModel, submissionAmendment, userId);
                submission.PremiumDetail = submissionAmendment.PremiumDetail;
                submission.PolicyDetail = submissionAmendment.PolicyDetail;
                submission.LayerDetails = new List<LayerDetail>();
                if (submissionModel.LayerDetails.Count > 0)
                {
                    submissionModel.LayerDetails.ForEach(x =>
                    {
                        var layer = new LayerDetail();
                        layer.LayerName = x.LayerName;

                        layer.CreatedByUserId = userId;
                        layer.CreatedOnDate = DateTime.UtcNow;

                        x.CoverageDetails.ForEach(o =>
                        {
                            var coverageDetail = new CoverageDetail();
                            coverageDetail.CoverageId = o.CoverageId;
                            coverageDetail.PremiumDetail = new PremiumDetail();
                            coverageDetail.PremiumDetail = PremiumMap(o.PremiumDetail, new PremiumDetail(), userId);
                            coverageDetail.PremiumDetail.CreatedOnDate = DateTime.UtcNow;
                            coverageDetail.PremiumDetail.CreatedByUserId = userId;
                            o.LayerId = null;
                            o.PremiumId = null;
                            layer.CoverageDetails.Add(coverageDetail);
                        });
                        submission.LayerDetails.Add(layer);
                    });
                }
            }

            return submission;
        }

        public Submission SubmissionMap(SubmissionModel submissionModel, Submission submission, int userId)
        {
            //SubmissionHistoryMap(submissionModel, submission,1);
            submission = SubmissionDetailMap(submissionModel, submission); 
            submission = SubmissionProjectMap(submissionModel, submission, userId);
            submission = SubmissionAdditionalInsuredMap(submissionModel, submission, userId);
            submission = SubmissionAdditionalCedantMap(submissionModel, submission, userId);
            submission = SubmissionPremiumMap(submissionModel, submission, userId);
            submission = SubmissionCoverageLevelPremiumMap(submissionModel, submission, userId);        
            submission = SubmissionPolicyDetailMap(submissionModel, submission, userId);
            submission = SubmissionCoverageMap(submissionModel, submission, userId);            
            submission.LastModifedOnDate = DateTime.UtcNow;
            submission.LastModifiedByUserId = userId;
            return submission;
        }

        //public void SubmissionHistoryMap(SubmissionModel submissionModel, Submission submission, int userId)
        //{
        //    string[] IgnoreableProperties = { "ProcessIdentifier", "LastModifedOnDate", "AdditionalCedants", "AdditionalCedants", "AdditionalInsureds", "LayerDetails",
        //                                    "PremiumDetail","ProjectDetail","QCStatusHistories","SubmissionHeader","PolicyDetail"};           

        //    IEnumerable<PropertyChanges> changes = Extensions.EnumeratePropertyDifferences<Submission, SubmissionModel>(submission, submissionModel);
        //    List<SubmissionHistory> entites = new List<SubmissionHistory>();
        //    int changeSetID = 0;// Context.SubmissionHistories.Where(p => p.SubmissionId == submissionModel.SubmissionId).Max(p => p.ChangeSetID);
        //    changeSetID++;
        //    foreach (var change in changes)
        //    {               
        //        var submissionHistory = new SubmissionHistory();
        //        if (!IgnoreableProperties.Contains(change.PropertyName))
        //        {                    
        //            submissionHistory = SubmissionHistoryMap(submissionModel, submission, change.PropertyName,changeSetID, userId);
        //            entites.Add(submissionHistory);
        //        }
        //    }
        //    Context.SubmissionHistories.AddRange(entites);
        //    Context.SaveChanges();
        //}

        //public SubmissionHistory SubmissionHistoryMap(SubmissionModel submissionModel,Submission submission, string PropertyName,int changeSetID, int userId)
        //{
        //    SubmissionHistory submissionHistory = new SubmissionHistory();
        //    string ColumnOldValue = null;
        //    string ColumnNewValue=null;
        //    switch (PropertyName)
        //    {
        //        case "ProfitCenterUnderWriterId":
        //            ColumnOldValue = submission.ProfitCenterUnderWriterId.ToString();
        //            ColumnNewValue = submissionModel.SelectedPCUnderwriter;
        //            submissionHistory.ColumnDesc = "Profit Center UnderWriter";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        case "IssuingUnderWriterId":
        //            ColumnOldValue = submission.IssuingUnderWriterId.ToString();
        //            ColumnNewValue = submissionModel.SelectedIssueUnderwriter;
        //            submissionHistory.ColumnDesc = "Issuing UnderWriter";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        case "ProductLineTypeId":
        //            ColumnOldValue = submission.ProductLineTypeId.ToString();
        //            ColumnNewValue = submissionModel.ProductLineTypeId.ToString();
        //            submissionHistory.ColumnDesc = "Product Line";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        case "ProductLineSubTypeId":
        //            ColumnOldValue = submissionModel.SelectedIssueUnderwriter;
        //            ColumnNewValue = submissionModel.SelectedIssueUnderwriter;
        //            submissionHistory.ColumnDesc = "Product Line Subtype";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        case "SectionCodeId":
        //            ColumnOldValue = submissionModel.SelectedIssueUnderwriter;
        //            ColumnNewValue = submissionModel.SelectedIssueUnderwriter;
        //            submissionHistory.ColumnDesc = "Section Code";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        case "ProfitCodeId":
        //            ColumnOldValue = submissionModel.SelectedIssueUnderwriter;
        //            ColumnNewValue = submissionModel.SelectedIssueUnderwriter;
        //            submissionHistory.ColumnDesc = "Profit Code";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        case "AttachmentTypeCode":
        //            ColumnOldValue = submissionModel.SelectedIssueUnderwriter;
        //            ColumnNewValue = submissionModel.SelectedIssueUnderwriter;
        //            submissionHistory.ColumnDesc = "Attachment Type";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        case "PolicyTypeID":
        //            ColumnOldValue = submissionModel.SelectedIssueUnderwriter;
        //            ColumnNewValue = submissionModel.SelectedIssueUnderwriter;
        //            submissionHistory.ColumnDesc = "Policy Type";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        case "CoverageID":
        //            ColumnOldValue = submissionModel.SelectedIssueUnderwriter;
        //            ColumnNewValue = submissionModel.SelectedIssueUnderwriter;
        //            submissionHistory.ColumnDesc = "Coverage Code";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        case "MarketSegmentCode":
        //            ColumnOldValue = submissionModel.SelectedIssueUnderwriter;
        //            ColumnNewValue = submissionModel.SelectedIssueUnderwriter;
        //            submissionHistory.ColumnDesc = "Market Segment";
        //            submissionHistory.TableName = "Submission";
        //            break;                   

        //        case "CurrentStatusId":
        //            ColumnOldValue = submissionModel.SelectedIssueUnderwriter;
        //            ColumnNewValue = submissionModel.SelectedIssueUnderwriter;
        //            submissionHistory.ColumnDesc = "Current Status";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        case "EffectiveDate":
        //            ColumnOldValue = submissionModel.SelectedIssueUnderwriter;
        //            ColumnNewValue = submissionModel.SelectedIssueUnderwriter;
        //            submissionHistory.ColumnDesc = "Effective Date";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        case "ExpiryDate":
        //            ColumnOldValue = submissionModel.SelectedIssueUnderwriter;
        //            ColumnNewValue = submissionModel.SelectedIssueUnderwriter;
        //            submissionHistory.ColumnDesc = "Expiry Date";
        //            submissionHistory.TableName = "Submission";
        //            break;

        //        default:                    
        //            break;
        //    }

        //    //int intId = Context.SubmissionHistories.Max(s => s.ChangeSetID);
        //    //int intId = Context.SubmissionHistories.Where(p => p.SubmissionId == submissionModel.SubmissionId).Max(p => p.ChangeSetID);
        //    submissionHistory.SubmissionId = submissionModel.SubmissionId;
        //    submissionHistory.ChangeSetID = changeSetID;
        //    submissionHistory.ColumnName = PropertyName;
        //    submissionHistory.ColumnOldValue = ColumnOldValue;
        //    submissionHistory.ColumnNewValue = ColumnNewValue;
        //    submissionHistory.CreatedByUserId = userId;
        //    submissionHistory.CreatedOnDate = DateTime.UtcNow;

        //    return submissionHistory;
        //}

        public Submission SubmissionDetailMap(SubmissionModel submissionModel, Submission submission)
        {
            submission.NewRenewalTypeCode = submissionModel.NewRenewalTypeCode;
            submission.ProfitCenterUnderWriterId = submissionModel.ProfitCenterUnderWriterId;
            submission.IssuingUnderWriterId = submissionModel.IssuingUnderWriterId;

            submission.ProductLineTypeId = submissionModel.ProductLineTypeId;
            submission.ProductLineSubTypeId = submissionModel.ProductLineSubTypeId;
            submission.SectionCodeId = submissionModel.SectionCodeId;
            submission.ProfitCodeId = submissionModel.ProfitCodeId;
            submission.AttachmentTypeCode = submissionModel.AttachmentTypeCode;
            submission.PolicyTypeID = submissionModel.PolicyTypeID;
            submission.CoverageID = submissionModel.CoverageID;
            submission.CurrentStatusId = submissionModel.CurrentStatusId;
            submission.EffectiveDate = submissionModel.EffectiveDate;
            submission.ExpiryDate = submissionModel.ExpiryDate;
            submission.RenewalofPolicyNumber = submissionModel.RenewalofPolicyNumber;
            submission.AffiliationsCode = submissionModel.AffiliationsCode;
            submission.MarketSegmentCode = submissionModel.MarketSegmentCode;
            
            submission.InsuredId = submissionModel.InsuredId;
            submission.InsuredAddressId = submissionModel.InsuredAddressId;
            submission.InsuredAliasId = submissionModel.InsuredAliasId;
            submission.IsDifferentDBA = submissionModel.IsDifferentDBA;
            submission.DBAName = submissionModel.DBAName;
            submission.DirectAssumedTypeCode = submissionModel.DirectAssumedTypeCode;
            submission.CedantName = submissionModel.CedantName;
            submission.DomicileStateId = submissionModel.DomicileStateId;
            submission.AssumedEntityType = submissionModel.AssumedEntityType;

            submission.BrokerEntityId = submissionModel.BrokerEntityId;
            submission.BrokerContactPersonId = submissionModel.BrokerContactPersonId;
            submission.IncumbentAttackingBrokerType = submissionModel.IncumbentAttackingBrokerType;

            //Other Details Mapping....
            submission.By_Berk_SI_FROM_Broker = submissionModel.By_Berk_SI_FROM_Broker;
            submission.IsBy_Berk_SI_FROM_Broker = submissionModel.IsBy_Berk_SI_FROM_Broker;
            submission.By_India_FROM_Berk_SI = submissionModel.By_India_FROM_Berk_SI;
            submission.IsBy_India_FROM_Berk_SI = submissionModel.IsBy_India_FROM_Berk_SI;
            submission.ProfitCentreOffice = submissionModel.ProfitCentreOffice;
            submission.IssuingOffice = submissionModel.IssuingOffice;
            submission.Remark = submissionModel.Remark;
            submission.IsUnclearAccount = submissionModel.IsUnclearAccount;

            //Status Dependent Details....
            submission.ReasonID = submissionModel.ReasonID;
            submission.ProcessDate = submissionModel.ProcessDate;
            submission.QCStatusId = (int)Enums.QCStatus.Pending;
            return submission;
        }

        public Submission SubmissionProjectMap(SubmissionModel submissionModel, Submission submission, int userId)
        {
            if (submission.ProjectDetail != null)
            {
                submission.ProjectDetail.ProjectName = submissionModel.ProjectDetail.ProjectName.RemoveExtraWhiteSpace().Trim();
                submission.ProjectDetail.ProjectOwnerName = submissionModel.ProjectDetail.ProjectOwnerName.RemoveExtraWhiteSpace().Trim();
                submission.ProjectDetail.ProjectStreetAddress = submissionModel.ProjectDetail.ProjectStreetAddress.RemoveExtraWhiteSpace().Trim();
                submission.ProjectDetail.Latitude = submissionModel.ProjectDetail.Latitude;
                submission.ProjectDetail.Longitude = submissionModel.ProjectDetail.Longitude;
                submission.ProjectDetail.GeneralContractorName = submissionModel.ProjectDetail.GeneralContractorName.RemoveExtraWhiteSpace().Trim();
                submission.ProjectDetail.CountryId = submissionModel.ProjectDetail.CountryId;
                submission.ProjectDetail.StateId = submissionModel.ProjectDetail.StateId;
                submission.ProjectDetail.CityId = submissionModel.ProjectDetail.CityId;
                submission.ProjectDetail.LastModifiedByUserId = userId;
                submission.ProjectDetail.LastModifedOnDate = DateTime.UtcNow;
                submission.ProjectDetail.ProcessIdentifier = submissionModel.ProcessIdentifier;
            }

            return submission;

        }


        public Submission SubmissionAdditionalInsuredMap(SubmissionModel submissionModel, Submission submission, int userId)
        {
            var InsuredIdList = submission.AdditionalInsureds.Select(x => x.ID).ToList();
            var currentInsList = submissionModel.AdditionalInsureds.Select(x => x.ID).ToList();

            submissionModel.AdditionalInsureds.ToList().ForEach(x =>
            {
                if (InsuredIdList.Any(y => y == x.ID))
                {
                    submission.AdditionalInsureds.Where(y => y.ID == x.ID).ToList().ForEach(o =>
                    {
                        o.InsuredName = x.InsuredName;
                        o.LastModifedOnDate = DateTime.UtcNow;
                        o.LastModifiedByUserId = userId;
                        o.ProcessIdentifier = submissionModel.ProcessIdentifier;
                    });
                }
                else
                {
                    var insured = AutoMapper.Mapper.DynamicMap<AdditionalInsured>(x);
                    insured.CreatedByUserId = userId;
                    insured.CreatedOnDate = DateTime.UtcNow;
                    insured.ProcessIdentifier = submissionModel.ProcessIdentifier;
                    submission.AdditionalInsureds.Add(insured);
                }
            });

            InsuredIdList.ForEach(x =>
              {
                  if (!currentInsList.Contains(x))
                  {
                      if (!currentInsList.Contains(x))
                      {
                          Context.AdditionalInsureds.Remove(Context.AdditionalInsureds.Where(z => z.ID == x).FirstOrDefault());
                          // submission.AdditionalInsureds.Remove(submission.AdditionalInsureds.Where(z => z.ID == x).FirstOrDefault());
                      }
                  }
              });

            return submission;
        }


        public Submission SubmissionAdditionalCedantMap(SubmissionModel submissionModel, Submission submission, int userId)
        {
            var CedantList = submission.AdditionalCedants.Select(x => x.ID).ToList();
            var currentList = submissionModel.AdditionalCedants.Select(x => x.ID).ToList();
            submissionModel.AdditionalCedants.ToList().ForEach(x =>
            {
                if (CedantList.Any(y => y == x.ID))
                {
                    submission.AdditionalCedants.Where(y => y.ID == x.ID).ToList().ForEach(o =>
                    {
                        o.CedantName = x.CedantName;
                        o.LastModifedOnDate = DateTime.UtcNow;
                        o.LastModifiedByUserId = userId;
                        o.ProcessIdentifier = submissionModel.ProcessIdentifier;
                    });
                }
                else
                {
                    var cedant = AutoMapper.Mapper.DynamicMap<AdditionalCedant>(x);
                    cedant.CreatedByUserId = userId;
                    cedant.CreatedOnDate = DateTime.UtcNow;
                    cedant.ProcessIdentifier = submissionModel.ProcessIdentifier;
                    submission.AdditionalCedants.Add(cedant);
                }
            });

            CedantList.ForEach(x =>
            {
                if (!currentList.Contains(x))
                {
                    Context.AdditionalCedants.Remove(Context.AdditionalCedants.Where(z => z.ID == x).FirstOrDefault());
                    // submission.AdditionalCedants.Remove(submission.AdditionalCedants.Where(z => z.ID == x).FirstOrDefault());
                }
            });
            return submission;
        }

        /// <summary>
        /// This Methed is going to use for map submission coverage table for update and delete..
        /// </summary>
        /// <param name="submissionModel"></param>
        /// <param name="submission"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Submission SubmissionCoverageMap(SubmissionModel submissionModel, Submission submission, int userId)
        {
            var ExistingSubmissionList = submission.SubmissionCoverages.Select(x => x.CoverageId).ToList();
            var CurrentSubmissionList = submissionModel.SelectedSubmissionCoverageList.Select(x => x.id).ToList();                       
          
            CurrentSubmissionList.ForEach(c =>
                {
                    if (!ExistingSubmissionList.Any(e => e == c))
                    {
                        var SubmissionCoverage = new SubmissionCoverage()
                        {
                            SubmissionId = submissionModel.SubmissionId,
                            CoverageId = c,
                            CreatedOnDate = DateTime.UtcNow,
                            CreatedByUserId = userId,
                            ProcessIdentifier = submissionModel.ProcessIdentifier
                        }; 
                         submission.SubmissionCoverages.Add(SubmissionCoverage);                       
                    }                                   
                });

            ExistingSubmissionList.ForEach(c =>
            {
                if (!CurrentSubmissionList.Contains(c))              
                {
                    submission.SubmissionCoverages.Remove(Context.SubmissionCoverages.Where(z => z.SubmissionId == submissionModel.SubmissionId && z.CoverageId==c).FirstOrDefault());                   
                }

            });

            return submission;
        }


        public Submission SubmissionPremiumMap(SubmissionModel submissionModel, Submission submission, int userId)
        {
            if (submissionModel.PremiumId != null)
            {
                submission.PremiumDetail.ExchangeRateDate = submissionModel.PremiumDetail.ExchangeRateDate;
                submission.PremiumDetail.LayerPercent = submissionModel.PremiumDetail.LayerPercent;
                submission.PremiumDetail.PolicyCommissionPercent = submissionModel.PremiumDetail.PolicyCommissionPercent;
                submission.PremiumDetail.OriginalCurrencyCode = submissionModel.PremiumDetail.OriginalCurrencyCode;
                submission.PremiumDetail.Installment = submissionModel.PremiumDetail.Installment;
                submission.PremiumDetail.OriginalLayerLimit = submissionModel.PremiumDetail.OriginalLayerLimit;
                submission.PremiumDetail.IsOriginalAttachmentPoint = submissionModel.PremiumDetail.IsOriginalAttachmentPoint;
                submission.PremiumDetail.OriginalAttachmentPoint = submissionModel.PremiumDetail.OriginalAttachmentPoint;
                submission.PremiumDetail.TransactionalCurrencyCode = submissionModel.PremiumDetail.TransactionalCurrencyCode;
                submission.PremiumDetail.ConversionRateToTransactional = submissionModel.PremiumDetail.ConversionRateToTransactional;
                submission.PremiumDetail.TransactionalSIR = submissionModel.PremiumDetail.TransactionalSIR;
                submission.PremiumDetail.TransactionalDeductible = submissionModel.PremiumDetail.TransactionalDeductible;
                submission.PremiumDetail.TransactionalGrossPremium = submissionModel.PremiumDetail.TransactionalGrossPremium;
                submission.PremiumDetail.TransactionalCollections = submissionModel.PremiumDetail.TransactionalCollections;
                submission.PremiumDetail.TransactionalDeductions = submissionModel.PremiumDetail.TransactionalDeductions;
                submission.PremiumDetail.TransactionalGSTIN = submissionModel.PremiumDetail.TransactionalGSTIN;
                submission.PremiumDetail.TransactionalGSTOUT = submissionModel.PremiumDetail.TransactionalGSTOUT;
                submission.PremiumDetail.TransactionalTIV = submissionModel.PremiumDetail.TransactionalTIV;
                submission.PremiumDetail.IsTransactionalTIV = submissionModel.PremiumDetail.IsTransactionalTIV;
                submission.PremiumDetail.JurCurrencyCode = submissionModel.PremiumDetail.JurCurrencyCode;
                submission.PremiumDetail.JurExchangeRate = submissionModel.PremiumDetail.JurExchangeRate;
                submission.PremiumDetail.USDExchangeRate = submissionModel.PremiumDetail.USDExchangeRate;
                submission.PremiumDetail.LastModifedOnDate = DateTime.UtcNow;
                submission.PremiumDetail.LastModifiedByUserId = userId.ToString();
                submission.PremiumDetail.ProcessIdentifier = submissionModel.ProcessIdentifier;
                submission.PremiumDetail.PremiumType = submissionModel.PremiumDetail.PremiumType;
            }
            else
            {
                var PremiumDetails = Mapper.DynamicMap<PremiumDetail>(submissionModel.PremiumDetail);
                PremiumDetails.CreatedByUserId = userId;
                PremiumDetails.CreatedOnDate = DateTime.UtcNow;
                PremiumDetails.ProcessIdentifier = submissionModel.ProcessIdentifier;
                submission.PremiumDetail = PremiumDetails;
            }
            return submission;

        }

        public PremiumDetail PremiumMap(PremiumDetailModel premiumModel, PremiumDetail premium, int userId)
        {
            if (premiumModel != null)
            {
                premium.LayerPercent = premiumModel.LayerPercent;
                premium.OriginalCurrencyCode = premiumModel.OriginalCurrencyCode;
                premium.Installment = premiumModel.Installment;
                premium.OriginalLayerLimit = premiumModel.OriginalLayerLimit;
                premium.IsOriginalAttachmentPoint = premiumModel.IsOriginalAttachmentPoint;
                premium.ExchangeRateDate = premiumModel.ExchangeRateDate;
                premium.PolicyCommissionPercent = premiumModel.PolicyCommissionPercent;
                premium.OriginalAttachmentPoint = premiumModel.OriginalAttachmentPoint;
                premium.TransactionalCurrencyCode = premiumModel.TransactionalCurrencyCode;
                premium.ConversionRateToTransactional = premiumModel.ConversionRateToTransactional;
                premium.TransactionalSIR = premiumModel.TransactionalSIR;
                premium.TransactionalDeductible = premiumModel.TransactionalDeductible;
                premium.TransactionalGrossPremium = premiumModel.TransactionalGrossPremium;
                premium.TransactionalCollections = premiumModel.TransactionalCollections;
                premium.TransactionalDeductions = premiumModel.TransactionalDeductions;
                premium.TransactionalGSTIN = premiumModel.TransactionalGSTIN;
                premium.TransactionalGSTOUT = premiumModel.TransactionalGSTOUT;
                premium.TransactionalTIV = premiumModel.TransactionalTIV;
                premium.IsTransactionalTIV = premiumModel.IsTransactionalTIV;
                premium.JurCurrencyCode = premiumModel.JurCurrencyCode;
                premium.JurExchangeRate = premiumModel.JurExchangeRate;
                premium.USDExchangeRate = premiumModel.USDExchangeRate;
                premium.PremiumType = premiumModel.PremiumType;
                premium.LastModifedOnDate = DateTime.UtcNow;
                premium.LastModifiedByUserId = userId.ToString();
                premium.ProcessIdentifier = premiumModel.ProcessIdentifier;
            }
            else
            {
                var PremiumDetails = Mapper.DynamicMap<PremiumDetail>(premium);
                PremiumDetails.CreatedByUserId = userId;
                PremiumDetails.CreatedOnDate = DateTime.UtcNow;
                PremiumDetails.ProcessIdentifier = premiumModel.ProcessIdentifier;
                premium = PremiumDetails;
            }
            return premium;

        }

        public Submission SubmissionCoverageLevelPremiumMap(SubmissionModel submissionModel, Submission submission, int userId)
        {
            var existingLayerList = submission.LayerDetails.Select(x => x.LayerId).ToList();
            var currentLayerList = submissionModel.LayerDetails.Select(x => x.LayerId).ToList();

            submissionModel.LayerDetails.ToList().ForEach(x =>
                {
                    if (existingLayerList.Any(y => y == x.LayerId))
                    {
                        submission.LayerDetails.Where(y => y.LayerId == x.LayerId).ToList().ForEach(o =>
                        {
                            o.LayerName = x.LayerName;
                            o.LastModifedOnDate = DateTime.UtcNow;
                            o.LastModifiedByUserId = userId;
                            var existingCoverageList = o.CoverageDetails.Select(z => z.CoverageDetailId).ToList();
                            var currentCoverageList = x.CoverageDetails.Select(z => z.CoverageDetailId).ToList();

                            x.CoverageDetails.ToList().ForEach(c =>
                                {
                                    if (o.CoverageDetails.Any(e => e.CoverageDetailId == c.CoverageDetailId))
                                    {
                                        o.CoverageDetails.Where(p => p.CoverageDetailId == c.CoverageDetailId).ToList().ForEach(q =>
                                          {
                                              var coverageModel = x.CoverageDetails.Where(p => p.CoverageDetailId == c.CoverageDetailId).FirstOrDefault();
                                              q.LayerId = coverageModel.LayerId;
                                              q.CoverageId = coverageModel.CoverageId;
                                              //    q.PremiumId = coverageModel.PremiumId;
                                              var premium = Context.PremiumDetails.Where(a => a.PremiumId == c.PremiumId).FirstOrDefault();
                                              if (premium == null)
                                              {
                                                  premium = new PremiumDetail();
                                                  premium.CreatedOnDate = DateTime.UtcNow;
                                                  premium.CreatedByUserId = userId;
                                              }
                                              q.PremiumDetail = PremiumMap(coverageModel.PremiumDetail, premium, userId);
                                              //q.PremiumDetail.CoverageDetails = null;
                                          });
                                    }
                                    else
                                    {
                                        var coverageDetail = AutoMapper.Mapper.DynamicMap<CoverageDetail>(c);
                                        coverageDetail.PremiumDetail.CreatedOnDate = DateTime.UtcNow;
                                        coverageDetail.PremiumDetail.CreatedByUserId = userId;
                                        o.CoverageDetails.Add(coverageDetail);
                                    }

                                });

                            existingCoverageList.ForEach(q =>
                            {
                                if (!currentCoverageList.Contains(q))
                                {
                                    Context.CoverageDetails.Remove(Context.CoverageDetails.Where(z => z.CoverageDetailId == q).FirstOrDefault());
                                }
                            });

                        });
                    }
                    else
                    {
                        var layer = AutoMapper.Mapper.DynamicMap<LayerDetail>(x);
                        layer.CreatedByUserId = userId;
                        layer.CreatedOnDate = DateTime.UtcNow;
                        submission.LayerDetails.Add(layer);
                    }


                });

                    existingLayerList.ForEach(r =>
                    {
                        if (!currentLayerList.Contains(r))
                        {
                            Context.LayerDetails.Remove(Context.LayerDetails.Where(z => z.LayerId == r).FirstOrDefault());
                        }
                    });


            return submission;

        }

        public CoverageDetail CoverageDetailMap(CoverageDetailModel coverageDetailModel, CoverageDetail coverageDetail)
        {

            return coverageDetail;
        }

        /// <summary>
        /// This methed is used for map policy details map....
        /// </summary>
        /// <param name="submissionModel"></param>
        /// <param name="submission"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Submission SubmissionPolicyDetailMap(SubmissionModel submissionModel, Submission submission, int userId)
        {
            if (submissionModel.PolicyId != null)
            {
                submission.PolicyDetail.IsBindDate = submissionModel.PolicyDetail.IsBindDate;
                submission.PolicyDetail.BindDate = submissionModel.PolicyDetail.BindDate;
                submission.PolicyDetail.Renewable = submissionModel.PolicyDetail.Renewable;
                submission.PolicyDetail.RenewalDate = submissionModel.PolicyDetail.RenewalDate;
                submission.PolicyDetail.AdmittedTypeCode = submissionModel.PolicyDetail.AdmittedTypeCode;
                submission.PolicyDetail.CompanyPaperTypeCode = submissionModel.PolicyDetail.CompanyPaperTypeCode;
                submission.PolicyDetail.IsCompanyPaperType = submissionModel.PolicyDetail.IsCompanyPaperType;
                submission.PolicyDetail.CompanyPaperNumberTypeCode = submissionModel.PolicyDetail.CompanyPaperNumberTypeCode;
                submission.PolicyDetail.IsCompanyPaperNumber = submissionModel.PolicyDetail.IsCompanyPaperNumber;
                submission.PolicyDetail.PolicySymbol = submissionModel.PolicyDetail.PolicySymbol;
                submission.PolicyDetail.PolicyNumber = submissionModel.PolicyDetail.PolicyNumber;
                submission.PolicyDetail.IsPolicyNumber = submissionModel.PolicyDetail.IsPolicyNumber;
                submission.PolicyDetail.SuffixCode = submissionModel.PolicyDetail.SuffixCode;
                submission.PolicyDetail.LTACode = submissionModel.PolicyDetail.LTACode;
                submission.PolicyDetail.RiskCountryId = submissionModel.PolicyDetail.RiskCountryId;
                submission.PolicyDetail.MasterPolicyNumber = submissionModel.PolicyDetail.MasterPolicyNumber;

                submission.PolicyDetail.OffOnShoreCode = submissionModel.PolicyDetail.OffOnShoreCode;
                submission.PolicyDetail.FormTypeId = submissionModel.PolicyDetail.FormTypeId;
                submission.PolicyDetail.LastModifedOnDate = DateTime.UtcNow;
                submission.PolicyDetail.LastModifiedByUserId = userId.ToString();
                submission.PolicyDetail.ProcessIdentifier = submissionModel.ProcessIdentifier;
            }
            else
            {
                var PolicyDetails = Mapper.DynamicMap<PolicyDetail>(submissionModel.PolicyDetail);
                if (PolicyDetails != null)
                {
                    PolicyDetails.CreatedByUserId = userId;
                    PolicyDetails.CreatedOnDate = DateTime.UtcNow;
                    PolicyDetails.ProcessIdentifier = submissionModel.ProcessIdentifier;
                    submission.PolicyDetail = PolicyDetails;
                }

            }

            return submission;

        }

       
        #endregion

        #region GetSubmission
        public SubmissionModel GetSubmission(Guid? submissionId, Enums.SubmissionProcess currentProcess)
        {
            if (currentProcess == Enums.SubmissionProcess.CreateAmendment)
            {
                submissionId = GetCurrentSubmissionId(submissionId);
            }

            var submission = Context.Submissions
                            .Include("SubmissionHeader").Include("SubmissionHeader.Submissions")
                            .Include("ProjectDetail")
                            .Include("AdditionalCedants")
                            .Include("AdditionalInsureds")
                            .Include("PremiumDetail")
                            .Include("PolicyDetail")
                            .Include("LayerDetails")
                            .Include("SubmissionCoverages")
                            .Include("LayerDetails.CoverageDetails")
                            .Where(x => x.SubmissionId == submissionId).FirstOrDefault();

            var submissionModel = new SubmissionModel();


            if (submission != null)
            {
                if (currentProcess == Enums.SubmissionProcess.CreateAmendment &&
                    (submission.QCStatusId != (int)Enums.QCStatus.Approval || !(submission.CurrentStatusId == (int)Enums.CurrentStatus.Bound || submission.CurrentStatusId == (int)Enums.CurrentStatus.ReEntry))) return submissionModel;
                submissionModel = AutoMapper.Mapper.DynamicMap<SubmissionModel>(submission ?? new Submission());
                submissionModel.SubmissionHeader = null;
                submissionModel.CurrentProcess = currentProcess;
                submissionModel.SixDigitNumber = submissionModel.SixDigitSubmissionNumber.ToString();
                submissionModel.PCUnderwriter = ApplicationContext.Master.GetPCUnderwriterList(submissionModel.ProfitCenterUnderWriterId).FirstOrDefault().UnderwriterName;
                submissionModel.SelectedPCUnderwriter = submissionModel.PCUnderwriter;
                submissionModel.IssueUnderwriter = submissionModel.IssuingUnderWriterId != 0 ? (ApplicationContext.Master.GetPCUnderwriterList(submissionModel.IssuingUnderWriterId).FirstOrDefault().UnderwriterName) : "";
                submissionModel.SelectedIssueUnderwriter = submissionModel.IssueUnderwriter;

                if (submissionModel.DomicileStateId != null)
                {
                    var state = mdmContext.States.Where(x => x.StateId == submissionModel.DomicileStateId).FirstOrDefault();
                    if (state != null)
                    {
                        submissionModel.DomicileCountryId = state.Country.CountryId;
                    }
                }

                submissionModel.ProjectDetail = AutoMapper.Mapper.DynamicMap<ProjectDetailModel>(submission.ProjectDetail);
                if (submissionModel.ProjectDetail != null) submissionModel.ProjectDetail.Submissions = null;

                submissionModel.AdditionalInsureds = AutoMapper.Mapper.DynamicMap<List<AdditionalInsuredModel>>(submission.AdditionalInsureds.ToList());
                submissionModel.AdditionalInsureds.ForEach(x => { x.Submission = null; });

                submissionModel.AdditionalCedants = AutoMapper.Mapper.DynamicMap<List<AdditionalCedantModel>>(submission.AdditionalCedants.ToList());

                submissionModel.SubmissionCoverages = AutoMapper.Mapper.DynamicMap<List<SubmissionCoverageModel>>(submission.SubmissionCoverages.ToList());
                submissionModel.SelectedSubmissionCoverageList = new List<MultiSelectDropDownItem>();
                submissionModel.SubmissionCoverages.ForEach(x =>
                {
                    x.Submission = null;
                    var SelectedSubmission=new MultiSelectDropDownItem()
                    {
                    id=x.CoverageId,
                    label=mdmContext.Coverages.Find(x.CoverageId).CoverageName
                    };
                    submissionModel.SelectedSubmissionCoverageList.Add(SelectedSubmission);
                });

                submissionModel.AdditionalCedants.ForEach(x =>
                {
                    x.Submission = null;
                    var state = mdmContext.States.Include("Country").Where(y => y.StateId == x.DomicileStateId).FirstOrDefault();
                    if (state != null)
                    {
                        x.Statename = state.StateName + " - " + state.StateCode;
                        x.CountryName = state.Country.CountryName + " - " + state.Country.CountryNumericCode;//x.CountryName + " - " + x.CountryNumericCode,
                    }
                    else
                    {
                        var country = mdmContext.Countries.Where(z => z.CountryId == x.DomicileCountryId).FirstOrDefault();
                        if (country != null) x.CountryName = country.CountryName + " - " + country.CountryNumericCode; ;
                    }

                    var AssumeEntity = mdmContext.TypeMasters.Where(y => y.TypeCode == x.AssumedEntityType).FirstOrDefault();
                    x.AssumedEntityTypeName = AssumeEntity != null ? AssumeEntity.TypeName : "";

                });

                submissionModel.BrokerDetail = ApplicationContext.Master.GetBrokerEntity(submissionModel.BrokerEntityId);
                submissionModel.Broker = submissionModel.BrokerDetail != null ? submissionModel.BrokerDetail.BrokerName : "";
                submissionModel.SelectedBroker = submissionModel.Broker;
                submissionModel.BrokerId = submissionModel.BrokerDetail != null ? submissionModel.BrokerDetail.BrokerId : Guid.Empty;
                submissionModel.BrokerEntity = submissionModel.BrokerDetail != null ? submissionModel.BrokerDetail.BrokerEntityName : "";
                submissionModel.SelectedBrokerEntity = submissionModel.BrokerEntity;

                submissionModel.BrokerContact = ApplicationContext.Master.SearchBrokerContact("", null, submissionModel.BrokerContactPersonId).FirstOrDefault();

                submissionModel.BrokerContactPerson = submissionModel.BrokerContact != null ? submissionModel.BrokerContact.FirstName + " " + submissionModel.BrokerContact.LastName : "";// ApplicationContext.Master.SearchBrokerContact("", null, submissionModel.BrokerContactPersonId);
                submissionModel.SelectedBrokerContactPerson = submissionModel.BrokerContactPerson;



                submissionModel.InsuredDetail = ApplicationContext.Master.GetInsured(submissionModel.InsuredId, submissionModel.InsuredAddressId, submissionModel.InsuredAliasId ?? 0);
                submissionModel.Insured = submissionModel.InsuredDetail != null ? submissionModel.InsuredDetail.InsuredAliasName : "";
                submissionModel.SelectedInsured = submissionModel.Insured;

                submissionModel.PolicyDetail = AutoMapper.Mapper.DynamicMap<PolicyDetailModel>(submission.PolicyDetail);
                if (submissionModel.PolicyDetail != null)
                {
                    submissionModel.PolicyDetail.Submissions = null;
                }
                else
                {
                    submissionModel.PolicyDetail = new PolicyDetailModel();
                }
                submissionModel.PremiumDetail = AutoMapper.Mapper.DynamicMap<PremiumDetailModel>(submission.PremiumDetail);

                if (submissionModel.PremiumDetail != null)
                {
                    submissionModel.PremiumDetail.TransactionalCurrencyCode = submissionModel.PremiumDetail.TransactionalCurrencyCode != null ? submissionModel.PremiumDetail.TransactionalCurrencyCode.Trim() : submissionModel.PremiumDetail.TransactionalCurrencyCode;
                    submissionModel.PremiumDetail.Submissions = null;
                }
                else
                {
                    submissionModel.PremiumDetail = new PremiumDetailModel();
                }

                submissionModel.LayerDetails = AutoMapper.Mapper.DynamicMap<List<LayerDetailModel>>(submission.LayerDetails.ToList());
                submissionModel.LayerDetails.ForEach(x => { x.Submission = null; });
                submissionModel.LayerDetails.ForEach(x =>
                {
                    x.CoverageDetails = AutoMapper.Mapper.DynamicMap<List<CoverageDetailModel>>(Context.CoverageDetails.Where(y => y.LayerId == x.LayerId).ToList());
                    x.CoverageDetails.ForEach(o => o.LayerDetail = null);
                    x.CoverageDetails.ForEach(p =>
                    {
                        var prem = Context.PremiumDetails.Where(y => y.PremiumId == p.PremiumId).FirstOrDefault();
                        p.PremiumDetail = AutoMapper.Mapper.DynamicMap<PremiumDetailModel>(prem);
                        if (p.PremiumDetail != null)
                            p.PremiumDetail.Submissions = null;
                    });
                });

                switch (submissionModel.CurrentProcess)
                {
                    case Enums.SubmissionProcess.CreateAmendment:
                        {

                            GetAmendmentModel(submissionModel);
                            submissionModel.ParentEffectiveDate = submission.EffectiveDate;
                            break;
                        }
                    case Enums.SubmissionProcess.EditAmendment:
                        {
                            var parentSubmission = submission.SubmissionHeader.Submissions.Where(x => x.SubmissionId == submission.ImmediateParentId).FirstOrDefault();
                            if (parentSubmission != null) submissionModel.ParentEffectiveDate = parentSubmission.EffectiveDate;
                            // submissionModel.ParentEffectiveDate = submission.SubmissionHeader.Submissions.Where(x => x.CurrentStatusId == (int)Enums.CurrentStatus.Bound && x.SubmissionNumber.Contains("-01")).FirstOrDefault().EffectiveDate;
                            break;
                        }
                    case Enums.SubmissionProcess.EditReEntry:
                    case Enums.SubmissionProcess.ViewReEntry:
                        {
                            var parentSubmission = submission.SubmissionHeader.Submissions.Where(x => x.SubmissionId == submission.ParentSubmissionId).FirstOrDefault();
                            if (parentSubmission != null)
                            {
                                submissionModel.ParentEffectiveDate = parentSubmission.EffectiveDate;
                                submissionModel.ParentSubmissionNo = parentSubmission.SubmissionNumber;
                            }
                            break;
                        }
                    case Enums.SubmissionProcess.ViewReversal:
                        {

                            var parentSubmission = submission.SubmissionHeader.Submissions.Where(x => x.SubmissionId == submission.ParentSubmissionId).FirstOrDefault();
                            if (parentSubmission != null)
                            {
                                submissionModel.ParentSubmissionNo = parentSubmission.SubmissionNumber;
                            }

                            var additionalCedant = parentSubmission.AdditionalCedants.ToList();
                            submissionModel.AdditionalCedants = AutoMapper.Mapper.DynamicMap<List<AdditionalCedantModel>>(additionalCedant);
                            submissionModel.AdditionalCedants.ForEach(x => x.Submission = null);
                            var additionalInsured = parentSubmission.AdditionalInsureds.ToList();
                            submissionModel.AdditionalInsureds = AutoMapper.Mapper.DynamicMap<List<AdditionalInsuredModel>>(additionalInsured);
                            submissionModel.AdditionalInsureds.ForEach(x => x.Submission = null);

                            var reversalSubmission = submission.SubmissionHeader.Submissions.Where(x => x.SubmissionId == submission.SubmissionId).FirstOrDefault();
                            submissionModel.LayerDetails = AutoMapper.Mapper.DynamicMap<List<LayerDetailModel>>(reversalSubmission.LayerDetails.ToList());
                            submissionModel.LayerDetails.ForEach(x => { x.Submission = null; });
                            submissionModel.LayerDetails.ForEach(x =>
                            {
                                //  x.CoverageDetails = AutoMapper.Mapper.DynamicMap<List<CoverageDetailModel>>(x.CoverageDetails.ToList());
                                x.CoverageDetails.ForEach(o => o.LayerDetail = null);
                                x.CoverageDetails.ForEach(p =>
                                {
                                    //   var prem = Context.PremiumDetails.Where(y => y.PremiumId == p.PremiumId).FirstOrDefault();
                                    // p.PremiumDetail = AutoMapper.Mapper.DynamicMap<PremiumDetailModel>(prem);
                                    if (p.PremiumDetail != null)
                                        p.PremiumDetail.Submissions = null;
                                });
                            });
                            break;
                        }
                }

            }

            return submissionModel;
        }


        public SubmissionModel GetSubmissionModel(Guid? submissionId, Enums.SubmissionProcess currentProcess)
        {
            var submissionModel = GetSubmission(submissionId, currentProcess);
            return submissionModel;
        }

        /// <summary>
        /// Amendment
        /// </summary>
        /// <param name="submissionModel"></param>
        private void GetAmendmentModel(SubmissionModel submissionModel)
        {
            submissionModel.PremiumDetail = SetPrimiumZero(submissionModel.PremiumDetail);
            submissionModel.CurrentStatusId = 0;
            submissionModel.LayerDetails.ForEach(x =>
            {
                x.CoverageDetails.ForEach(p =>
                {
                    if (p.PremiumDetail != null)
                    {
                        p.PremiumDetail = SetPrimiumZero(p.PremiumDetail);
                    }

                });
            });
        }

        private PremiumDetailModel SetPrimiumZero(PremiumDetailModel premiumModel)
        {
            premiumModel.LayerPercent = 0;
            premiumModel.PolicyCommissionPercent = 0;
            premiumModel.OriginalLayerLimit = 0;
            premiumModel.ConversionRateToTransactional = 0;
            premiumModel.OriginalAttachmentPoint = 0;
            premiumModel.TransactionalSIR = 0;
            premiumModel.TransactionalDeductible = 0;
            premiumModel.TransactionalGrossPremium = 0;
            premiumModel.TransactionalCollections = 0;
            premiumModel.TransactionalDeductions = 0;
            premiumModel.TransactionalGSTIN = 0;
            premiumModel.TransactionalGSTOUT = 0;
            premiumModel.TransactionalTIV = 0;
            premiumModel.JurExchangeRate = 0;
            premiumModel.USDExchangeRate = 0;

            return premiumModel;
        }

        public bool CompareEndorsePremium(PremiumDetailModel premiumModel, Guid ParentSubmissionId)
        {
            var rs = false;
            var submission = Context.Submissions.Include("SubmissionHeader").Include("PremiumDetail").Where(x => x.SubmissionId == ParentSubmissionId).FirstOrDefault();
            var parentSubmission = submission.SubmissionHeader.Submissions.Where(x => x.CurrentStatusId == (int)Enums.CurrentStatus.Bound).FirstOrDefault();
            if (parentSubmission != null)
            {
                var premimum = AutoMapper.Mapper.DynamicMap<PremiumDetailModel>(submission.PremiumDetail);
                rs = premiumModel.LayerPercent != 0 && premiumModel.LayerPercent != (premimum.LayerPercent ?? 0);
                rs = rs || (premiumModel.PolicyCommissionPercent != 0 && premiumModel.PolicyCommissionPercent != (premimum.PolicyCommissionPercent ?? 0));
                rs = rs || (premiumModel.OriginalLayerLimit != 0 && premiumModel.OriginalLayerLimit != (premimum.OriginalLayerLimit ?? 0));
                rs = rs || (premiumModel.ConversionRateToTransactional != 0 && premiumModel.ConversionRateToTransactional != (premimum.ConversionRateToTransactional ?? 0));
                rs = rs || (premiumModel.OriginalAttachmentPoint != 0 && premiumModel.OriginalAttachmentPoint != (premimum.OriginalAttachmentPoint ?? 0));
                rs = rs || (premiumModel.TransactionalSIR != 0 && premiumModel.TransactionalSIR != (premimum.TransactionalSIR ?? 0));
                rs = rs || (premiumModel.TransactionalDeductible != 0 && premiumModel.TransactionalDeductible != (premimum.TransactionalDeductible ?? 0));
                rs = rs || (premiumModel.TransactionalGrossPremium != 0 && premiumModel.TransactionalGrossPremium != (premimum.TransactionalGrossPremium ?? 0));
                rs = rs || (premiumModel.TransactionalCollections != 0 && premiumModel.TransactionalCollections != (premimum.TransactionalCollections ?? 0));
                rs = rs || (premiumModel.TransactionalDeductions != 0 && premiumModel.TransactionalDeductions != (premimum.TransactionalDeductions ?? 0));
                rs = rs || (premiumModel.TransactionalGSTIN != 0 && premiumModel.TransactionalGSTIN != (premimum.TransactionalGSTIN ?? 0));
                rs = rs || (premiumModel.TransactionalGSTOUT != 0 && premiumModel.TransactionalGSTOUT != (premimum.TransactionalGSTOUT ?? 0));
                rs = rs || (premiumModel.TransactionalTIV != 0 && premiumModel.TransactionalTIV != (premimum.TransactionalTIV ?? 0));
                rs = rs || (premiumModel.JurExchangeRate != 0 && premiumModel.JurExchangeRate != (premimum.JurExchangeRate ?? 0));
                rs = rs || (premiumModel.USDExchangeRate != 0 && premiumModel.USDExchangeRate != (premimum.USDExchangeRate ?? 0));
            }
            return rs;
        }
        #endregion

        #region Reverasal

        public void CreateReversal(Guid submissionId, int userId)
        {
            Context.usp_CreateReversal(submissionId, userId);
        }

        #endregion

        #region UserRegions

        public UserRegionModel GetUserRegions(int userId, int RegionId = 0)
        {
            UserRegionModel userRegionModel = new UserRegionModel();

            userRegionModel.UserRegionList = Context.UserRegions.Where(x => x.UserId == userId).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.RegionName,
                                     Value = x.RegionId.ToString(),
                                     Selected = RegionId == x.RegionId

                                 }).OrderBy(x => x.Text).ToList();

            return userRegionModel;

        }

        public void UpdateCookie(string cookie, LoggedInUser loggedInUser)
        {
            var ticket = new FormsAuthenticationTicket(1, loggedInUser.UserId.ToString(), DateTime.Now, DateTime.Now.AddDays(30), false, new JavaScriptSerializer().Serialize(loggedInUser));
            var userData = ticket.UserData;
            Guid cookieId = GetGUID(cookie);
            if (Context.UserSessionLogs.Any(o => o.Id == cookieId))
            {
                UserSessionLog UpdatedModel = Context.UserSessionLogs.Find(cookieId);
                UpdatedModel.LastModifiedOnDate = DateTime.UtcNow;
                UpdatedModel.SessionString = userData;
                Context.SaveChanges();
            }
        }

        public Guid GetGUID(string str)
        {
            Guid guid = Guid.Empty;
            if (!Guid.TryParse(str, out guid))
            {
                guid = Guid.Empty;
            }
            return guid;
        }



        #endregion


        #region Submission and QC listinng
        public SubmissionListingModel GetSubmissionFilters()
        {
            return new SubmissionListingModel
            {
                Filters = new SubmissionFilters { },

                FilterLists = new SubmissionFilterLists
                {
                    NewRenewal = mdmContext.TypeMasters.Where(o => o.IsActive).Where(o => o.TypeCategory == "NewRenewal").ToList()
                        .Select(o => new SelectListItem { Text = o.TypeName, Value = o.TypeCode }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select New Renewal -"),

                    Renewable = mdmContext.TypeMasters.Where(o => o.IsActive).Where(o => o.TypeCategory == "Renewable").ToList()
                        .Select(o => new SelectListItem { Text = o.TypeName, Value = o.TypeCode }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select Renewable -"),

                    ProfitCenterUW = mdmContext.Underwriters.Where(o => o.IsActive).ToList()
                        .Select(o => new SelectListItem { Text = o.UnderwriterName, Value = o.UnderwriterId.ToString() }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select Profit Centre Underwriter -"),

                    ProductLine = mdmContext.ProductLines.Where(o => o.IsActive).ToList()
                        .Select(o => new SelectListItem { Text = o.ProductLineName, Value = o.ProductLineId.ToString() }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select Product Line -"),

                    ProductLineSubtype = mdmContext.ProductLineSubTypes.Where(o => o.IsActive).ToList()
                        .Select(o => new SelectListItem { Text = o.ProductLineSubTypeName, Value = o.ProductLineSubTypeId.ToString() }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select Product Sub Type -"),

                    ProfitCentreOffices = mdmContext.Branches.Where(o => o.IsActive).ToList()
                        .Select(o => new SelectListItem { Text = o.BranchName, Value = o.BranchId.ToString() }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select Profit Centre Office  -"),

                    IssuingOffices = mdmContext.Underwriters.Where(o => o.IsActive).ToList()
                        .Select(o => new SelectListItem { Text = o.UnderwriterName, Value = o.UnderwriterId.ToString() }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select Issuing Office -"),

                    PolicyType = mdmContext.PolicyTypes.Where(o => o.IsActive).ToList()
                        .Select(o => new SelectListItem { Text = o.PolicyType1, Value = o.PolicyTypeId.ToString() }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select Policy Type -"),

                    CurrentStatus = mdmContext.Status.Where(o => o.IsActive).Where(o => o.StatusCategory == "SubmissionStatus" && o.SourceSystemName == "INT_SUBM").ToList()
                        .Select(o => new SelectListItem { Text = o.StatusName, Value = o.StatusId.ToString() }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select Status -"),

                    Broker = mdmContext.Brokers.Where(o => o.IsActive).ToList()
                        .Select(o => new SelectListItem { Text = o.BrokerName, Value = o.BrokerId.ToString() }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select Broker -"),

                    BrokerEntity = mdmContext.BrokerParties.Include("City").Where(o => o.IsActive).ToList()
                        .Select(o => new SelectListItem { Text = o.BrokerEntityName + " - " + o.City.CityName, Value = o.BrokerPartyId.ToString() }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select Broker Entity -"),

                    QCStatus = mdmContext.Status.Where(o => o.IsActive).Where(o => o.StatusCategory == "QCStatus").ToList()
                        .Select(o => new SelectListItem { Text = o.StatusName, Value = o.StatusId.ToString() }).OrderBy(o => o.Text).ToList()
                        .InsertDefault("- Select QC Status -")
                }
            };
        }


        public DataTableResponse<USP_GetSubmissions_Model> GetSubmissions(SubmissionFilters filters, int region)
        {

            var recordCount = new ObjectParameter("recordCount", typeof(int));
            var result = Mapper.DynamicMap<List<USP_GetSubmissions_Model>>(Context.USP_GetSubmissions
                (
                    submissionNumber: filters.SubmissionNumber,
                    newRenewalCode: filters.NewRenewalCode,
                    renewalCode: filters.RenewalCode,
                    policyNumber: filters.PolicyNumber,
                    insuredName: filters.InsuredName,
                    uwID: filters.UwID,
                    pID: filters.PID,
                    psID: filters.PsID,
                    cID: filters.CID,
                    qCStatus: filters.QCStatus,
                    bID: filters.BID,
                    bpID: filters.BpID,
                    pTypes: filters.PTypes,
                    effDateF: filters.EffDateF,
                    effDateT: filters.EffDateT,
                    proDateF: filters.ProDateF,
                    proDateT: filters.ProDateT,
                    createDateF: filters.CreateDateF,
                    createDateT: filters.CreateDateT,
                    modifyDateF: filters.ModifyDateF,
                    modifyDateT: filters.ModifyDateT,
                    modifiedBy: filters.ModifiedBy,
                    createdBy: filters.CreatedBy,
                    qCDateF: filters.QCDateF,
                    qCDateT: filters.QCDateT,
                    orderBy: filters.sCol,
                    orderDirection: filters.sDir,
                    pageNo: filters.start,
                    pageSize: filters.length,
                    recordCount: recordCount,
                    regionId: region

                ).ToList());

            foreach (var item in result)
            {
                item.AmendmentList = Mapper.DynamicMap<List<USP_GetSubmissions_Model>>(Context.USP_GetAmendmentSubmissions(submissionNumber: item.SubmissionNumber, regionId: region).ToList());
            }

            var count = recordCount.Value == DBNull.Value ? 0 : Convert.ToInt32(recordCount.Value);
            var xyz = new DataTableResponse<USP_GetSubmissions_Model>
            {
                status = true,
                sEcho = filters.draw + 1,
                iTotalRecords = count,
                iTotalDisplayRecords = count,
                aaData = Mapper.DynamicMap<List<USP_GetSubmissions_Model>>(result),

                //amendmentList = Mapper.DynamicMap<List<USP_GetSubmissions_Model>>(result.Where(x => !x.SubmissionNumber.ToString().EndsWith("01")))
            };
            return xyz;

        }



        /// <summary>
        /// //Amendment QC
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="region"></param>
        /// <param name="QCS"></param>
        /// <param name="SCS"></param>
        /// <returns></returns>
        public DataTableResponse<USP_GetAmendmentQC_Model> GetAmendmentQC(SubmissionFilters filters, int region)
        {

            var recordCount = new ObjectParameter("recordCount", typeof(int));
            var result = Mapper.DynamicMap<List<USP_GetAmendmentQC_Model>>(Context.USP_GetAmendmentQC
                (
                    submissionNumber: filters.SubmissionNumber,
                    newRenewalCode: filters.NewRenewalCode,
                    renewalCode: filters.RenewalCode,
                    policyNumber: filters.PolicyNumber,
                    insuredName: filters.InsuredName,
                    uwID: filters.UwID,
                    pID: filters.PID,
                    psID: filters.PsID,
                    cID: filters.CID,
                    qCStatus: filters.QCStatus,
                    bID: filters.BID,
                    bpID: filters.BpID,
                    pTypes: filters.PTypes,
                    effDateF: filters.EffDateF,
                    effDateT: filters.EffDateT,
                    proDateF: filters.ProDateF,
                    proDateT: filters.ProDateT,
                    createDateF: filters.CreateDateF,
                    createDateT: filters.CreateDateT,
                    modifyDateF: filters.ModifyDateF,
                    modifyDateT: filters.ModifyDateT,
                    modifiedBy: filters.ModifiedBy,
                    createdBy: filters.CreatedBy,
                    qCDateF: filters.QCDateF,
                    qCDateT: filters.QCDateT,
                    orderBy: filters.sCol,
                    orderDirection: filters.sDir,
                    pageNo: filters.start,
                    pageSize: filters.length,
                    recordCount: recordCount,
                    regionId: region

                ).ToList());

            var count = recordCount.Value == DBNull.Value ? 0 : Convert.ToInt32(recordCount.Value);
            var xyz = new DataTableResponse<USP_GetAmendmentQC_Model>
            {
                status = true,
                sEcho = filters.draw + 1,
                iTotalRecords = count,
                iTotalDisplayRecords = count,
                aaData = Mapper.DynamicMap<List<USP_GetAmendmentQC_Model>>(result),

                //amendmentList = Mapper.DynamicMap<List<USP_GetSubmissions_Model>>(result.Where(x => !x.SubmissionNumber.ToString().EndsWith("01")))
            };
            return xyz;

        }



        /// <summary>
        ///  //Submission QC
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public DataTableResponse<USP_GetSubmissionsQC_Model> GetSubmissionQC(SubmissionFilters filters, int region)
        {

            var recordCount = new ObjectParameter("recordCount", typeof(int));
            var result = Mapper.DynamicMap<List<USP_GetSubmissionsQC_Model>>(Context.USP_GetSubmissionsQC
                (
                    submissionNumber: filters.SubmissionNumber,
                    newRenewalCode: filters.NewRenewalCode,
                    renewalCode: filters.RenewalCode,
                    policyNumber: filters.PolicyNumber,
                    insuredName: filters.InsuredName,
                    uwID: filters.UwID,
                    pID: filters.PID,
                    psID: filters.PsID,
                    cID: filters.CID,
                    qCStatus: filters.QCStatus,
                    bID: filters.BID,
                    bpID: filters.BpID,
                    pTypes: filters.PTypes,
                    effDateF: filters.EffDateF,
                    effDateT: filters.EffDateT,
                    proDateF: filters.ProDateF,
                    proDateT: filters.ProDateT,
                    createDateF: filters.CreateDateF,
                    createDateT: filters.CreateDateT,
                    modifyDateF: filters.ModifyDateF,
                    modifyDateT: filters.ModifyDateT,
                    modifiedBy: filters.ModifiedBy,
                    createdBy: filters.CreatedBy,
                    qCDateF: filters.QCDateF,
                    qCDateT: filters.QCDateT,
                    orderBy: filters.sCol,
                    orderDirection: filters.sDir,
                    pageNo: filters.start,
                    pageSize: filters.length,
                    recordCount: recordCount,
                    regionId: region

                ).ToList());

            var count = recordCount.Value == DBNull.Value ? 0 : Convert.ToInt32(recordCount.Value);
            var xyz = new DataTableResponse<USP_GetSubmissionsQC_Model>
            {
                status = true,
                sEcho = filters.draw + 1,
                iTotalRecords = count,
                iTotalDisplayRecords = count,
                aaData = Mapper.DynamicMap<List<USP_GetSubmissionsQC_Model>>(result),

                //amendmentList = Mapper.DynamicMap<List<USP_GetSubmissions_Model>>(result.Where(x => !x.SubmissionNumber.ToString().EndsWith("01")))
            };
            return xyz;

        }

        #endregion



        public bool checkSubmissionNumberExist(string sixDigitNo)
        {
            return Context.Submissions.Any(x => x.SixDigitSubmissionNumber == sixDigitNo);
        }
        public bool checkPolicyNumberExist(string PolicyNumber)
        {
            return Context.PolicyDetails.Any(x => x.PolicyNumber == PolicyNumber);
        }

        public Guid? GetCurrentSubmissionId(Guid? boundSubmissionId)
        {
            var submission = Context.Submissions.Include("SubmissionHeader").Include("SubmissionHeader.Submissions")
                            .Where(x => x.SubmissionId == boundSubmissionId).FirstOrDefault();
            bool IsFound = true;
            while (IsFound)
            {
                var child = submission.SubmissionHeader.Submissions.Where(x => x.ParentSubmissionId == boundSubmissionId && x.CurrentStatusId != (int)Enums.CurrentStatus.Reversal).FirstOrDefault();
                if (child != null)
                    boundSubmissionId = child.SubmissionId;
                else
                    IsFound = false;
            }

            return boundSubmissionId;
        }


        #region
        //private void SetInstallmentSurchargeNewColumnName(DataTable dt)
        //{
        //    dt.Columns["Account_ID"].ColumnName = "Account ID";
        //    dt.Columns["policy_number"].ColumnName = "Policy Number";
        //    dt.Columns["Insured_Name"].ColumnName = "Insured Name";
        //    dt.Columns["Trans_Type"].ColumnName = "Trans Type";
        //    dt.Columns["Audit_No"].ColumnName = "Audit No";
        //    dt.Columns["Allocation_No__including_RA_"].ColumnName = "Allocation No (including RA)";
        //    dt.Columns["RecoupableSurcharges"].ColumnName = "RecoupableSurcharges";
        //    dt.Columns["installmentfee"].ColumnName = "Installment Fee";
        //    dt.Columns["surcharge_Desc"].ColumnName = "surcharge Desc";
        //    dt.Columns["surcharge_State"].ColumnName = "surcharge State";
        //    dt.Columns["Allocated_with__Audit_No_"].ColumnName = "Allocated with (Audit No)";
        //    dt.Columns["Bill_date"].ColumnName = "Billing Date / Entry Date";
        //    dt.Columns["DueDate"].ColumnName = "Due Date / Transaction Date";
        //    dt.Columns["origAmount"].ColumnName = "orig Amount";
        //    dt.Columns["Allocated_Amount"].ColumnName = "Allocated Amount";
        //    dt.Columns["description"].ColumnName = "description";
        //    dt.Columns["PaymentRef"].ColumnName = "PaymentRef";
        //    dt.Columns["Journal_No"].ColumnName = "Journal No";
        //}


        public DataTable GetExportData(int? regionId)
        {
            DataAccessLayer objDal = new DataAccessLayer();

            DataTable data = new DataTable();
            Dictionary<string, string> spParameters = new Dictionary<string, string>();
            spParameters.Add("@regionId", regionId.ToString());
            data = objDal.ExecuteSPWithReturnDataTableWithParameter("[dbo].[USP_ExportSubmission]", spParameters);
            return data;
        }

        #endregion
    }
}
