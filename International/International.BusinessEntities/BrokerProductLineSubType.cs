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
    
    public partial class BrokerProductLineSubTypeModel
    {
        public int Id { get; set; }
        public int ProductLineSubTypeId { get; set; }
        public System.Guid BrokerPartyID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
    
        public virtual BrokerPartyModel BrokerParty { get; set; }
        public virtual ProductLineSubTypeModel ProductLineSubType { get; set; }
    }
}