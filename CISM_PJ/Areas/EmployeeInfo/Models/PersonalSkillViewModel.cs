using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;
namespace CISM_PJ.Areas.EmployeeInfo.Models
{
    public class PersonalSkillViewModel
    {
        public System.Guid personal_skill_id { get; set; }
        public string language { get; set; }
        public string skil_name { get; set; }
        public System.Guid skill_id { get; set; }
        public System.Guid employee_id { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public string reg_no { get; set; }
        public static implicit operator PersonalSkillViewModel(EmployeePersonalSkill data)
        {
            return new PersonalSkillViewModel
            {
                personal_skill_id = data.personal_skill_id,
                language = data.language,
                skill_id = data.skill_id,
                skil_name = data.Skill.name,
                reg_no = data.Employee.staff_no,
                employee_id = data.employee_id,               
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}