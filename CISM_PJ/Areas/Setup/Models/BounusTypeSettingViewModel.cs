using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Setup.Models
{
    public class BounusTypeSettingViewModel
    {
        public Guid bonus_setting_id { get; set; }
        public int emp_type_Id { get; set; }
        public int emp_grade_id { get; set; }
        public string emp_grade_name { get; set; }
        public string emp_type { get; set; }
        public int bonus_id { get; set; }
        public string bonus_name { get; set; }
        public int from_year { get; set; }
        public int to_year { get; set; }
        public decimal from_amt { get; set; }
        public decimal to_amt { get; set; }
        public string description { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }

        public ModelState state { get; set; }
        public static implicit operator BounusTypeSettingViewModel(BonusTypeSetting data)
        {
            return new BounusTypeSettingViewModel
            {
                bonus_setting_id = data.bonus_setting_id,
                emp_type_Id = data.emp_type_Id,
                emp_grade_id = data.emp_grade_id,
                emp_grade_name= data.EmployeeGrade.emp_grade_name,
                emp_type = data.EmployeeType.emp_type,
                bonus_name = data.BonusSetup.bonus_name,
                bonus_id = data.bonus_id,
                //from_year = data.from_year,
                //to_year = data.to_year,
                //from_amt = data.from_amt,
                //to_amt = data.to_amt,
                createddate = data.createddate,
                modifieddate = data.modifieddate,
                createduser = data.createduser
            };
        }
    }
}