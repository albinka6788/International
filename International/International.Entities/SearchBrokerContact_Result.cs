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
    
    public partial class SearchBrokerContact_Result
    {
        public System.Guid ContactPersonId { get; set; }
        public System.Guid BrokerPartyId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PartyFunction { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Fax { get; set; }
        public bool IsActive { get; set; }
        public string SourceSystemName { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
    }
}
