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
    
    public partial class AdditionalCedant
    {
        public int ID { get; set; }
        public System.Guid SubmissionId { get; set; }
        public string CedantName { get; set; }
        public Nullable<short> DomicileCountryId { get; set; }
        public Nullable<short> DomicileStateId { get; set; }
        public string AssumedEntityType { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
        public Nullable<System.Guid> ProcessIdentifier { get; set; }
    
        public virtual Submission Submission { get; set; }
    }
}
