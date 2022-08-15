using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Models
{
    public class LoginSessionModel
    {
        public int user_id { get; set; }
        public int company_id { get; set; }
        public int? emp_id { get; set; }
        public int? dept_id { get; set; }
        public string dept_code { get; set; }
        public int? access_level { get; set; }
        public string company_name { get; set; }
        public string user_name { get; set; }
        public string full_name { get; set; }
        public string user_pwd { get; set; }
        public string db_conn { get; set; }
        public Guid? role_id { get; set; }
        public string loginInitail { get; set; }
        public bool is_sa_use { get; set; }
    }
}