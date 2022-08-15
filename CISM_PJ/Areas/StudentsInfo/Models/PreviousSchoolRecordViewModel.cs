using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;
namespace CISM_PJ.Areas.StudentsInfo.Models
{
    public class PreviousSchoolRecordViewModel
    {
        public System.Guid prv_school_id { get; set; }
        public string reg_no { get; set; }
        public System.Guid student_id { get; set; }
        public System.DateTime year_from { get; set; }
        public System.DateTime year_to { get; set; }
        public string syear_from { get; set; }
        public string syear_to { get; set; }
        public string passed_grade { get; set; }
        public string attended_school { get; set; }
        public string country { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public ModelState state { get; set; }
        public static implicit operator PreviousSchoolRecordViewModel(PreviosSchoolRecordByStudent data)
        {
            return new PreviousSchoolRecordViewModel
            {
                student_id = data.student_id,
                prv_school_id = data.prv_school_id,
                reg_no = data.reg_no,
                year_from = data.year_from,
                year_to = data.year_to,
                syear_from = data.year_from != null ? ((DateTime)data.year_from).ToString("dd/MM/yyyy") : "",
                syear_to = data.year_to != null ? ((DateTime)data.year_to).ToString("dd/MM/yyyy") : "",
                attended_school = data.attended_school,
                passed_grade = data.passed_grade,
                country = data.country,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}