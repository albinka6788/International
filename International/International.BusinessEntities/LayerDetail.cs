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
    
    public partial class LayerDetailModel
    {
        public LayerDetailModel()
        {
            this.CoverageDetails = new List<CoverageDetailModel>();
        }
    
        public int LayerId { get; set; }
        public Nullable<System.Guid> SubmissionId { get; set; }
        public string LayerName { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
    
        public virtual List<CoverageDetailModel> CoverageDetails { get; set; }
        public virtual SubmissionModel Submission { get; set; }
    }
}