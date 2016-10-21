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
    
    public partial class ProductLineSubType
    {
        public ProductLineSubType()
        {
            this.BrokerProductLineSubTypes = new HashSet<BrokerProductLineSubType>();
            this.UnderwriterProductSubLines = new HashSet<UnderwriterProductSubLine>();
            this.SectionCodes = new HashSet<SectionCode>();
        }
    
        public int ProductLineSubTypeId { get; set; }
        public string ProductLineSubTypeName { get; set; }
        public string ProductLineSubTypeDescription { get; set; }
        public short ProductLineId { get; set; }
        public string SourceSystemName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
        public string Code { get; set; }
    
        public virtual ICollection<BrokerProductLineSubType> BrokerProductLineSubTypes { get; set; }
        public virtual ICollection<UnderwriterProductSubLine> UnderwriterProductSubLines { get; set; }
        public virtual ProductLine ProductLine { get; set; }
        public virtual ICollection<SectionCode> SectionCodes { get; set; }
    }
}
