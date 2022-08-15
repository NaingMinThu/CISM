using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;
namespace CISM_PJ.Areas.StudentsInfo.Models
{
    public class ClassTimeTableViewModel
    {
        public string sr { get; set; }
        public Guid time_table_id { get; set; }
        public int grade_id { get; set; }
        public Guid class_id { get; set; }
        public string calss_name { get; set; }
        public string grade_name { get; set; }
        public DateTime affected_date { get; set; }
        public string saffected_date { get; set; }
        public int year { get; set; }
        public ModelState state { get; set; }
    }
}