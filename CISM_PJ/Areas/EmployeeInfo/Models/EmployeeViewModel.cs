using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;

namespace CISM_PJ.Areas.EmployeeInfo.Models
{
    public class EmployeeViewModel
    {
        public System.Guid employee_id { get; set; }
        public string staff_no { get; set; }
        public string name { get; set; }
        public System.DateTime dob { get; set; }
        public int emp_type_Id { get; set; }
        public int grade_id { get; set; }
        public string nrc_passport { get; set; }
        public string nationality { get; set; }
        public bool gender { get; set; }
        public bool marital_status { get; set; }
        public string address { get; set; }
        public string contract_no { get; set; }
        public string contract_no1 { get; set; }
        public string email { get; set; }
        public Nullable<byte> status { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public Nullable<int> createduser { get; set; }
        public Nullable<int> modifieduser { get; set; }
        public ModelState state { get; set; }
        public string sdob { get; set; }
        public string remark { get; set; }
        public string emp_type { get; set; }
        public string grade_name { get; set; }
        public static implicit operator EmployeeViewModel(Employee data)
        {
            return new EmployeeViewModel
            {
                employee_id = data.employee_id,
                name = data.name,
                staff_no = data.staff_no,
                emp_type_Id = data.emp_type_Id,
                grade_id = data.grade_id,
                dob = data.dob,
                sdob = ((DateTime)data.dob).ToString("dd/MM/yyyy"),
                nrc_passport = data.nrc_passport,
                nationality = data.nationality,
                gender = data.gender,
                marital_status = data.marital_status,
                address = data.address,
                contract_no = data.contract_no,
                contract_no1 = data.contract_no1,
                email = data.email,
                remark = data.remark,                
                status = data.status,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}