using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;

namespace CISM_PJ.Areas.EmployeeInfo.Models
{
    public class EmploymentHistoryViewModel
    {
        public System.Guid employment_history_id { get; set; }
        public System.Guid employee_id { get; set; }
        public string reg_no { get; set; }
        public System.DateTime year_from { get; set; }
        public System.DateTime year_to { get; set; }
        public string employer { get; set; }
        public string job_title { get; set; }
        public decimal salary { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public string syear_from { get; set; }
        public string syear_to { get; set; }
        public static implicit operator EmploymentHistoryViewModel(EmploymentHistory data)
        {
            return new EmploymentHistoryViewModel
            {
                employee_id = data.employee_id,
                employment_history_id = data.employment_history_id,
                reg_no = data.reg_no,
                year_from = data.year_from,
                year_to = data.year_to,
                employer = data.employer,
                job_title = data.job_title,
                salary = data.salary,
                syear_from = data.year_from != null ? ((DateTime)data.year_from).ToString("dd/MM/yyyy") : "",
                syear_to = data.year_to != null ? ((DateTime)data.year_to).ToString("dd/MM/yyyy") : "",
            };
        }
    }
}