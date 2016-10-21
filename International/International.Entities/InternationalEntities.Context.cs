﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace International.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class InternationalSubmissionEntities : DbContext
    {
        public InternationalSubmissionEntities()
            : base("name=InternationalSubmissionEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Right> Rights { get; set; }
        public virtual DbSet<RightsToRole> RightsToRoles { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<UserSessionLog> UserSessionLogs { get; set; }
        public virtual DbSet<AdditionalInsured> AdditionalInsureds { get; set; }
        public virtual DbSet<LayerDetail> LayerDetails { get; set; }
        public virtual DbSet<SubmissionHeader> SubmissionHeaders { get; set; }
        public virtual DbSet<UserRegion> UserRegions { get; set; }
        public virtual DbSet<ProjectDetail> ProjectDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<SubModule> SubModules { get; set; }
        public virtual DbSet<QCStatusHistory> QCStatusHistories { get; set; }
        public virtual DbSet<CoverageDetail> CoverageDetails { get; set; }
        public virtual DbSet<AdditionalCedant> AdditionalCedants { get; set; }
        public virtual DbSet<PolicyDetail> PolicyDetails { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<PremiumDetail> PremiumDetails { get; set; }
        public virtual DbSet<SubmissionCoverage> SubmissionCoverages { get; set; }
    
        public virtual ObjectResult<USP_GetGroups_Result> USP_GetGroups(string groupName, Nullable<int> status, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var groupNameParameter = groupName != null ?
                new ObjectParameter("GroupName", groupName) :
                new ObjectParameter("GroupName", typeof(string));
    
            var statusParameter = status.HasValue ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GetGroups_Result>("USP_GetGroups", groupNameParameter, statusParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<string> sp_GeneratePassword(Nullable<int> length)
        {
            var lengthParameter = length.HasValue ?
                new ObjectParameter("Length", length) :
                new ObjectParameter("Length", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_GeneratePassword", lengthParameter);
        }
    
        public virtual ObjectResult<USP_GetUsers_Result> USP_GetUsers(string userName, Nullable<int> status, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var statusParameter = status.HasValue ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GetUsers_Result>("USP_GetUsers", userNameParameter, statusParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual int usp_CreateAmendment(Nullable<System.Guid> submissionId, Nullable<int> userid)
        {
            var submissionIdParameter = submissionId.HasValue ?
                new ObjectParameter("SubmissionId", submissionId) :
                new ObjectParameter("SubmissionId", typeof(System.Guid));
    
            var useridParameter = userid.HasValue ?
                new ObjectParameter("Userid", userid) :
                new ObjectParameter("Userid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_CreateAmendment", submissionIdParameter, useridParameter);
        }
    
        public virtual int usp_CreateReversal(Nullable<System.Guid> boundSubmissionId, Nullable<int> userid)
        {
            var boundSubmissionIdParameter = boundSubmissionId.HasValue ?
                new ObjectParameter("BoundSubmissionId", boundSubmissionId) :
                new ObjectParameter("BoundSubmissionId", typeof(System.Guid));
    
            var useridParameter = userid.HasValue ?
                new ObjectParameter("Userid", userid) :
                new ObjectParameter("Userid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_CreateReversal", boundSubmissionIdParameter, useridParameter);
        }
    
        public virtual int USP_UpdateQCStatus(Nullable<System.Guid> submissionId, Nullable<int> sStatus, Nullable<int> qCStatus, string qCRemark, Nullable<System.DateTime> qCApprovalDate, Nullable<int> userId)
        {
            var submissionIdParameter = submissionId.HasValue ?
                new ObjectParameter("submissionId", submissionId) :
                new ObjectParameter("submissionId", typeof(System.Guid));
    
            var sStatusParameter = sStatus.HasValue ?
                new ObjectParameter("SStatus", sStatus) :
                new ObjectParameter("SStatus", typeof(int));
    
            var qCStatusParameter = qCStatus.HasValue ?
                new ObjectParameter("QCStatus", qCStatus) :
                new ObjectParameter("QCStatus", typeof(int));
    
            var qCRemarkParameter = qCRemark != null ?
                new ObjectParameter("QCRemark", qCRemark) :
                new ObjectParameter("QCRemark", typeof(string));
    
            var qCApprovalDateParameter = qCApprovalDate.HasValue ?
                new ObjectParameter("QCApprovalDate", qCApprovalDate) :
                new ObjectParameter("QCApprovalDate", typeof(System.DateTime));
    
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("USP_UpdateQCStatus", submissionIdParameter, sStatusParameter, qCStatusParameter, qCRemarkParameter, qCApprovalDateParameter, userIdParameter);
        }
    
        public virtual ObjectResult<USP_ExportSubmission_Result> USP_ExportSubmission(Nullable<int> regionId)
        {
            var regionIdParameter = regionId.HasValue ?
                new ObjectParameter("regionId", regionId) :
                new ObjectParameter("regionId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_ExportSubmission_Result>("USP_ExportSubmission", regionIdParameter);
        }
    
        public virtual ObjectResult<USP_GetSubmissions_Result> USP_GetSubmissions(string submissionNumber, string newRenewalCode, string renewalCode, string policyNumber, string insuredName, Nullable<int> uwID, Nullable<int> pID, Nullable<int> psID, Nullable<int> cID, Nullable<int> qCStatus, Nullable<System.Guid> bID, Nullable<System.Guid> bpID, string pTypes, Nullable<System.DateTime> effDateF, Nullable<System.DateTime> effDateT, Nullable<System.DateTime> proDateF, Nullable<System.DateTime> proDateT, Nullable<System.DateTime> createDateF, Nullable<System.DateTime> createDateT, Nullable<System.DateTime> modifyDateF, Nullable<System.DateTime> modifyDateT, string modifiedBy, string createdBy, Nullable<System.DateTime> qCDateF, Nullable<System.DateTime> qCDateT, string orderBy, string orderDirection, Nullable<int> pageNo, Nullable<int> pageSize, Nullable<int> regionId, ObjectParameter recordCount)
        {
            var submissionNumberParameter = submissionNumber != null ?
                new ObjectParameter("submissionNumber", submissionNumber) :
                new ObjectParameter("submissionNumber", typeof(string));
    
            var newRenewalCodeParameter = newRenewalCode != null ?
                new ObjectParameter("newRenewalCode", newRenewalCode) :
                new ObjectParameter("newRenewalCode", typeof(string));
    
            var renewalCodeParameter = renewalCode != null ?
                new ObjectParameter("renewalCode", renewalCode) :
                new ObjectParameter("renewalCode", typeof(string));
    
            var policyNumberParameter = policyNumber != null ?
                new ObjectParameter("policyNumber", policyNumber) :
                new ObjectParameter("policyNumber", typeof(string));
    
            var insuredNameParameter = insuredName != null ?
                new ObjectParameter("insuredName", insuredName) :
                new ObjectParameter("insuredName", typeof(string));
    
            var uwIDParameter = uwID.HasValue ?
                new ObjectParameter("uwID", uwID) :
                new ObjectParameter("uwID", typeof(int));
    
            var pIDParameter = pID.HasValue ?
                new ObjectParameter("pID", pID) :
                new ObjectParameter("pID", typeof(int));
    
            var psIDParameter = psID.HasValue ?
                new ObjectParameter("psID", psID) :
                new ObjectParameter("psID", typeof(int));
    
            var cIDParameter = cID.HasValue ?
                new ObjectParameter("cID", cID) :
                new ObjectParameter("cID", typeof(int));
    
            var qCStatusParameter = qCStatus.HasValue ?
                new ObjectParameter("QCStatus", qCStatus) :
                new ObjectParameter("QCStatus", typeof(int));
    
            var bIDParameter = bID.HasValue ?
                new ObjectParameter("bID", bID) :
                new ObjectParameter("bID", typeof(System.Guid));
    
            var bpIDParameter = bpID.HasValue ?
                new ObjectParameter("bpID", bpID) :
                new ObjectParameter("bpID", typeof(System.Guid));
    
            var pTypesParameter = pTypes != null ?
                new ObjectParameter("pTypes", pTypes) :
                new ObjectParameter("pTypes", typeof(string));
    
            var effDateFParameter = effDateF.HasValue ?
                new ObjectParameter("effDateF", effDateF) :
                new ObjectParameter("effDateF", typeof(System.DateTime));
    
            var effDateTParameter = effDateT.HasValue ?
                new ObjectParameter("effDateT", effDateT) :
                new ObjectParameter("effDateT", typeof(System.DateTime));
    
            var proDateFParameter = proDateF.HasValue ?
                new ObjectParameter("proDateF", proDateF) :
                new ObjectParameter("proDateF", typeof(System.DateTime));
    
            var proDateTParameter = proDateT.HasValue ?
                new ObjectParameter("proDateT", proDateT) :
                new ObjectParameter("proDateT", typeof(System.DateTime));
    
            var createDateFParameter = createDateF.HasValue ?
                new ObjectParameter("createDateF", createDateF) :
                new ObjectParameter("createDateF", typeof(System.DateTime));
    
            var createDateTParameter = createDateT.HasValue ?
                new ObjectParameter("createDateT", createDateT) :
                new ObjectParameter("createDateT", typeof(System.DateTime));
    
            var modifyDateFParameter = modifyDateF.HasValue ?
                new ObjectParameter("modifyDateF", modifyDateF) :
                new ObjectParameter("modifyDateF", typeof(System.DateTime));
    
            var modifyDateTParameter = modifyDateT.HasValue ?
                new ObjectParameter("modifyDateT", modifyDateT) :
                new ObjectParameter("modifyDateT", typeof(System.DateTime));
    
            var modifiedByParameter = modifiedBy != null ?
                new ObjectParameter("modifiedBy", modifiedBy) :
                new ObjectParameter("modifiedBy", typeof(string));
    
            var createdByParameter = createdBy != null ?
                new ObjectParameter("createdBy", createdBy) :
                new ObjectParameter("createdBy", typeof(string));
    
            var qCDateFParameter = qCDateF.HasValue ?
                new ObjectParameter("QCDateF", qCDateF) :
                new ObjectParameter("QCDateF", typeof(System.DateTime));
    
            var qCDateTParameter = qCDateT.HasValue ?
                new ObjectParameter("QCDateT", qCDateT) :
                new ObjectParameter("QCDateT", typeof(System.DateTime));
    
            var orderByParameter = orderBy != null ?
                new ObjectParameter("orderBy", orderBy) :
                new ObjectParameter("orderBy", typeof(string));
    
            var orderDirectionParameter = orderDirection != null ?
                new ObjectParameter("orderDirection", orderDirection) :
                new ObjectParameter("orderDirection", typeof(string));
    
            var pageNoParameter = pageNo.HasValue ?
                new ObjectParameter("pageNo", pageNo) :
                new ObjectParameter("pageNo", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("pageSize", pageSize) :
                new ObjectParameter("pageSize", typeof(int));
    
            var regionIdParameter = regionId.HasValue ?
                new ObjectParameter("regionId", regionId) :
                new ObjectParameter("regionId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GetSubmissions_Result>("USP_GetSubmissions", submissionNumberParameter, newRenewalCodeParameter, renewalCodeParameter, policyNumberParameter, insuredNameParameter, uwIDParameter, pIDParameter, psIDParameter, cIDParameter, qCStatusParameter, bIDParameter, bpIDParameter, pTypesParameter, effDateFParameter, effDateTParameter, proDateFParameter, proDateTParameter, createDateFParameter, createDateTParameter, modifyDateFParameter, modifyDateTParameter, modifiedByParameter, createdByParameter, qCDateFParameter, qCDateTParameter, orderByParameter, orderDirectionParameter, pageNoParameter, pageSizeParameter, regionIdParameter, recordCount);
        }
    
        public virtual ObjectResult<USP_GetAmendmentSubmissions_Result> USP_GetAmendmentSubmissions(string submissionNumber, Nullable<int> regionId)
        {
            var submissionNumberParameter = submissionNumber != null ?
                new ObjectParameter("submissionNumber", submissionNumber) :
                new ObjectParameter("submissionNumber", typeof(string));
    
            var regionIdParameter = regionId.HasValue ?
                new ObjectParameter("regionId", regionId) :
                new ObjectParameter("regionId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GetAmendmentSubmissions_Result>("USP_GetAmendmentSubmissions", submissionNumberParameter, regionIdParameter);
        }
    
        public virtual ObjectResult<USP_GetAmendmentQC_Result> USP_GetAmendmentQC(string submissionNumber, string newRenewalCode, string renewalCode, string policyNumber, string insuredName, Nullable<int> uwID, Nullable<int> pID, Nullable<int> psID, Nullable<int> cID, Nullable<int> qCStatus, Nullable<System.Guid> bID, Nullable<System.Guid> bpID, string pTypes, Nullable<System.DateTime> effDateF, Nullable<System.DateTime> effDateT, Nullable<System.DateTime> proDateF, Nullable<System.DateTime> proDateT, Nullable<System.DateTime> createDateF, Nullable<System.DateTime> createDateT, Nullable<System.DateTime> modifyDateF, Nullable<System.DateTime> modifyDateT, string modifiedBy, string createdBy, Nullable<System.DateTime> qCDateF, Nullable<System.DateTime> qCDateT, string orderBy, string orderDirection, Nullable<int> pageNo, Nullable<int> pageSize, Nullable<int> regionId, ObjectParameter recordCount)
        {
            var submissionNumberParameter = submissionNumber != null ?
                new ObjectParameter("submissionNumber", submissionNumber) :
                new ObjectParameter("submissionNumber", typeof(string));
    
            var newRenewalCodeParameter = newRenewalCode != null ?
                new ObjectParameter("newRenewalCode", newRenewalCode) :
                new ObjectParameter("newRenewalCode", typeof(string));
    
            var renewalCodeParameter = renewalCode != null ?
                new ObjectParameter("renewalCode", renewalCode) :
                new ObjectParameter("renewalCode", typeof(string));
    
            var policyNumberParameter = policyNumber != null ?
                new ObjectParameter("policyNumber", policyNumber) :
                new ObjectParameter("policyNumber", typeof(string));
    
            var insuredNameParameter = insuredName != null ?
                new ObjectParameter("insuredName", insuredName) :
                new ObjectParameter("insuredName", typeof(string));
    
            var uwIDParameter = uwID.HasValue ?
                new ObjectParameter("uwID", uwID) :
                new ObjectParameter("uwID", typeof(int));
    
            var pIDParameter = pID.HasValue ?
                new ObjectParameter("pID", pID) :
                new ObjectParameter("pID", typeof(int));
    
            var psIDParameter = psID.HasValue ?
                new ObjectParameter("psID", psID) :
                new ObjectParameter("psID", typeof(int));
    
            var cIDParameter = cID.HasValue ?
                new ObjectParameter("cID", cID) :
                new ObjectParameter("cID", typeof(int));
    
            var qCStatusParameter = qCStatus.HasValue ?
                new ObjectParameter("QCStatus", qCStatus) :
                new ObjectParameter("QCStatus", typeof(int));
    
            var bIDParameter = bID.HasValue ?
                new ObjectParameter("bID", bID) :
                new ObjectParameter("bID", typeof(System.Guid));
    
            var bpIDParameter = bpID.HasValue ?
                new ObjectParameter("bpID", bpID) :
                new ObjectParameter("bpID", typeof(System.Guid));
    
            var pTypesParameter = pTypes != null ?
                new ObjectParameter("pTypes", pTypes) :
                new ObjectParameter("pTypes", typeof(string));
    
            var effDateFParameter = effDateF.HasValue ?
                new ObjectParameter("effDateF", effDateF) :
                new ObjectParameter("effDateF", typeof(System.DateTime));
    
            var effDateTParameter = effDateT.HasValue ?
                new ObjectParameter("effDateT", effDateT) :
                new ObjectParameter("effDateT", typeof(System.DateTime));
    
            var proDateFParameter = proDateF.HasValue ?
                new ObjectParameter("proDateF", proDateF) :
                new ObjectParameter("proDateF", typeof(System.DateTime));
    
            var proDateTParameter = proDateT.HasValue ?
                new ObjectParameter("proDateT", proDateT) :
                new ObjectParameter("proDateT", typeof(System.DateTime));
    
            var createDateFParameter = createDateF.HasValue ?
                new ObjectParameter("createDateF", createDateF) :
                new ObjectParameter("createDateF", typeof(System.DateTime));
    
            var createDateTParameter = createDateT.HasValue ?
                new ObjectParameter("createDateT", createDateT) :
                new ObjectParameter("createDateT", typeof(System.DateTime));
    
            var modifyDateFParameter = modifyDateF.HasValue ?
                new ObjectParameter("modifyDateF", modifyDateF) :
                new ObjectParameter("modifyDateF", typeof(System.DateTime));
    
            var modifyDateTParameter = modifyDateT.HasValue ?
                new ObjectParameter("modifyDateT", modifyDateT) :
                new ObjectParameter("modifyDateT", typeof(System.DateTime));
    
            var modifiedByParameter = modifiedBy != null ?
                new ObjectParameter("modifiedBy", modifiedBy) :
                new ObjectParameter("modifiedBy", typeof(string));
    
            var createdByParameter = createdBy != null ?
                new ObjectParameter("createdBy", createdBy) :
                new ObjectParameter("createdBy", typeof(string));
    
            var qCDateFParameter = qCDateF.HasValue ?
                new ObjectParameter("QCDateF", qCDateF) :
                new ObjectParameter("QCDateF", typeof(System.DateTime));
    
            var qCDateTParameter = qCDateT.HasValue ?
                new ObjectParameter("QCDateT", qCDateT) :
                new ObjectParameter("QCDateT", typeof(System.DateTime));
    
            var orderByParameter = orderBy != null ?
                new ObjectParameter("orderBy", orderBy) :
                new ObjectParameter("orderBy", typeof(string));
    
            var orderDirectionParameter = orderDirection != null ?
                new ObjectParameter("orderDirection", orderDirection) :
                new ObjectParameter("orderDirection", typeof(string));
    
            var pageNoParameter = pageNo.HasValue ?
                new ObjectParameter("pageNo", pageNo) :
                new ObjectParameter("pageNo", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("pageSize", pageSize) :
                new ObjectParameter("pageSize", typeof(int));
    
            var regionIdParameter = regionId.HasValue ?
                new ObjectParameter("regionId", regionId) :
                new ObjectParameter("regionId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GetAmendmentQC_Result>("USP_GetAmendmentQC", submissionNumberParameter, newRenewalCodeParameter, renewalCodeParameter, policyNumberParameter, insuredNameParameter, uwIDParameter, pIDParameter, psIDParameter, cIDParameter, qCStatusParameter, bIDParameter, bpIDParameter, pTypesParameter, effDateFParameter, effDateTParameter, proDateFParameter, proDateTParameter, createDateFParameter, createDateTParameter, modifyDateFParameter, modifyDateTParameter, modifiedByParameter, createdByParameter, qCDateFParameter, qCDateTParameter, orderByParameter, orderDirectionParameter, pageNoParameter, pageSizeParameter, regionIdParameter, recordCount);
        }
    
        public virtual ObjectResult<USP_GetSubmissionsQC_Result> USP_GetSubmissionsQC(string submissionNumber, string newRenewalCode, string renewalCode, string policyNumber, string insuredName, Nullable<int> uwID, Nullable<int> pID, Nullable<int> psID, Nullable<int> cID, Nullable<int> qCStatus, Nullable<System.Guid> bID, Nullable<System.Guid> bpID, string pTypes, Nullable<System.DateTime> effDateF, Nullable<System.DateTime> effDateT, Nullable<System.DateTime> proDateF, Nullable<System.DateTime> proDateT, Nullable<System.DateTime> createDateF, Nullable<System.DateTime> createDateT, Nullable<System.DateTime> modifyDateF, Nullable<System.DateTime> modifyDateT, string modifiedBy, string createdBy, Nullable<System.DateTime> qCDateF, Nullable<System.DateTime> qCDateT, string orderBy, string orderDirection, Nullable<int> pageNo, Nullable<int> pageSize, Nullable<int> regionId, ObjectParameter recordCount)
        {
            var submissionNumberParameter = submissionNumber != null ?
                new ObjectParameter("submissionNumber", submissionNumber) :
                new ObjectParameter("submissionNumber", typeof(string));
    
            var newRenewalCodeParameter = newRenewalCode != null ?
                new ObjectParameter("newRenewalCode", newRenewalCode) :
                new ObjectParameter("newRenewalCode", typeof(string));
    
            var renewalCodeParameter = renewalCode != null ?
                new ObjectParameter("renewalCode", renewalCode) :
                new ObjectParameter("renewalCode", typeof(string));
    
            var policyNumberParameter = policyNumber != null ?
                new ObjectParameter("policyNumber", policyNumber) :
                new ObjectParameter("policyNumber", typeof(string));
    
            var insuredNameParameter = insuredName != null ?
                new ObjectParameter("insuredName", insuredName) :
                new ObjectParameter("insuredName", typeof(string));
    
            var uwIDParameter = uwID.HasValue ?
                new ObjectParameter("uwID", uwID) :
                new ObjectParameter("uwID", typeof(int));
    
            var pIDParameter = pID.HasValue ?
                new ObjectParameter("pID", pID) :
                new ObjectParameter("pID", typeof(int));
    
            var psIDParameter = psID.HasValue ?
                new ObjectParameter("psID", psID) :
                new ObjectParameter("psID", typeof(int));
    
            var cIDParameter = cID.HasValue ?
                new ObjectParameter("cID", cID) :
                new ObjectParameter("cID", typeof(int));
    
            var qCStatusParameter = qCStatus.HasValue ?
                new ObjectParameter("QCStatus", qCStatus) :
                new ObjectParameter("QCStatus", typeof(int));
    
            var bIDParameter = bID.HasValue ?
                new ObjectParameter("bID", bID) :
                new ObjectParameter("bID", typeof(System.Guid));
    
            var bpIDParameter = bpID.HasValue ?
                new ObjectParameter("bpID", bpID) :
                new ObjectParameter("bpID", typeof(System.Guid));
    
            var pTypesParameter = pTypes != null ?
                new ObjectParameter("pTypes", pTypes) :
                new ObjectParameter("pTypes", typeof(string));
    
            var effDateFParameter = effDateF.HasValue ?
                new ObjectParameter("effDateF", effDateF) :
                new ObjectParameter("effDateF", typeof(System.DateTime));
    
            var effDateTParameter = effDateT.HasValue ?
                new ObjectParameter("effDateT", effDateT) :
                new ObjectParameter("effDateT", typeof(System.DateTime));
    
            var proDateFParameter = proDateF.HasValue ?
                new ObjectParameter("proDateF", proDateF) :
                new ObjectParameter("proDateF", typeof(System.DateTime));
    
            var proDateTParameter = proDateT.HasValue ?
                new ObjectParameter("proDateT", proDateT) :
                new ObjectParameter("proDateT", typeof(System.DateTime));
    
            var createDateFParameter = createDateF.HasValue ?
                new ObjectParameter("createDateF", createDateF) :
                new ObjectParameter("createDateF", typeof(System.DateTime));
    
            var createDateTParameter = createDateT.HasValue ?
                new ObjectParameter("createDateT", createDateT) :
                new ObjectParameter("createDateT", typeof(System.DateTime));
    
            var modifyDateFParameter = modifyDateF.HasValue ?
                new ObjectParameter("modifyDateF", modifyDateF) :
                new ObjectParameter("modifyDateF", typeof(System.DateTime));
    
            var modifyDateTParameter = modifyDateT.HasValue ?
                new ObjectParameter("modifyDateT", modifyDateT) :
                new ObjectParameter("modifyDateT", typeof(System.DateTime));
    
            var modifiedByParameter = modifiedBy != null ?
                new ObjectParameter("modifiedBy", modifiedBy) :
                new ObjectParameter("modifiedBy", typeof(string));
    
            var createdByParameter = createdBy != null ?
                new ObjectParameter("createdBy", createdBy) :
                new ObjectParameter("createdBy", typeof(string));
    
            var qCDateFParameter = qCDateF.HasValue ?
                new ObjectParameter("QCDateF", qCDateF) :
                new ObjectParameter("QCDateF", typeof(System.DateTime));
    
            var qCDateTParameter = qCDateT.HasValue ?
                new ObjectParameter("QCDateT", qCDateT) :
                new ObjectParameter("QCDateT", typeof(System.DateTime));
    
            var orderByParameter = orderBy != null ?
                new ObjectParameter("orderBy", orderBy) :
                new ObjectParameter("orderBy", typeof(string));
    
            var orderDirectionParameter = orderDirection != null ?
                new ObjectParameter("orderDirection", orderDirection) :
                new ObjectParameter("orderDirection", typeof(string));
    
            var pageNoParameter = pageNo.HasValue ?
                new ObjectParameter("pageNo", pageNo) :
                new ObjectParameter("pageNo", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("pageSize", pageSize) :
                new ObjectParameter("pageSize", typeof(int));
    
            var regionIdParameter = regionId.HasValue ?
                new ObjectParameter("regionId", regionId) :
                new ObjectParameter("regionId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GetSubmissionsQC_Result>("USP_GetSubmissionsQC", submissionNumberParameter, newRenewalCodeParameter, renewalCodeParameter, policyNumberParameter, insuredNameParameter, uwIDParameter, pIDParameter, psIDParameter, cIDParameter, qCStatusParameter, bIDParameter, bpIDParameter, pTypesParameter, effDateFParameter, effDateTParameter, proDateFParameter, proDateTParameter, createDateFParameter, createDateTParameter, modifyDateFParameter, modifyDateTParameter, modifiedByParameter, createdByParameter, qCDateFParameter, qCDateTParameter, orderByParameter, orderDirectionParameter, pageNoParameter, pageSizeParameter, regionIdParameter, recordCount);
        }
    }
}