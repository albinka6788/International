using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace International.BusinessEntities.Models
{
    [MetadataType(typeof(UserModelMeta))]
    public partial class UserModel
    {

        public string UserName { get; set; }
        public string UserGroup { get; set; }
        public string Status { get; set; }
      //  public List<DropdownListItems> RoleList { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }

        public string CreratedByName { get; set; }

        public string LastModifiedByName { get; set; }

        public List<UserRights> UserRights { get; set; }

        public List<UserRights> UserModules { get; set; }

        public List<UserRights> UserMenu { get; set; }

        public List<UserRegionModel> RegionList { get; set; }

        public List<UserRegionModel> UserRegionList { get; set; }

        public List<SelectListItem> GroupList { get; set; }

        public Nullable<int> FilterStatus { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public string GroupNo { get; set; }
        public string GroupName { get; set; }
    }

    public class UserModelMeta
    {
        [Required(ErrorMessage = "Please select Group")]
        ///^[ A-Za-z0-9_@./#&+-]*$/
        
        public int GroupId { get; set; }

        public int? UserId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[-a-zA-Z0-9.]*", ErrorMessage = "Special characters are not allowed")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[-a-zA-Z0-9. \s]*", ErrorMessage = "Special characters are not allowed")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Please enter at least 6 characters")]
        public string Password { get; set; }
    }

    public partial class SelectionListModel
    {
        public List<SelectListItem> ProductLine { get; set; }
        public List<SelectListItem> ProductLineSubType { get; set; }
        public List<SelectListItem> NewRenewalList { get; set; }
        public List<SelectListItem> SectionCodeList { get; set; }
        public List<SelectListItem> ProfitCodeList { get; set; }
        public List<SelectListItem> AttachmentTypeList { get; set; }
        public List<SelectListItem> PolicyTypeList { get; set; }
        public List<MultiSelectDropDownItem> CoverageCodeList { get; set; }
        public List<SelectListItem> LayerLevelCoverageCodeList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> DomacileStateList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> DirectAssumedList { get; set; }
        public List<SelectListItem> AssumedEntityList { get; set; }  
        public List<SelectListItem> CountryList { get; set; }
        public List<SelectListItem> CurrencyList { get; set; }
        public List<SelectListItem> ProfitCentreOfficeList { get; set; }
        public List<SelectListItem> AdditionalCedantStateList { get; set; }
        public List<SelectListItem> IncumbentAttackingBrokerList { get; set; }
        public List<SelectListItem> ReasonList { get; set; }
        public List<SelectListItem> RenewableList { get; set; }
        public List<SelectListItem> AdmittedNotAdmittedList { get; set; }
        public List<SelectListItem> CompanyPaperList { get; set; }
        public List<SelectListItem> CompanyPaperNumberList { get; set; }
        public List<SelectListItem> PolicySymbolList { get; set; }
        public List<SelectListItem> SuffixCodeList { get; set; }

        public List<SelectListItem> OriginalCurrencyList { get; set; }
        public List<SelectListItem> TransactionalCurrencyList { get; set; }
        public List<SelectListItem> JurCurrencyList { get; set; }

        public List<SelectListItem> AffliationList { get; set; }
        public List<SelectListItem> MarketSegmentList { get; set; }
        public List<SelectListItem> OffOnShoreList { get; set; }
        public List<SelectListItem> FormTypeList { get; set; }
       
        

    }

    public class MultiSelectDropDownItem
    {
        public int id { get; set; }
        public string label { get; set; }

    }

    [MetadataType(typeof(SubmissionModelMeta))]
    public partial class SubmissionModel
    {      
        //public PremiumDetailModel PremiumDetailModel { get; set; }
        //public PolicyDetailModel PolicyDetailModel { get; set; }

        public string PCUnderwriter { get; set; }

        public string IssueUnderwriter { get; set; }

        public string SelectedPCUnderwriter { get; set; }

        public string SelectedIssueUnderwriter { get; set; }   

        public string Insured { get; set; }

        public string SelectedInsured { get; set; }

        public string Broker { get; set; }
        public string SelectedBroker { get; set; }

        public SearchBrokerEntityByProductLine_Model BrokerDetail { get; set; }

        public SearchInsuredList_Model InsuredDetail { get; set; }

        public SearchBrokerContact_Model BrokerContact { get; set; }

        public string BrokerEntity { get; set; }
        public string SelectedBrokerEntity { get; set; }
        public Guid BrokerId { get; set; }

        public string BrokerContactPerson { get; set; }
        public string SelectedBrokerContactPerson { get; set; }

        public short DomicileCountryId { get; set; }

        public string SixDigitNumber { get; set; }

        public string ParentSubmissionNo { get; set; }

        public string CurrentStatus { get; set; }   

        public Enums.SubmissionProcess CurrentProcess { get; set; }

        public DateTime? ParentEffectiveDate { get; set; }

        public List<MultiSelectDropDownItem> SelectedSubmissionCoverageList { get; set; }
    }



    public partial class SubmissionModelMeta
    {
        [Required(ErrorMessage = "Please enter a valid 6-digit number")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Please enter a valid 6-digit number")]
        [RegularExpression("^([0-9]+$)",ErrorMessage="Please enter numbers only")]
        public string SixDigitNumber { get; set; }

        [Required(ErrorMessage = "Please select New/Renewal")]
        public string NewRenewalTypeCode { get; set; }

        [Required(ErrorMessage = "Please enter Profit Centre Underwriter")]
        public int ProfitCenterUnderWriterId { get; set; }

        [Required(ErrorMessage = "Please enter Profit Centre Underwriter")]
        public string SelectedPCUnderwriter { get; set; }

       

        [Required(ErrorMessage = "Please select Product Line")]
        public short ProductLineTypeId { get; set; }

        [Required(ErrorMessage = "Please select Product Line Subtype")]
        public short ProductLineSubTypeId { get; set; }

        [Required(ErrorMessage = "Please select Current Status")]
        public int CurrentStatusId { get; set; }

        [Required(ErrorMessage = "Please enter Effective Date")]
        public DateTime EffectiveDate { get; set; }

        [Required(ErrorMessage = "Please enter  Expiry Date")]
        public DateTime ExpiryDate { get; set; }

       
        // Insured Details 

        [RegularExpression(@"^([()$a-zA-Z0-9 ]+$)", ErrorMessage = "Please enter valid DBA name")]
        [Required(ErrorMessage = "Please enter a DBA name")]
        public string DBAName { get; set; }

        [Required(ErrorMessage = "Please select Yes / No if DBA Name different")]
        public bool? IsDifferentDBA { get; set; }

        [Required(ErrorMessage = "Please select Direct / Assumed")]
        public string DirectAssumedTypeCode { get; set; }       

        [RegularExpression(@"^([-_.,()/@$a-zA-Z0-9]+$)", ErrorMessage = "Please enter a valid value for Cedant Name")]
        public string CedantName { get; set; }

      

        [Required(ErrorMessage = "Please select Insured Name")]
        public string SelectedInsured { get; set; }
      
        // Broker Details
      

        [Required(ErrorMessage = "Please select Broker Name")]
        public string SelectedBroker { get; set; }

     

        [Required(ErrorMessage = "Please select Broker Entity")]
        public string SelectedBrokerEntity { get; set; }

      

        [Required(ErrorMessage = "Please select Contact Person")]
        public string SelectedBrokerContactPerson { get; set; }

        [Required(ErrorMessage = "Please select value for Incumbent / Attacking")]
        public string IncumbentAttackingBrokerType { get; set; }

        [Required(ErrorMessage = "Please enter Date / Time")]
        public Nullable<System.DateTime> By_Berk_SI_FROM_Broker { get; set; }

        [Required(ErrorMessage = "Please enter Date / Time")]
        public Nullable<System.DateTime> By_India_FROM_Berk_SI { get; set; }

        [Required(ErrorMessage = "Please select Profit Centre Office")]
        public string ProfitCentreOffice { get; set; }

        [Required(ErrorMessage = "Please select Issuing Office")]
        public string IssuingOffice { get; set; }

        [Required(ErrorMessage = "Please select a Reason Code")]
        public Nullable<System.Guid> ReasonID { get; set; }

        [Required(ErrorMessage = "Please enter Process Date")]
        public Nullable<System.DateTime> ProcessDate { get; set; }


        [StringLength(16, MinimumLength = 16, ErrorMessage = "Please enter valid Renewal of (Policy Number)")]
        public string RenewalofPolicyNumber { get; set; }

        [Required(ErrorMessage = "Please select coverage code")]
        public List<MultiSelectDropDownItem> SelectedSubmissionCoverageList { get; set; }

    }


    [MetadataType(typeof(ProjectDetailModelMeta))]
    public partial class ProjectDetailModel
    {

    }

    public class ProjectDetailModelMeta
    {
        [RegularExpression(@"^([-_.,()/@$a-zA-Z0-9 ]+$)", ErrorMessage = "Please enter a valid value for Project Name")]
        public string ProjectName { get; set; }

        [RegularExpression(@"^([-_.,()/@$a-zA-Z0-9 ]+$)", ErrorMessage = "Please enter a valid value for General Contractor")]
        public string GeneralContractorName { get; set; }

        [RegularExpression(@"^([-_.,()/@$a-zA-Z0-9 ]+$)", ErrorMessage = "Please enter a valid value for Project Owner Name")]
        public string ProjectOwnerName { get; set; }

        [RegularExpression(@"^([-_.,()/@$a-zA-Z0-9 ]+$)", ErrorMessage = "Please enter a valid value for Project Street Address")]
        public string ProjectStreetAddress { get; set; }

        
       
        [RegularExpression(@"^([-+]?\d+(\.\d{1,7})?)", ErrorMessage = "Please enter a valid value for Latitude")]
        public string Latitude { get; set; }

        [RegularExpression(@"^([-+]?\d+(\.\d{1,7})?)", ErrorMessage = "Please enter a valid value for Longitude")]
        public string Longitude { get; set; }
    }

    // PremiumDetail ModelMeta ...
    [MetadataType(typeof(PremiumDetailModelMeta))]
    public partial class PremiumDetailModel
    {
       
    }

    public class PremiumDetailModelMeta
    {
        [Required(ErrorMessage = "Please select Exchange rate as on")]
        public Nullable<System.DateTime> ExchangeRateDate { get; set; }
         
        [RangeAttribute(0.00, 100, ErrorMessage = "The value must be between 0 - 100")]
        [RegularExpression(@"^([-+]?\d+(\.\d{1,2})?)", ErrorMessage = "Decimal number with 2 digit are allowed !")]            
        public Nullable<decimal> LayerPercent { get; set; }

        [RangeAttribute(0.00, 100, ErrorMessage = "The value must be between 0 - 100")]
        [RegularExpression(@"^([-+]?\d+(\.\d{1,2})?)", ErrorMessage = "Decimal number with 2 digit are allowed !")]
        public Nullable<decimal> PolicyCommissionPercent { get; set; }
               
        [Required(ErrorMessage = "Please select Original Currency")]
        public string OriginalCurrencyCode { get; set; }

       // [Required(ErrorMessage = "Please enter Layer Limit in Original Currency")]
        public Nullable<decimal> OriginalLayerLimit { get; set; }

        //[Required(ErrorMessage = "Please enter Attachment Point in Original Currency")]
        public Nullable<decimal> OriginalAttachmentPoint { get; set; }

        [Required(ErrorMessage = "Please select Transactional Currency")]
        public string TransactionalCurrencyCode { get; set; }

       // [Required(ErrorMessage = "Please enter Conversion Rate from Original to Transactional Currency")]
        public Nullable<decimal> ConversionRateToTransactional { get; set; }

        //[Required(ErrorMessage = "Please enter Deductible in Transactional Currency")]
        public Nullable<decimal> TransactionalDeductible { get; set; }

        [Required(ErrorMessage = "Please enter Premium in Transactional Currency")]
        public Nullable<decimal> TransactionalGrossPremium { get; set; }

        //[Required(ErrorMessage = "Please enter Collections in Transactional Currency")]
        public Nullable<decimal> TransactionalCollections { get; set; }

        //[Required(ErrorMessage = "Please enter Deductions in Transactional Currency")]
        public Nullable<decimal> TransactionalDeductions { get; set; }

      //  [Required(ErrorMessage = "Please enter Total Insured Value (TIV) in Transactional Currency")]
        public Nullable<decimal> TransactionalTIV { get; set; }

        [Required(ErrorMessage = "Please select Jurisdictional Currency")]
        public string JurCurrencyCode { get; set; }

        
       // [Required(ErrorMessage = "Please enter Conversation Rate from Transactional to Jurisdictional Currency")]
        public Nullable<decimal> JurExchangeRate { get; set; }

        //[Required(ErrorMessage = "Please enter Conversation Rate from Transactional to USD Currency")]
        public Nullable<decimal> USDExchangeRate { get; set; }

   
        
    }

    [MetadataType(typeof(PolicyDetailModelMeta))]
    public partial class PolicyDetailModel
    {

    }

    public class PolicyDetailModelMeta
    {
        [Required(ErrorMessage = "Please enter a valid value of Policy Number")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Please enter a valid Policy Number")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Please enter Bind Date")]
        public Nullable<System.DateTime> BindDate { get; set; }
    }


    [MetadataType(typeof(AdditionalInsuredMetaModel))]
    public partial class AdditionalInsuredModel
    { }

    
    public partial class AdditionalInsuredMetaModel
    {
        [RegularExpression(@"^([-_.,()/@$a-zA-Z0-9 ]+$)", ErrorMessage = "Please enter a valid value for Additional Insured Name")]
        [Required(ErrorMessage = "Please enter Additional Insured Name")]
        public string InsuredName { get; set; }
    }

    [MetadataType(typeof(AdditionalCedantMetaModel))]
    public partial class AdditionalCedantModel
    {
        public string CountryName { get; set; }
        public string Statename { get; set; }
        public string AssumedEntityTypeName { get; set; }
    }

    public partial class AdditionalCedantMetaModel
    {
        [RegularExpression(@"^([-_.,()/@$a-zA-Z0-9 ]+$)", ErrorMessage = "Please enter a valid value for Additional Reinsured Name")]
        [Required(ErrorMessage = "Please enter Additional Reinsured Name")]
        public string CedantName { get; set; }
    }


    public partial class USP_GetSubmissions_Model
    {
        public List<USP_GetSubmissions_Model> AmendmentList { get; set; }
    }
}