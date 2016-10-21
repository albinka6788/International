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
    
    public partial class PolicyType
    {
        public int PolicyTypeId { get; set; }
        public string AttachmentType { get; set; }
        public string PolicyType1 { get; set; }
        public string CoverageVersion { get; set; }
        public string PolicySymbol { get; set; }
        public string PolicySymbolDescription { get; set; }
        public string PolicyCode { get; set; }
        public Nullable<bool> IsDirect { get; set; }
        public Nullable<short> ProductLineId { get; set; }
        public string SourceSystemName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
        public Nullable<int> ProductlineSubtypeId { get; set; }
    
        public virtual ProductLine ProductLine { get; set; }
    }
}
