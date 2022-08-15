using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;
namespace CISM_PJ.Areas.Setup.Models
{
    public class GradeListbyEmployeeTypeViewModel
    {
        public int gradeType { get; set; }
        public int emp_type_Id { get; set; }
        public int emp_grade_id { get; set; }
        public string emp_type { get; set; }
        public string emp_grade_name { get; set; }
        public ModelState state { get; set; }
    }
}