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
    
    public partial class ProjectDetailModel
    {
        public ProjectDetailModel()
        {
            this.Submissions = new List<SubmissionModel>();
        }
    
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string GeneralContractorName { get; set; }
        public string ProjectOwnerName { get; set; }
        public Nullable<short> CountryId { get; set; }
        public Nullable<short> StateId { get; set; }
        public Nullable<int> CityId { get; set; }
        public string ProjectStreetAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<System.Guid> ProcessIdentifier { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
    
        public virtual List<SubmissionModel> Submissions { get; set; }
    }
}
