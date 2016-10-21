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
    
    public partial class CoverageModel
    {
        public int CoverageId { get; set; }
        public string CoverageCode { get; set; }
        public string CoverageName { get; set; }
        public string CoverageDescription { get; set; }
        public short ProductLineId { get; set; }
        public string SourceSystemName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
        public Nullable<int> PolicyTypeId { get; set; }
    
        public virtual ProductLineModel ProductLine { get; set; }
    }
}