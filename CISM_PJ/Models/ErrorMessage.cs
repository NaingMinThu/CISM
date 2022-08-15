using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Models
{
    public class ErrorMessage
    {
        public string code { get; set; }
        public string message { get; set; }

        // Default Value 0 for Successful.
        // 0=success, -1=can't save without error, -2=can't save with error, 1=Duplicate
        public int errorcode { get; set; }
        public int svccode { get; set; }
        public Guid quot_id { get; set; }
        public int svc_id { get; set; }
        public List<string> result_list { get; set; }
    }
}