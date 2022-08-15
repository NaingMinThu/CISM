using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Setup.Models
{
    public class EmployeeGradeModel
    {
        public int emp_grade_id { get; set; }
        public string emp_grade_name { get; set; }
        public string description { get; set; }
        public ModelState state { get; set; }
        public static implicit operator EmployeeGradeModel(EmployeeGrade data)
        {
            return new EmployeeGradeModel
            {
                emp_grade_id = data.emp_grade_id,
                emp_grade_name = data.emp_grade_name,
                description = data.description,
            };
        }
    }
}