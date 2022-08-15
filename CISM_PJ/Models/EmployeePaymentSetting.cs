//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CISM_PJ.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeePaymentSetting
    {
        public int emp_payment_setting_id { get; set; }
        public int emp_type_Id { get; set; }
        public int emp_grade_id { get; set; }
        public decimal basic_pay { get; set; }
        public decimal probation { get; set; }
        public string increment { get; set; }
        public int currency_id { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
    
        public virtual EmployeeGrade EmployeeGrade { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
    }
}
