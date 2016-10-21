//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace International.BusinessEntities.Models
{
    using System;
    
    public partial class SearchInsuredList_Model
    {
        public Nullable<long> Ranks { get; set; }
        public System.Guid PartyId { get; set; }
        public string Name { get; set; }
        public Nullable<int> AdvisenId { get; set; }
        public string DBNumber { get; set; }
        public string NAICCode { get; set; }
        public string SICCode { get; set; }
        public Nullable<int> InsuredAliasId { get; set; }
        public string InsuredAliasName { get; set; }
        public Nullable<System.DateTime> ChildInsFromDate { get; set; }
        public Nullable<System.DateTime> ChildInsToDate { get; set; }
        public Nullable<System.Guid> AddressId { get; set; }
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string Zipcode { get; set; }
        public Nullable<bool> IsPrimaryAddress { get; set; }
        public string AddressType { get; set; }
        public Nullable<int> CityId { get; set; }
        public string CityName { get; set; }
        public Nullable<short> StateId { get; set; }
        public string StateName { get; set; }
        public Nullable<short> CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
