using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Admin.Models
{
    public class MenuViewModel
    {
        public short? MenusID { get; set; }
        [MaxLength(50), MinLength(2)]
        public string MenusName { get; set; }
        [MaxLength(50), MinLength(2)]
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public short? ParentMenuID { get; set; }
        public virtual List<MenuViewModel> ChildMenus { get; set; }
        public virtual List<Permssion_ActionName> MenuActionList { get; set; }
        public virtual List<MenuViewModel> MenuPermssionTypeList { get; set; }
        public virtual List<Permssion_ActionName> AdditionalActionList { get; set; }
        public virtual List<RolePermission> RolePermissions { get; set; }
        public short SortSeq { get; set; }
        public bool Active { get; set; }
        public string ParentMenuName { get; set; }
        public int? RoleID { get; set; }
        public bool isCheck { get; set; }
        public bool isProgram { get; set; }
        public int? PerTypeID { get; set; }
        public int? Index { get; set; }
        public bool? is_SA_Use { get; set; }
        public int? role_pems_id { get; set; }
        public string query_str { get; set; }
        public string menu_icon { get; set; }
    }
    public class Permssion_ActionName
    {
        public string actionname { get; set; }
        public int actionid { get; set; }
        public int? menusID { get; set; }
        public bool isCheck { get; set; }
        public int? ParentMenuID { get; set; }
    }
}