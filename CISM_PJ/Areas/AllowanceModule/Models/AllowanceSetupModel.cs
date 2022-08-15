using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.AllowanceModule.Models
{
    public class AllowanceSetupModel
    {
        public System.Guid allowance_id { get; set; }
        public int allowance_type_id { get; set; }
        public System.DateTime date { get; set; }
        public string sdate { get; set; }
        public System.Guid employee_id { get; set; }
        public decimal amount { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public ModelState state { get; set; }
        public string staff_no { get; set; }
        public string emp_name { get; set; }
        public string allowance_name { get; set; }
        public string description { get; set; }
        public static implicit operator AllowanceSetupModel(Allowance data)
        {
            return new AllowanceSetupModel
            {
                allowance_id = data.allowance_id,
                allowance_type_id = data.allowance_type_id,
                date = data.date,
                sdate = data.date.ToString("dd/MM/yyyy"),
                amount = data.amount,
                employee_id = data.employee_id,
                staff_no = data.Employee.staff_no,
                emp_name = data.Employee.name,
                description = data.AllowanceType.description,
                allowance_name = data.AllowanceType.name,
                createddate = data.createddate,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}