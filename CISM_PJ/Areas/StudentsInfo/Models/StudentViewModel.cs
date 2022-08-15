using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;

namespace CISM_PJ.Areas.StudentsInfo.Models
{
    public class StudentViewModel
    {
        public System.Guid student_id { get; set; }
        public string reg_no { get; set; }
        public string name { get; set; }
        public System.DateTime dob { get; set; }
        public string sdob { get; set; }
        public Nullable<System.Guid> class_id { get; set; }
        public int citizen_id { get; set; }
        public int paymentmode_id { get; set; }
        public string father_name { get; set; }
        public string mother_name { get; set; }
        public string address { get; set; }
        public string contract_no { get; set; }
        public string contract_no1 { get; set; }
        public string email { get; set; }
        public string remark { get; set; }
        public bool isDelete { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public string sr { get; set; }
        public string class_name { get; set; }
        public Nullable<int> year_id { get; set; }
        public int grade_id { get; set; }
        public string nationality { get; set; }
        public Nullable<System.DateTime> date_of_join { get; set; }
        public Nullable<System.DateTime> date_of_exit { get; set; }
        public string sdate_of_join { get; set; }
        public string sdate_of_exit { get; set; }
        public Nullable<byte> status { get; set; }
        public ModelState state { get; set; }
        public static implicit operator StudentViewModel(Student data)
        {
            return new StudentViewModel
            {
                student_id = data.student_id,
                name = data.name,
                father_name = data.father_name,
                mother_name = data.mother_name,
                reg_no = data.reg_no,
                class_id = data.class_id,
                dob = data.dob,
                citizen_id = data.citizen_id,
                paymentmode_id = data.paymentmode_id,
                address = data.address,
                contract_no = data.contract_no,
                contract_no1 = data.contract_no1,
                email = data.email,
                remark = data.remark,
                year_id = data.year_id,
                grade_id = data.grade_id,
                isDelete = data.isDelete,
                nationality = data.nationality,
                date_of_exit = data.date_of_exit,
                date_of_join = data.date_of_join,
                sdob = ((DateTime)data.dob).ToString("dd/MM/yyyy"),
                sdate_of_exit = data.date_of_exit != null ? ((DateTime)data.date_of_exit).ToString("dd/MM/yyyy") : "",
                sdate_of_join =  ((DateTime)data.date_of_join).ToString("dd/MM/yyyy"),
                status = data.status,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}