using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;

namespace CISM_PJ.Areas.Setup.Models
{
    public class ClassofAcademicViewModel
    {
        public int? grade_id { get; set; }
        public Guid class_id { get; set; }
        public string grade_name { get; set; }
        public string class_name { get; set; }
        public string class_list { get; set; }
        public string description { get; set; }
        public int year { get; set; }
        public ModelState state { get; set; }        
        public string Sstate { get; set; }
        public string state_status { get; set; }
        public List<ClassByGrade> classList { get; set; }
    }
}