using International.BusinessEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace International.BusinessEntities
{
    public static class ApplicationConstants
    {
        public static string ApplicationID = "DKLJ7234ERN34YTN34U7NFMW0RC906";
        public static string ApplicationKey = "KJHE745B8934BSDJKF8RTGNERK489";
        public static string SourceSystemName = "INT_SUBM";



        public static List<Enums.CurrentStatus> RenewalIncludeStatuslist = new List<Enums.CurrentStatus> { Enums.CurrentStatus.Working, Enums.CurrentStatus.RenewalPending };

        public static List<Enums.CurrentStatus> AmendmentStatuslist = new List<Enums.CurrentStatus> { Enums.CurrentStatus.Endorsement, Enums.CurrentStatus.Cancellation };

        public static List<Enums.CurrentStatus> IncludeNewSubmissionStatuslist = new List<Enums.CurrentStatus> { Enums.CurrentStatus.Working };

        public static List<EnumList> SatusList = new List<EnumList>
        {
            new EnumList 
            {
                Enum =(int) Enums.SubmissionProcess.CreateAmendment,
                ListEnum = new List<int>
                        {
                               (int) Enums.CurrentStatus.Endorsement, 
                               (int) Enums.CurrentStatus.Cancellation
                        }                
            },
            new EnumList 
            {
                Enum =(int) Enums.NewRenwal.NEWRENEWAL_R,
                ListEnum = new List<int>
                        {
                               (int) Enums.CurrentStatus.Working, 
                               (int) Enums.CurrentStatus.RenewalPending
                        }                
            },
            new EnumList 
            {
                Enum =(int) Enums.NewRenwal.NEWRENEWAL_N,
                ListEnum = new List<int> {
                   (int) Enums.CurrentStatus.Working
                    }                
            },
            new EnumList 
            {
                Enum =(int) Enums.CurrentStatus.Working,
                ListEnum =  new List<int>
                { 
                    (int) Enums.CurrentStatus.Working,
                    (int) Enums.CurrentStatus.Indicated, 
                    (int) Enums.CurrentStatus.Quoted, 
                    (int) Enums.CurrentStatus.Declined,
                    (int) Enums.CurrentStatus.Blocked, 
                    (int) Enums.CurrentStatus.Bound,
                    (int) Enums.CurrentStatus.Closed
                }
            },
               new EnumList 
            {
               Enum = (int) Enums.CurrentStatus.Indicated,
               ListEnum =  new List<int> 
               { 
                   (int) Enums.CurrentStatus.Working,
                   (int) Enums.CurrentStatus.Indicated,
                   (int) Enums.CurrentStatus.LostIndicated, 
                   (int) Enums.CurrentStatus.Quoted,
                   (int) Enums.CurrentStatus.Blocked, 
                   (int) Enums.CurrentStatus.Bound                 
               }
            },
            new EnumList 
            {
               Enum = (int) Enums.CurrentStatus.Quoted,
               ListEnum =  new List<int>
               {
                   (int) Enums.CurrentStatus.Working,
                   (int) Enums.CurrentStatus.Indicated,   
                   (int) Enums.CurrentStatus.Quoted,
                   (int) Enums.CurrentStatus.LostQuoted,                                
                   (int) Enums.CurrentStatus.Bound 
               }
            },
            new EnumList 
            {
               Enum = (int) Enums.CurrentStatus.LostQuoted,
               ListEnum =  new List<int>
               {
                   (int) Enums.CurrentStatus.Quoted,
                   (int) Enums.CurrentStatus.LostQuoted,
                   (int) Enums.CurrentStatus.Working
               }
            },
             new EnumList 
             {
               Enum = (int) Enums.CurrentStatus.LostIndicated,
               ListEnum =  new List<int> 
               {
                   (int) Enums.CurrentStatus.Working, 
                   (int) Enums.CurrentStatus.Indicated,
                   (int) Enums.CurrentStatus.LostIndicated
               }
             },
             new EnumList 
             {
               Enum = (int) Enums.CurrentStatus.Bound,
               ListEnum =  new List<int> { (int) Enums.CurrentStatus.Bound}
             },
             new EnumList 
             {
               Enum = (int) Enums.CurrentStatus.Declined,
               ListEnum =  new List<int> {
                   (int) Enums.CurrentStatus.Working,
                   (int) Enums.CurrentStatus.Declined
               }
             },
             new EnumList 
             {
               Enum = (int) Enums.CurrentStatus.Blocked,
               ListEnum =  new List<int> {
                   (int) Enums.CurrentStatus.Blocked,
                   (int) Enums.CurrentStatus.Working
               }
             },
             new EnumList 
             {
               Enum = (int) Enums.CurrentStatus.Endorsement,
               ListEnum =  new List<int>
               { 
                   (int) Enums.CurrentStatus.Endorsement,                  
                   (int) Enums.CurrentStatus.Cancellation
               }
             },
             new EnumList 
             {
               Enum = (int) Enums.CurrentStatus.Cancellation,
               ListEnum =  new List<int>
               {                   
                   (int) Enums.CurrentStatus.Cancellation
               }
             },
             new EnumList 
             {
               Enum = (int) Enums.CurrentStatus.RenewalPending,
               ListEnum =  new List<int>
               { 
                   (int) Enums.CurrentStatus.RenewalPending,
                   (int) Enums.CurrentStatus.Working
               }
             },
             new EnumList 
             {
               Enum = (int) Enums.CurrentStatus.Closed,
               ListEnum =  new List<int>
               { 
                   (int) Enums.CurrentStatus.Closed,
                   (int) Enums.CurrentStatus.Working
               }
             },
             new EnumList 
             {
               Enum = (int) Enums.CurrentStatus.ReEntry,
               ListEnum =  new List<int>
               { 
                   (int) Enums.CurrentStatus.ReEntry
               }
             },
             new EnumList 
             {
               Enum = (int) Enums.CurrentStatus.Reversal,
               ListEnum =  new List<int>
               { 
                   (int) Enums.CurrentStatus.Reversal
               }
             }
            

        };

        public static List<SubmissionStatusValidation> SubmissionStatusList = new List<SubmissionStatusValidation>
        {
            new SubmissionStatusValidation 
            {
                Status = Enums.CurrentStatus.Working,
                IncludeStatus =  new List<Enums.CurrentStatus>
                { 
                    Enums.CurrentStatus.Working,
                    Enums.CurrentStatus.Indicated, 
                    Enums.CurrentStatus.Quoted, 
                    Enums.CurrentStatus.Declined,
                    Enums.CurrentStatus.Blocked, 
                    Enums.CurrentStatus.Bound,
                    Enums.CurrentStatus.Closed
                }
            },
            new SubmissionStatusValidation 
            {
               Status = Enums.CurrentStatus.Indicated,
               IncludeStatus =  new List<Enums.CurrentStatus> 
               { 
                   Enums.CurrentStatus.Working,
                   Enums.CurrentStatus.Indicated,
                   Enums.CurrentStatus.LostIndicated, 
                   Enums.CurrentStatus.Quoted,
                   Enums.CurrentStatus.Blocked, 
                   Enums.CurrentStatus.Bound                 
               }
            },
            new SubmissionStatusValidation 
            {
               Status = Enums.CurrentStatus.Quoted,
               IncludeStatus =  new List<Enums.CurrentStatus>
               {
                   Enums.CurrentStatus.Working,
                   Enums.CurrentStatus.Indicated,   
                   Enums.CurrentStatus.Quoted,
                   Enums.CurrentStatus.LostQuoted,                                
                   Enums.CurrentStatus.Bound 
               }
            },
            new SubmissionStatusValidation 
            {
               Status = Enums.CurrentStatus.LostQuoted,
               IncludeStatus =  new List<Enums.CurrentStatus>
               {
                   Enums.CurrentStatus.Quoted,
                   Enums.CurrentStatus.LostQuoted,
                   Enums.CurrentStatus.Working
               }
            },
             new SubmissionStatusValidation 
             {
               Status = Enums.CurrentStatus.LostIndicated,
               IncludeStatus =  new List<Enums.CurrentStatus> 
               {
                   Enums.CurrentStatus.Working, 
                   Enums.CurrentStatus.Indicated,
                   Enums.CurrentStatus.LostIndicated
               }
             },
             new SubmissionStatusValidation 
             {
               Status = Enums.CurrentStatus.Bound,
               IncludeStatus =  new List<Enums.CurrentStatus> { Enums.CurrentStatus.Bound}
             },
             new SubmissionStatusValidation 
             {
               Status = Enums.CurrentStatus.Declined,
               IncludeStatus =  new List<Enums.CurrentStatus> {
                   Enums.CurrentStatus.Working,
                   Enums.CurrentStatus.Declined
               }
             },
             new SubmissionStatusValidation 
             {
               Status = Enums.CurrentStatus.Blocked,
               IncludeStatus =  new List<Enums.CurrentStatus> {
                   Enums.CurrentStatus.Blocked,
                   Enums.CurrentStatus.Working
               }
             },
             new SubmissionStatusValidation 
             {
               Status = Enums.CurrentStatus.Endorsement,
               IncludeStatus =  new List<Enums.CurrentStatus>
               { 
                   Enums.CurrentStatus.Endorsement,
                   Enums.CurrentStatus.Reversal,
                   Enums.CurrentStatus.Cancellation,
                   Enums.CurrentStatus.ReEntry
               }
             },
             new SubmissionStatusValidation 
             {
               Status = Enums.CurrentStatus.RenewalPending,
               IncludeStatus =  new List<Enums.CurrentStatus>
               { 
                   Enums.CurrentStatus.RenewalPending,
                   Enums.CurrentStatus.Working
               }
             },
             new SubmissionStatusValidation 
             {
               Status = Enums.CurrentStatus.Closed,
               IncludeStatus =  new List<Enums.CurrentStatus>
               { 
                   Enums.CurrentStatus.Closed,
                   Enums.CurrentStatus.Working
               }
             },
             new SubmissionStatusValidation 
             {
               Status = Enums.CurrentStatus.ReEntry,
               IncludeStatus =  new List<Enums.CurrentStatus>
               { 
                   Enums.CurrentStatus.ReEntry,
                   Enums.CurrentStatus.Endorsement,
                   Enums.CurrentStatus.Cancellation
               }
             }
        };
    }


}
