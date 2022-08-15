using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;

namespace CISM_PJ.Areas.Admin.Models
{
    public class RolePermissionViewModel
    {
        public Guid? roleId { get; set; }
        public string roleName { get; set; }
        public string description { get; set; }
        public Guid? rolePermissionId { get; set; }
        public short menuId { get; set; }
        public string menuName { get; set; }
        public ModelState state { get; set; }
        public List<RolePermissionTypeViewModel> perTypeList { get; set; }
    }
}