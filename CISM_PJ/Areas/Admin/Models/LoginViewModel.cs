using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Admin.Models
{
    public class LoginViewModel
    {
        public string company_name { get; set; }
        public string user_name { get; set; }
        public string user_pwd { get; set; }
    }
}