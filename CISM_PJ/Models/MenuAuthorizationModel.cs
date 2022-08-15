using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Models
{
    public class MenuAuthorizationModel
    {
        public string perms_type_code { get; set; }
        public string perms_type_name { get; set; }
        public bool isGrant { get; set; }
        public bool IsSaUse { get; set; }
        public List<RolePermissionTypeViewModel> perList { get; set; }
    }
}