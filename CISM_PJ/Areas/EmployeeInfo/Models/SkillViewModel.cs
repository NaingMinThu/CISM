using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;

namespace CISM_PJ.Areas.EmployeeInfo.Models
{
    public class SkillViewModel
    {
        public System.Guid skill_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public static implicit operator SkillViewModel(Skill data)
        {
            return new SkillViewModel
            {
                skill_id = data.skill_id,
                name = data.name,
                description = data.description,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}