using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;
namespace CISM_PJ.Areas.Setup.Models
{
    public class SubjectOfAcademciViewModel
    {
        public int? grade_id { get; set; }
        public Guid subject_id { get; set; }
        public int year_id { get; set; }
        public string syear { get; set; }
        public string grade_name { get; set; }
        public string subject_name { get; set; }
        public string description { get; set; }
        public ModelState state { get; set; }
        public string Sstate { get; set; }
        public string state_status { get; set; }
    }
}