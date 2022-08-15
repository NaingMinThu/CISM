using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Setup.Models
{
    public class EmployeeTypeModel
    {

        public int emp_type_Id { get; set; }
        public string emp_type { get; set; }
        public string description { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public ModelState state { get; set; }
        public static implicit operator EmployeeTypeModel(EmployeeType data)
        {
            return new EmployeeTypeModel
            {
                emp_type_Id = data.emp_type_Id,
                emp_type = data.emp_type,
                description = data.description,
                createddate = data.createddate,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}