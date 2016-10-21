using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace International.BusinessEntities
{
    public class Enums
    {
        public enum CurrentStatus
        {
            Blocked = 22,
            Bound = 23,
            Cancellation = 24,
            Declined = 26,
            Endorsement = 27,
            Indicated = 29,
            [Description("Lost-Indicated")]
            LostIndicated = 31,
            [Description("Lost-Quoted")]
            LostQuoted = 32,
            Quoted = 34,
            Reversal = 37,
            [Description("Renewal Pending")]
            RenewalPending = 38,
            Working = 40,
            [Description("Re-Entry")]
            ReEntry = 35,
            Closed = 25
        }

        public enum Typecategory
        {
            AttachmentType = 1,
            NewRenewal = 2,
            DirectAssumed = 3,
            AssumedEntity = 4,
            BrokerAttribute = 5,
            AdmittedNotAdmitted = 6,
            CompanyPaper = 7,
            CompanyPaperNumber = 8,
            Suffix = 9,
            Renewable = 10,
            MarketSegment = 11,
            Affiliations=12,
            OffshoreOnshore=13
        }

        public enum LOBs
        {
            Property = 1,
            Casualty = 2,
            ExecProf = 3,
            Healthcare = 4,
            Program = 5,
            Surety = 10
        }

        //public enum AttachmentType
        //{
        //    AT_EXCESS = "Excess",
        //    AT_NL = "AT blank value",
        //    AT_PRIMAR = "Primary",
        //    AT_QUOTAS = "Quota Share"
        //}




        public enum Right
        {
            None = 0,
            View = 1,
            Edit = 2,
            Delete = 3
        }

        public enum StatusCategory
        {
            SubmissionStatus = 1
        }


        public enum NewRenwal
        {
            NEWRENEWAL_N = 1,
            NEWRENEWAL_R = 2
        }

        public enum SubmissionProcess
        {
            [Description("Create Submission")]
            CreateSubmission = 0,
            [Description("Edit Submission")]
            EditSubmission = 1,
            [Description("View Submission")]
            ViewSubmission = 2,
            [Description("Create Amendment")]
            CreateAmendment = 3,
            [Description("Edit Amendment")]
            EditAmendment = 4,
            [Description("Edit Re-Entry")]
            EditReEntry = 5,
            [Description("View Re-Entry")]
            ViewReEntry = 6,
            [Description("View Reversal")]
            ViewReversal = 7,
     
            [Description("Submission QC")]
            SubmissionQC = 8,
            [Description("Amendment QC")]
            AmendmentQC = 9,
           [Description("View Amendment")]
           ViewAmendment= 10
        }

        public enum QCStatus
        {
            Pending = 16,
            Approval = 14
        }

        public enum InsuredSearch
        {
            Insured = 0,
            ChildInsured = 1,
            InsuredAddress = 2
        }

    }
}
