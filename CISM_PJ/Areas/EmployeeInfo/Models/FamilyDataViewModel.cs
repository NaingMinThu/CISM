using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;

namespace CISM_PJ.Areas.EmployeeInfo.Models
{
    public class FamilyDataViewModel
    {
        public System.Guid family_id { get; set; }
        public System.Guid employee_id { get; set; }
        public string reg_no { get; set; }
        public string name { get; set; }
        public string relationship { get; set; }
        public int age { get; set; }
        public string occupation { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public static implicit operator FamilyDataViewModel(EmployeeFamily data)
        {
            return new FamilyDataViewModel
            {
                family_id = data.family_id,
                employee_id = data.employee_id,
                name = data.name,
                reg_no = data.reg_no,
                relationship = data.relationship,
                age = data.age,
                occupation = data.occupation,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}