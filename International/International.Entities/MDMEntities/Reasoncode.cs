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
    
    public partial class Reasoncode
    {
        public System.Guid ReasonID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string SourceSystemName { get; set; }
        public short SourceSystemId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime LastModifedOnDate { get; set; }
        public int LastModifiedByUserId { get; set; }
        public bool Renewal { get; set; }
        public int StatusId { get; set; }
        public string DetailDescription { get; set; }
    
        public virtual Status Status { get; set; }
        public virtual Status Status1 { get; set; }
    }
}
