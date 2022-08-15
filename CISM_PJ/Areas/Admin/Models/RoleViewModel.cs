using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public Guid roleId { get; set; }
        public string roleName { get; set; } 
        public string description { get; set; } 
        public ModelState state { get; set; }
    }
}