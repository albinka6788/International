using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using International.BusinessEntities.Models;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace International.BusinessLogic.Classes
{
    public static class Extensions
    {
        public static SelectionListModel SelectionListModels { get; set; }

        public static Dictionary<string, string> GetQueryStrings(this HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs()
                          .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        public static string GetQueryString(this HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null)
                return null;

            var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, true) == 0);
            if (string.IsNullOrEmpty(match.Value))
                return null;

            return match.Value;
        }

        public static string GetHeader(this HttpRequestMessage request, string key)
        {
            IEnumerable<string> keys = null;
            if (!request.Headers.TryGetValues(key, out keys))
                return null;

            return keys.First();
        }

        public static string UpperCaseFirstCharacter(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return Regex.Replace(text, "^[a-z]", m => m.Value.ToUpper());
        }

        public static string RemoveExtraWhiteSpace(this string text)
        {
            Regex trimmer = new Regex(@"\s\s+");
            if (String.IsNullOrEmpty(text)) return "";
            return text = trimmer.Replace(text, " ");
        }

        public static string SearchKeyFor(this DataTableRequest request, string columnName)
        {
            var col = request.Columns.FirstOrDefault(o => o.mDataProp == columnName);
            if (col == null) return "";
            else return HttpUtility.HtmlDecode(col.sSearch);
        }

        public static List<SelectListItem> InsertDefault(this List<SelectListItem> list, string text)
        {
            list.InsertDefault(text, null);
            return list;
        }

        public static List<SelectListItem> InsertDefault(this List<SelectListItem> list, string text, string value)
        {
            list.Insert(0, new SelectListItem { Text = text, Value = value });
            return list;
        }

        public static IEnumerable<PropertyChanges> EnumeratePropertyDifferences<T, T1>(this T obj1, T1 obj2)
        {
            var sel = SelectionListModels;
            PropertyInfo[] properties = typeof(T).GetProperties();
           // List<string> changes = new List<string>();
            List<PropertyChanges> changes = new List<PropertyChanges>();

            //string[] IgnoreableProperties = { "ProcessIdentifier", "LastModifedOnDate", "AdditionalCedants", "AdditionalCedants", "AdditionalInsureds", "LayerDetails",
            //                                "PremiumDetail","ProjectDetail","QCStatusHistories","SubmissionHeader","PolicyDetail"};

            foreach (PropertyInfo pi in properties)
            {
                object value1 = typeof(T).GetProperty(pi.Name).GetValue(obj1, null);
                object value2 = typeof(T1).GetProperty(pi.Name).GetValue(obj2, null);

                if (value1 != value2 && (value1 == null || !value1.Equals(value2)))
                {
                    var data = new PropertyChanges { PropertyName = pi.Name, OldValue = value1 == null ? null : value1.ToString(), NewVlaue = value2 == null ? null : value2.ToString() };
                    changes.Add(data);
                    //if (pi.Name == "PCUnderwriter")
                    //{
                    //    var PCUnderwriter = ApplicationContext.Master.GetPCUnderwriterList(submissionModel.ProfitCenterUnderWriterId).FirstOrDefault().UnderwriterName;
                    //    //var SelectedPCUnderwriter = submissionModel.PCUnderwriter;
                    //    //var IssueUnderwriter = submissionModel.IssuingUnderWriterId != 0 ? (ApplicationContext.Master.GetPCUnderwriterList(submissionModel.IssuingUnderWriterId).FirstOrDefault().UnderwriterName) : "";
                    //    //var SelectedIssueUnderwriter = submissionModel.IssueUnderwriter;
                    //}
                    //else if (pi.Name == "ProductLineTypeId")
                    //{
                    //    var val = SelectionListModels.ProductLine.Where(x => x.Value == value1.ToString()).ToList();
                    //    var val1 = SelectionListModels.ProductLine.Where(x => x.Value == value2.ToString()).Select(x => x.Text).ToList();
                    //    changes.Add(string.Format("Property {0} changed from {1} to {2}", pi.Name, val[0].Text, val1));
                    //}
                    //changes.Add(string.Format("Property {0} changed from {1} to {2}", pi.Name, value1, value2));
                }
            }
            return changes;
        }

        public static bool IsTrackPropertieChange(string PropertyName)
        {
            string[] IgnoreableProperties = { "ProcessIdentifier", "LastModifedOnDate", "AdditionalCedants", "AdditionalCedants", "AdditionalInsureds", "LayerDetails",
                                            "PremiumDetail","ProjectDetail","QCStatusHistories","SubmissionHeader","PolicyDetail"};
            if (IgnoreableProperties.Contains(PropertyName))
                return false;            
            else
                return true;

        }

        public static string GetProperties(string PropertyName,string OldValue, string NewValue)
        {
            if (PropertyName == "PCUnderwriter")
            { 
            
            }
            return "";
        }
        
      
    }

    public class PropertyChanges
    {
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewVlaue { get; set; }
    }

}
