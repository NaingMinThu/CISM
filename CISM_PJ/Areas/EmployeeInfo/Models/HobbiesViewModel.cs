using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;

namespace CISM_PJ.Areas.EmployeeInfo.Models
{
    public class HobbiesViewModel
    {
        public System.Guid hobbies_id { get; set; }
        public System.Guid employee_id { get; set; }
        public string name { get; set; }
        public static implicit operator HobbiesViewModel(EmployeeHobby data)
        {
            return new HobbiesViewModel
            {
                employee_id = data.employee_id,
                name = data.name,
                hobbies_id = data.hobbies_id
            };
        }
    }
}