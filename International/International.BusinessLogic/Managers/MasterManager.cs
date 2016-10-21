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
using International.BusinessEntities;
using International.Entities.MDMEntities;

namespace International.BusinessLogic.Managers
{
    public class MasterManager : BaseManager
    {
        public MasterManager() { }
        public MasterManager(InternationalSubmissionEntities Context, MDMEntities mdmContext) : base(Context, mdmContext) { }

        public List<BrokerModel> GetBrokers()
        {
            var list = new List<BrokerModel>();
            list = mdmContext.Brokers.Take(50).ToList().Select(x => new BrokerModel
            {
                BrokerName = x.BrokerName,
                BrokerCode = x.BrokerCode
            }).ToList();
            //return mdmmdmContext.Brokers.ToList().Map<Broker, BrokerModel>();
            //  list = AutoMapper.Mapper.DynamicMap<List<BrokerModel>>(broker);
            return list;
        }

        public List<SelectListItem> GetProductLine(short? ProductLineId)
        {
            return mdmContext.ProductLines.Where(x => x.IsActive).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.ProductLineName,
                                     Value = x.ProductLineId.ToString(),
                                     Selected = ProductLineId == x.ProductLineId
                                 }).OrderBy(x => x.Text).ToList();
        }

        public List<SelectListItem> GetProductLineSubType(short ProductLineId, short? ProductLineSubTypeId)
        {
            return mdmContext.ProductLineSubTypes.Where(x => x.IsActive && x.ProductLineId == ProductLineId).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.ProductLineSubTypeName,
                                     Value = x.ProductLineSubTypeId.ToString(),
                                     Selected = ProductLineSubTypeId == x.ProductLineSubTypeId
                                 }).OrderBy(x => x.Text).ToList();
        }

        public List<SelectListItem> GetCountry(short? countryId)
        {
            return mdmContext.Countries.Where(x => (x.IsActive ?? false)).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.CountryName + " - " + x.CountryNumericCode,
                                     Value = x.CountryId.ToString(),
                                     Selected = countryId == x.CountryId
                                 }).OrderBy(x => x.Text).ToList();
        }

        public List<SelectListItem> GetState(short countryId, short? stateId)
        {
            return mdmContext.States.Where(x => x.CountryId == countryId && (x.IsActive ?? false)).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.StateName + " - " + x.StateCode,
                                     Value = x.StateId.ToString(),
                                     Selected = stateId == x.StateId
                                 }).OrderBy(x => x.Text).ToList();
        }

        public List<SelectListItem> GetCity(short stateId, int? cityId)
        {
            return mdmContext.Cities.Where(x => x.StateId == stateId && (x.IsActive ?? false)).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.CityName + " - " + x.CityCode,
                                     Value = x.CityId.ToString(),
                                     Selected = cityId == x.StateId
                                 }).OrderBy(x => x.Text).ToList();

        }

        public List<SelectListItem> GetProfitCodeList(short sectionCodeId, short? profitCodeId)
        {
            return mdmContext.ProfitCodes.Where(x => x.SectionCodeId == sectionCodeId && (x.IsActive)).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.ProfitCodeName,
                                     Value = x.ProfitCodeId.ToString(),
                                     Selected = profitCodeId == x.ProfitCodeId
                                 }).OrderBy(x => x.Text).ToList();

        }

        public List<SelectListItem> GetSectionCodeList(short productLineSubTypeId, short? sectionCodeId)
        {
            return mdmContext.SectionCodes.Where(x => x.ProductLineSubTypeId == productLineSubTypeId && (x.IsActive)).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.SectionCodeName,
                                     Value = x.SectionCodeId.ToString(),
                                     Selected = sectionCodeId == x.SectionCodeId
                                 }).OrderBy(x => x.Text).ToList();

        }
       
        //public List<SelectListItem> GetCoverageCodeList(int policyTypeId, int coverageId)
        //{
        //    return mdmContext.Coverages.Where(x => x.IsActive && x.PolicyTypeId == policyTypeId).
        //                         Select(x => new SelectListItem()
        //                         {
        //                             Text = x.CoverageName,
        //                             Value = x.CoverageId.ToString(),
        //                             Selected = coverageId == x.CoverageId
        //                         }).OrderBy(x => x.Text).ToList();

        //}

        public List<MultiSelectDropDownItem> GetCoverageCodeList(int policyTypeId, int coverageId)
        {
            return mdmContext.Coverages.Where(x => x.IsActive && x.PolicyTypeId == policyTypeId).
                                 Select(x => new MultiSelectDropDownItem()
                                 {
                                     label = x.CoverageName,
                                     id = x.CoverageId                                   
                                 }).OrderBy(x => x.label).ToList();

        }

        public List<SelectListItem> GetLayerLevelCoverageCodeList(int coverageId)
        {

            return mdmContext.Coverages.GroupBy(x => x.CoverageName).
                               Select(o => new SelectListItem()
                               {
                                   Text = o.Key,
                                   Value = o.Max(c => c.CoverageId).ToString()
                               }).OrderBy(x => x.Text).ToList();

        }


        public List<SelectListItem> GetTypeMaster(string typeCategory, string typeCode)
        {
            return mdmContext.TypeMasters.Where(x => x.TypeCategory == typeCategory && x.SourceSystemName == "INT_SUBM" && x.IsActive).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.TypeName,
                                     Value = x.TypeCode,
                                     Selected = typeCode == x.TypeCode
                                 }).OrderBy(x => x.Text).ToList();

        }

        public string GetTypeMasterValue(string typeCategory, string typeCode)
        {
            var row = mdmContext.TypeMasters.SingleOrDefault(x => x.TypeCategory == typeCategory && x.IsActive && x.SourceSystemName == "INT_SUBM" && x.TypeCode == typeCode);
            var TypeName = row != null ? row.TypeName : String.Empty;
            return TypeName.ToString();
        }

        public List<SelectListItem> GetPolicyTypeList(string AttachmentType, int productLineId, int productLineSubTypeId, int? policyTypeId)
        {
            return mdmContext.GetPolicyTypeList(productLineId, productLineSubTypeId, AttachmentType ?? string.Empty).Select(x => new SelectListItem()
            {
                Text = x.PolicyType,
                Value = x.PolicyTypeId.ToString(),
                Selected = policyTypeId == x.PolicyTypeId
            }).OrderBy(x => x.Text).ToList();
        }


        public List<SelectListItem> GetCurrentStatusList(int statusId, string newRenewalCode, Enums.SubmissionProcess currentProcess)
        {
            var list = new List<SelectListItem>();
            var data = new EnumList();

            if (currentProcess == Enums.SubmissionProcess.CreateAmendment)
            {
                data = ApplicationConstants.SatusList.Where(o => o.Enum == (int)currentProcess).FirstOrDefault();
            }
            else if (statusId != 0)
            {
                data = ApplicationConstants.SatusList.Where(o => o.Enum == statusId).FirstOrDefault();
            }
            else if (!String.IsNullOrEmpty(newRenewalCode))
            {
                var NewRenewalEnum = (Enums.NewRenwal)Enum.Parse(typeof(Enums.NewRenwal), newRenewalCode);
                data = ApplicationConstants.SatusList.Where(o => o.Enum == (int)NewRenewalEnum).FirstOrDefault();
            }


            if (data != null)
                list = data.ListEnum.Select(x => new SelectListItem
                                            {
                                                Text = !string.IsNullOrEmpty(Common.GetDescription((Enums.CurrentStatus)x)) ?
                                                        Common.GetDescription((Enums.CurrentStatus)x) : ((Enums.CurrentStatus)x).ToString(),
                                                Value = ((Int32)x).ToString()
                                            }).ToList();

            return list;

        }

        public List<SelectListItem> GetCurrencyList(string CurrencyCode)
        {
            return mdmContext.Currencies.Where(x => x.IsActive).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.CurrencyName,
                                     Value = x.CurrencyCode.ToString(),
                                     Selected = CurrencyCode == x.CurrencyCode
                                 }).OrderBy(x => x.Text).ToList();

        }
        //CurrencyRegions
        public List<SelectListItem> GetOrginalCurrencyList(string CurrencyCode, short Regionid)
        {
            return mdmContext.CurrencyRegions.Include("Currency").Where(x => x.Currency.IsActive && x.Regionid == Regionid && x.Original == 1).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.Currency.CurrencyName,
                                     Value = x.Currency.CurrencyCode.ToString(),
                                     Selected = CurrencyCode == x.Currency.CurrencyCode
                                 }).OrderBy(x => x.Text).ToList();

        }

        public List<SelectListItem> GetTransactionalCurrencyList(string CurrencyCode, short Regionid)
        {
            return mdmContext.CurrencyRegions.Include("Currency").Where(x => x.Currency.IsActive && x.Regionid == Regionid && x.Transactional == 1).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.Currency.CurrencyName,
                                     Value = x.Currency.CurrencyCode.ToString(),
                                     Selected = CurrencyCode == x.Currency.CurrencyCode
                                 }).OrderBy(x => x.Text).ToList();

        }

        public List<SelectListItem> GetJurCurrencyList(string CurrencyCode, short Regionid)
        {
            return mdmContext.CurrencyRegions.Include("Currency").Where(x => x.Currency.IsActive && x.Regionid == Regionid && x.Jurisdictional == 1).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.Currency.CurrencyName,
                                     Value = x.Currency.CurrencyCode.ToString(),
                                     Selected = CurrencyCode == x.Currency.CurrencyCode
                                 }).OrderBy(x => x.Text).ToList();

        }

        public List<UserRegionModel> GetRegions(int? region)
        {
            return mdmContext.Regions.Where(x => x.IsActive).
                                 Select(x => new UserRegionModel()
                                 {
                                     label = x.RegionName,
                                     id = x.RegionId,

                                 }).OrderBy(x => x.label).ToList();



        }

        public RegionModel GetRegion(int id)
        {
            // var region = 
            var region = (from regions in mdmContext.Regions
                          where regions.RegionId == id
                          select new RegionModel()
                          {
                              RegionCode = regions.RegionCode,
                              RegionId = regions.RegionId,
                              RegionName = regions.RegionName,
                              RegionDescription = regions.RegionDescription
                          }).FirstOrDefault();
            return region;

        }

        public List<SelectListItem> GetProfitCentreOffice(string BranchCode, int? regionId)
        {
            return mdmContext.Branches.Where(x => x.IsActive && x.RegionId == regionId).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.BranchName,
                                     Value = x.BranchCode.ToString(),
                                     Selected = BranchCode == x.BranchCode

                                 }).OrderBy(x => x.Text).ToList();

        }

        public List<SelectListItem> GetReasonCodes(Guid? reasonId)
        {
            return mdmContext.Reasoncodes.Where(x => x.IsActive).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.Description,
                                     Value = x.ReasonID.ToString(),
                                     Selected = reasonId == x.ReasonID

                                 }).OrderBy(x => x.Text).ToList();

        }

        public List<SelectListItem> GetPolicySymbolList(int RegionId)
        {
            return mdmContext.PolicySymbols.Where(x=> x.RegionId == RegionId).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.PolicySymbol1,
                                     Value = x.PolicySymbol1.ToString()                                    
                                 }).OrderBy(x => x.Text).ToList();

        }

        public List<SelectListItem> GetFormTypeList(int productLineId, int formTypeId)
        {
            return mdmContext.FormTypes.Where(x => (x.IsActive ?? false) && x.ProductLineId == productLineId).
                                 Select(x => new SelectListItem()
                                 {
                                     Text = x.FormTypeName,
                                     Value = x.ID.ToString(),
                                     Selected = formTypeId == x.ID
                                 }).OrderBy(x => x.Text).ToList();

        }

        



        #region Underwriter

        public List<SearchUnderwriterByProductLine_Model> GetPCUnderwriterList(string key)
        {
            return GetPCUnderwriterList(key, 0, 0);
        }

        public List<SearchUnderwriterByProductLine_Model> GetPCUnderwriterList(int underwriterId)
        {
            return GetPCUnderwriterList(string.Empty, 0, underwriterId);
        }

        //public List<SearchUnderwriterByProductLine_Model> GetPCUnderwriterList(string key, int regionId)
        //{
        //    return GetPCUnderwriterList(key, regionId, 0, 0);
        //}

        public List<SearchUnderwriterByProductLine_Model> GetPCUnderwriterList(string key, int regionId, int underwriterId)
        {
            return GetPCUnderwriterList(key, regionId, underwriterId, 0);
        }

        public List<SearchUnderwriterByProductLine_Model> GetPCUnderwriterList(string key, int regionId, int underwriterId, int productlineId)
        {
            var uW = AutoMapper.Mapper.DynamicMap<List<SearchUnderwriterByProductLine_Model>>(mdmContext.SearchUnderwriterByProductLine(key, regionId, underwriterId, productlineId).ToList());
            return uW;
        }

        public List<SelectListItem> GetUWProductLine(int underwriterId, int productLineId = 0)
        {
            return GetPCUnderwriterList(underwriterId).Where(x => !string.IsNullOrEmpty(x.ProductLineName)).Select(x => new SelectListItem
            {
                Text = x.ProductLineName,
                Value = x.ProductLineId.ToString(),
                Selected = productLineId == x.ProductLineId
            }).ToList();
        }

        public List<SelectListItem> GetUWProductLineSubType(int underwriterId, short productLineId)
        {
            return GetPCUnderwriterList(string.Empty, 0, underwriterId, productLineId).Where(x => !String.IsNullOrEmpty(x.ProductLineSubTypeName)).Select(x => new SelectListItem
            {
                Text = x.ProductLineSubTypeName,
                Value = x.ProductLineSubTypeId.ToString()
            }).ToList();
        }
        #endregion

        #region Insured
        public List<SearchInsuredList_Model> GetInsuredList(string term)
        {
            return GetInsuredList(term, null);
        }

        public List<SearchInsuredList_Model> GetInsuredList(string term, Guid? insuredPartyId, Enums.InsuredSearch type = Enums.InsuredSearch.Insured)
        {
            var insuredList = (from tbl in mdmContext.SearchInsuredList(term, insuredPartyId, (int)type, 0, null)
                               select new SearchInsuredList_Model
                                   {
                                       AddressId = tbl.AddressId,
                                       Addressline1 = tbl.Addressline1,
                                       Addressline2 = tbl.Addressline2,
                                       AddressType = tbl.AddressType,
                                       AdvisenId = tbl.AdvisenId,
                                       ChildInsFromDate = tbl.ChildInsFromDate,
                                       ChildInsToDate = tbl.ChildInsToDate,
                                       CityId = tbl.CityId,
                                       CityName = tbl.CityName,
                                       CountryId = tbl.CountryId,
                                       CountryName = tbl.CountryName,
                                       DBNumber = tbl.DBNumber,
                                       InsuredAliasId = tbl.InsuredAliasId,
                                       InsuredAliasName = tbl.InsuredAliasName,
                                       IsPrimaryAddress = tbl.IsPrimaryAddress,
                                       NAICCode = tbl.NAICCode,
                                       Name = tbl.Name,
                                       PartyId = tbl.PartyId,
                                       Ranks = tbl.Ranks,
                                       SICCode = tbl.SICCode,
                                       StateId = tbl.StateId,
                                       StateName = tbl.StateName,
                                       Zipcode = tbl.Zipcode
                                   }).ToList();

            return insuredList;
            //return AutoMapper.Mapper.DynamicMap<List<SearchInsuredList_Model>>(insuredList);
           
        }

        public SearchInsuredList_Model GetInsured(Guid? insuredPartyId, Guid? addressId, int childInsuredId = 0, Enums.InsuredSearch type = Enums.InsuredSearch.InsuredAddress)
        {
            var insuredDetail = mdmContext.SearchInsuredList("", insuredPartyId, (Int32)type, childInsuredId, addressId).FirstOrDefault();
            return AutoMapper.Mapper.DynamicMap<SearchInsuredList_Model>(insuredDetail);
        }

        #endregion

        #region Broker

        public List<SearchBrokerByProductLine_Model> SearchBroker(string key, short productLineId, short productlineSubTypeId, Guid? brokerId, DateTime? EffectiveDate, DateTime? ExpiryDate)
        {
            var brokerList = mdmContext.SearchBrokerByProductLine(key, brokerId, productLineId, productlineSubTypeId, EffectiveDate, ExpiryDate).ToList();
            return AutoMapper.Mapper.DynamicMap<List<SearchBrokerByProductLine_Model>>(brokerList);
        }

        public List<SearchBrokerEntityByProductLine_Model> SearchBrokerEntity(string key, short productLineId, short productlineSubTypeId, Guid brokerId, Guid? brokerEntityId)
        {
            var brokerEntityList = mdmContext.SearchBrokerEntityByProductLine(brokerId, key, brokerEntityId, productLineId, productlineSubTypeId, true).ToList();
            return AutoMapper.Mapper.DynamicMap<List<SearchBrokerEntityByProductLine_Model>>(brokerEntityList);
        }

        public SearchBrokerEntityByProductLine_Model GetBrokerEntity(Guid? brokerEntityId)
        {
            var brokerEntity = mdmContext.SearchBrokerEntityByProductLine(null, "", brokerEntityId, 0, 0, true).FirstOrDefault();
            return AutoMapper.Mapper.DynamicMap<SearchBrokerEntityByProductLine_Model>(brokerEntity);
        }

        public List<SearchBrokerContact_Model> SearchBrokerContact(string key, Guid? brokerEntityId, Guid? ContactPersonId)
        {
            var brokerContact = mdmContext.SearchBrokerContact(key, brokerEntityId, ContactPersonId).ToList();
            return AutoMapper.Mapper.DynamicMap<List<SearchBrokerContact_Model>>(brokerContact);
        }

        #endregion
    }
}
