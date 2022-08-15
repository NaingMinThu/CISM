using CISM_PJ.Areas.Setup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;
namespace CISM_PJ.Areas.TimeTable.Models
{
    public class ClassTimeTableDetailModel
    {
        public int period_id { get; set; }
        public string period_name { get; set; }
        public string time_period { get; set; }
        public string description { get; set; }
        public List<SubjectbyGradeViewModel> monday_List { get; set; }
        public List<SubjectbyGradeViewModel> tue_List { get; set; }
        public List<SubjectbyGradeViewModel> wed_List { get; set; }
        public List<SubjectbyGradeViewModel> thur_List { get; set; }
        public List<SubjectbyGradeViewModel> fri_List { get; set; }
        public string monday_List_data { get; set; }
        public string tue_List_data { get; set; }
        public string wed_List_data { get; set; }
        public string thur_List_data { get; set; }
        public string fri_List_data { get; set; }
    }
    public class ClassTimeTableModel
    {
        public string sr { get; set; }
        public Guid time_table_id { get; set; }
        public int grade_id { get; set; }
        public int class_id { get; set; }
        public string calss_name { get; set; }
        public string grade_name { get; set; }
        public DateTime affected_date { get; set; }
        public string saffected_date { get; set; }
        public ModelState state { get; set; }
    }
}