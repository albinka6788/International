//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace International.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubModule
    {
        public SubModule()
        {
            this.RightsToRoles = new HashSet<RightsToRole>();
        }
    
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public Nullable<int> Sort { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public string CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public string LastModifiedByUserId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsMenu { get; set; }
        public int MinRight { get; set; }
        public Nullable<bool> IsModule { get; set; }
    
        public virtual Module Module { get; set; }
        public virtual ICollection<RightsToRole> RightsToRoles { get; set; }
    }
}