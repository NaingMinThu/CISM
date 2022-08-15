using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;

namespace CISM_PJ.Areas.EmployeeInfo.Models
{
    public class EducationalQualificationViewModel
    {
        public System.Guid education_id { get; set; }
        public System.Guid employee_id { get; set; }
        public string reg_no { get; set; }
        public string qual_reg_no { get; set; }
        public System.DateTime year_from { get; set; }
        public System.DateTime year_to { get; set; }
        public string school { get; set; }
        public string courses { get; set; }
        public string degree_obtained { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public string syear_from { get; set; }
        public string syear_to { get; set; }
        public static implicit operator EducationalQualificationViewModel(EmployeeEducation data)
        {
            return new EducationalQualificationViewModel
            {
                employee_id = data.employee_id,
                education_id = data.education_id,
                reg_no = data.reg_no,
                year_from = data.year_from,
                year_to = data.year_to,
                school = data.school,
                courses = data.courses,
                degree_obtained = data.degree_obtained,
                syear_from = data.year_from != null ? ((DateTime)data.year_from).ToString("dd/MM/yyyy") : "",
                syear_to = data.year_to != null ? ((DateTime)data.year_to).ToString("dd/MM/yyyy") : "",
            };
        }
    }
}