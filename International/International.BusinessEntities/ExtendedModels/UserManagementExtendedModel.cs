using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace International.BusinessEntities.Models
{
    [MetadataType(typeof(GroupModelMeta))]
    public partial class GroupModel
    {
        public List<RightsToRoleModel> RightsToRoleList { get; set; }
    }

    public class GroupModelMeta
    {
        [Required(ErrorMessage = "Please enter a valid Group Name")]
        [RegularExpression(@"^[-a-zA-Z \s]*", ErrorMessage = "Please enter a valid Group Name")]
        public string GroupName { get; set; }
    }

    public class GroupFilterModel
    {
        public string GroupName { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }

    }

    public partial class RightsToRoleModel
    {
        public string RoleName { get; set; }
        public string ModuleName { get; set; }
        public int ParentId { get; set; }
        public int? Sort { get; set; }
    }


    public partial class UserRegionModel
    {

        public string label { get; set; }
        public List<SelectListItem> UserRegionList { get; set; }



    }


    public class UserRights
    {

        public int RoleId { get; set; }
        public int? Sort { get; set; }
        public string RoleName { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int Rights { get; set; }
        public int MinRight { get; set; }
        public int ParentId { get; set; }
        public bool IsMenu { get; set; }
    }





}
