using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Setup.Models
{
    public class SubjectbyGradeViewModel
    {
        public Guid subject_id { get; set; }
        public string subj_name { get; set; }
        public string description { get; set; }
    }
}