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
    
    public partial class QCStatusHistory
    {
        public int Id { get; set; }
        public System.Guid SubmissionId { get; set; }
        public Nullable<System.DateTime> Date1 { get; set; }
        public Nullable<int> Status1 { get; set; }
        public Nullable<System.DateTime> Date2 { get; set; }
        public Nullable<int> Status2 { get; set; }
        public Nullable<System.DateTime> Date3 { get; set; }
        public Nullable<int> Status3 { get; set; }
        public Nullable<System.DateTime> Date4 { get; set; }
        public Nullable<int> Status4 { get; set; }
        public Nullable<System.DateTime> Date5 { get; set; }
        public Nullable<int> Status5 { get; set; }
        public Nullable<System.DateTime> Date6 { get; set; }
        public Nullable<int> Status6 { get; set; }
        public Nullable<System.DateTime> Date7 { get; set; }
        public Nullable<int> Status7 { get; set; }
        public Nullable<System.DateTime> Date8 { get; set; }
        public Nullable<int> Status8 { get; set; }
        public Nullable<System.DateTime> Date9 { get; set; }
        public Nullable<int> Status9 { get; set; }
        public Nullable<System.DateTime> Date10 { get; set; }
        public Nullable<int> Status10 { get; set; }
        public Nullable<int> StatusCount { get; set; }
    
        public virtual Submission Submission { get; set; }
    }
}
