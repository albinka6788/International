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
    
    public partial class TypeMasterModel
    {
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string TypeDescription { get; set; }
        public string TypeCategory { get; set; }
        public string SourceSystemName { get; set; }
        public short SourceSystemTypeId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime EffectiveStartDate { get; set; }
        public System.DateTime EffectiveEndDate { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
        public Nullable<System.Guid> TypeId { get; set; }
    }
}