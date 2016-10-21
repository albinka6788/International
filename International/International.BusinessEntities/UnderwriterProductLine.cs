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
    
    public partial class UnderwriterProductLineModel
    {
        public int Id { get; set; }
        public short ProductLineId { get; set; }
        public int UnderwriterId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
    
        public virtual UnderwriterModel Underwriter { get; set; }
        public virtual ProductLineModel ProductLine { get; set; }
    }
}