using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace International.BusinessEntities.Models
{
    public class LoggedInUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string ApplicationKey { get; set; }
        public bool IsAuthenticated { get; set; }
       

        public List<UserRights> UserRights { get; set; }
        public List<UserRights> UserModules { get; set; }
        public List<UserRights> UserMenu { get; set; }

        public RegionModel SubmissionRegion { get; set; } 
    }

    public class Response
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class Response<T> : Response
    {
        public T data { get; set; }
    }

    public class AppException
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public List<string> InnerException { get; set; }
        public Dictionary<string, string> ServerVariables { get; set; }
        public Dictionary<string, string> QueryVariables { get; set; }
        public Dictionary<string, string> FormVariables { get; set; }
    }

    public class DataTableRequest
    {
        public int sEcho { get; set; }
        public int iColumns { get; set; }
        public string sColumns { get; set; }
        public int iDisplayStart { get; set; }
        public int iDisplayLength { get; set; }
        public string sSearch { get; set; }
        public bool bRegex { get; set; }
        public int iSortingCols { get; set; }
        public string sSortDir { get; set; }
        public int iSortCol { get; set; }
        public List<DataTableRequestColumn> Columns { get; set; }
    }

    public class DataTableRequestColumn
    {
        public string mDataProp { get; set; }
        public string sSearch { get; set; }
        public bool bRegex { get; set; }
        public bool bSearchable { get; set; }
        public bool bSortable { get; set; }
    }

    public class DataTableResponse<T> : Response
    {
        public int sEcho { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<T> aaData { get; set; }
        public List<T> amendmentList { get; set; }
    }

    public class DataTableFilterItem
    {
        public string FilterValue { get; set; }
        public string FieldName { get; set; }
    }

    public class DataTableFilterQuery
    {
        public string key { get; set; }
        public string fieldName { get; set; }
        public string orderBy { get; set; }
    }

    public class DataTable10Request
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string sDir { get; set; }
        public string sCol { get; set; }
    }	

    public class SubmissionListingModel
    {
        public SubmissionFilters Filters { get; set; }
        public SubmissionFilterLists FilterLists { get; set; }
    }

    public class SubmissionStatusValidation
    {
        public Enums.CurrentStatus Status { get; set; }
        public List<Enums.CurrentStatus> IncludeStatus { get; set; }
    }

    public class EnumList
    {
        public int Enum { get; set; }
        public List<int> ListEnum { get; set; }
    }

    public class SubmissionFilters : DataTable10Request
    {
        public string SubmissionNumber { get; set; }
        public string NewRenewalCode { get; set; }
        public string RenewalCode { get; set; }
        public int? QCStatus { get; set; }
        public string PolicyNumber { get; set; }
        public string InsuredName { get; set; }
        public int? UwID { get; set; }
        public int? PID { get; set; }
        public int? PsID { get; set; }
        public int? CID { get; set; }
        public Guid? BID { get; set; }
        public Guid? BpID { get; set; }
        public string PTypes { get; set; }
        public DateTime? EffDateF { get; set; }
        public DateTime? EffDateT { get; set; }
        public DateTime? ProDateF { get; set; }
        public DateTime? ProDateT { get; set; }
        public DateTime? CreateDateF { get; set; }
        public DateTime? CreateDateT { get; set; }
        public DateTime? ModifyDateF { get; set; }
        public DateTime? ModifyDateT { get; set; }
        public DateTime? QCDateF { get; set; }
        public DateTime? QCDateT { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class SubmissionFilterLists
    {
        public string SubmissionNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string Insuredname { get; set; }
        public string ProfitCentreOffice { get; set; }
        public string IssuingOffice { get; set; }
        public string EditedBy { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? EffectiveDateF { get; set; }
        public DateTime? EffectiveDateT { get; set; }
        public DateTime? ProcessDateF { get; set; }
        public DateTime? ProcessDateT { get; set; }
        public DateTime? CreateDateF { get; set; }
        public DateTime? CreateDateT { get; set; }
        public DateTime? EditDateF { get; set; }
        public DateTime? EditDateT { get; set; }
        public DateTime? QCDateF { get; set; }
        public DateTime? QCDateT { get; set; }

        public List<SelectListItem> NewRenewal { get; set; }
        public List<SelectListItem> Renewable { get; set; }
        public List<SelectListItem> ProfitCenterUW { get; set; }
        public List<SelectListItem> ProductLine { get; set; }
        public List<SelectListItem> ProductLineSubtype { get; set; }
        public List<SelectListItem> ProfitCentreOffices { get; set; }
        public List<SelectListItem> IssuingOffices { get; set; }
        public List<SelectListItem> PolicyType { get; set; }
        public List<SelectListItem> CurrentStatus { get; set; }
        public List<SelectListItem> Broker { get; set; }
        public List<SelectListItem> BrokerEntity { get; set; }
        public List<SelectListItem> QCStatus { get; set; }
    }
}
