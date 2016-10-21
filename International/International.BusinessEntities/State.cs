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
    using System.Collections.Generic;
    
    public partial class StateModel
    {
        public StateModel()
        {
            this.Cities = new List<CityModel>();
        }
    
        public short StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string StateDescription { get; set; }
        public string StateAbreviationCode { get; set; }
        public string ProjectStateCode { get; set; }
        public short CountryId { get; set; }
        public string SourceSystemName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
    
        public virtual List<CityModel> Cities { get; set; }
        public virtual CountryModel Country { get; set; }
    }
}
