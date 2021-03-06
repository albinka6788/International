//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace International.Entities.MDMEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Branch
    {
        public short BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string BranchDescription { get; set; }
        public Nullable<short> RegionId { get; set; }
        public Nullable<int> CityId { get; set; }
        public string SourceSystemName { get; set; }
        public string SourceSystemId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
    
        public virtual City City { get; set; }
        public virtual Region Region { get; set; }
    }
}
