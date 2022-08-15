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
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Allowances = new HashSet<Allowance>();
            this.EmployeeEducations = new HashSet<EmployeeEducation>();
            this.EmployeeFamilies = new HashSet<EmployeeFamily>();
            this.EmployeeHobbies = new HashSet<EmployeeHobby>();
            this.EmployeePersonalSkills = new HashSet<EmployeePersonalSkill>();
            this.EmploymentHistories = new HashSet<EmploymentHistory>();
        }
    
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
        public string remark { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public Nullable<int> createduser { get; set; }
        public Nullable<int> modifieduser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Allowance> Allowances { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
        public virtual Grade Grade { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeFamily> EmployeeFamilies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeHobby> EmployeeHobbies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeePersonalSkill> EmployeePersonalSkills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmploymentHistory> EmploymentHistories { get; set; }
    }
}
