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
    
    public partial class ProductLineModel
    {
        public ProductLineModel()
        {
            this.BrokerProductLines = new List<BrokerProductLineModel>();
            this.Coverages = new List<CoverageModel>();
            this.PolicyTypes = new List<PolicyTypeModel>();
            this.ProductLineSubTypes = new List<ProductLineSubTypeModel>();
            this.UnderwriterProductLines = new List<UnderwriterProductLineModel>();
        }
    
        public short ProductLineId { get; set; }
        public string ProductLineName { get; set; }
        public string ProductLineCode { get; set; }
        public string SourceSystemName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
    
        public virtual List<BrokerProductLineModel> BrokerProductLines { get; set; }
        public virtual List<CoverageModel> Coverages { get; set; }
        public virtual List<PolicyTypeModel> PolicyTypes { get; set; }
        public virtual List<ProductLineSubTypeModel> ProductLineSubTypes { get; set; }
        public virtual List<UnderwriterProductLineModel> UnderwriterProductLines { get; set; }
    }
}
