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
    
    public partial class CurrencyRegionModel
    {
        public int Currencyid { get; set; }
        public short Regionid { get; set; }
        public int Transactional { get; set; }
        public int Original { get; set; }
        public int Jurisdictional { get; set; }
    
        public virtual CurrencyModel Currency { get; set; }
        public virtual RegionModel Region { get; set; }
    }
}