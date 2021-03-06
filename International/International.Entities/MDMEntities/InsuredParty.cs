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
    
    public partial class InsuredParty
    {
        public InsuredParty()
        {
            this.ContactPersonInsureds = new HashSet<ContactPersonInsured>();
            this.InsuredAlias = new HashSet<InsuredAlia>();
        }
    
        public System.Guid PartyId { get; set; }
        public string Name { get; set; }
        public Nullable<int> AdvisenId { get; set; }
        public string DBNumber { get; set; }
        public Nullable<System.DateTime> InActivationDate { get; set; }
        public string Status { get; set; }
        public Nullable<long> ParentInsuredId { get; set; }
        public bool IsActive { get; set; }
        public string SourceSystemName { get; set; }
        public string SourceSystemId { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
        public string NAICCode { get; set; }
        public string SICCode { get; set; }
        public Nullable<int> GUPAdvisenID { get; set; }
        public Nullable<System.Guid> GUPId { get; set; }
        public Nullable<bool> IsGUP { get; set; }
    
        public virtual ICollection<ContactPersonInsured> ContactPersonInsureds { get; set; }
        public virtual ICollection<InsuredAlia> InsuredAlias { get; set; }
    }
}
