using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Setup.Models
{
    public class EmloyeeTypePaymentSettingViewModel
    {
        public int emp_payment_setting_id { get; set; }
        public int emp_type_Id { get; set; }
        public int emp_grade_id { get; set; }
        public string emp_grade_name { get; set; }
        public string emp_type { get; set; }
        public decimal basic_pay { get; set; }
        public decimal probation { get; set; }
        public string increment { get; set; }
        public int currency_id { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public ModelState state { get; set; }
        public static implicit operator EmloyeeTypePaymentSettingViewModel(EmployeePaymentSetting data)
        {
            return new EmloyeeTypePaymentSettingViewModel
            {
                emp_payment_setting_id = data.emp_payment_setting_id,
                emp_type_Id = data.emp_type_Id,
                emp_grade_id = data.emp_grade_id,
                emp_grade_name= data.EmployeeGrade.emp_grade_name,
                emp_type = data.EmployeeType.emp_type,
                basic_pay = data.basic_pay,
                probation = data.probation,
                increment = data.increment,
                createddate = data.createddate,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                currency_id = data.currency_id
            };
        }
    }
}