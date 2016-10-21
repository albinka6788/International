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
    
    public partial class SectionCode
    {
        public SectionCode()
        {
            this.ProfitCodes = new HashSet<ProfitCode>();
        }
    
        public short SectionCodeId { get; set; }
        public string SectionCodeName { get; set; }
        public string Code { get; set; }
        public string SectionCodeDescription { get; set; }
        public int ProductLineSubTypeId { get; set; }
        public Nullable<short> SectionCodeLookupId { get; set; }
        public string SourceSystemName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
        public Nullable<int> SourceSystemId { get; set; }
    
        public virtual ProductLineSubType ProductLineSubType { get; set; }
        public virtual ICollection<ProfitCode> ProfitCodes { get; set; }
    }
}