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
    
    public partial class ModuleModel
    {
        public ModuleModel()
        {
            this.SubModules = new List<SubModuleModel>();
        }
    
        public int id { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public Nullable<int> Sort { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public string CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public string LastModifiedByUserId { get; set; }
    
        public virtual List<SubModuleModel> SubModules { get; set; }
    }
}
