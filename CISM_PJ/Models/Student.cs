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
    
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.PreviosSchoolRecordByStudents = new HashSet<PreviosSchoolRecordByStudent>();
            this.StudentGradeHistories = new HashSet<StudentGradeHistory>();
        }
    
        public System.Guid student_id { get; set; }
        public string reg_no { get; set; }
        public string name { get; set; }
        public System.DateTime dob { get; set; }
        public int grade_id { get; set; }
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
        public Nullable<int> year_id { get; set; }
        public string nationality { get; set; }
        public Nullable<System.DateTime> date_of_join { get; set; }
        public Nullable<System.DateTime> date_of_exit { get; set; }
        public Nullable<byte> status { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
    
        public virtual CitizenShip CitizenShip { get; set; }
        public virtual ClassByGrade ClassByGrade { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual PaymentMode PaymentMode { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PreviosSchoolRecordByStudent> PreviosSchoolRecordByStudents { get; set; }
        public virtual Year Year { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentGradeHistory> StudentGradeHistories { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
