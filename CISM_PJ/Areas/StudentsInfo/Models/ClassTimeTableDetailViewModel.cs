using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.StudentsInfo.Models
{
    public class ClassTimeTableDetailViewModel
    {
        public Guid class_time_table_id { get; set; }
        public int index { get; set; }
        public int subject_id { get; set; }
        public string from_time { get; set; }
        public string to_time { get; set; }
        public string sfrom_time { get; set; }
        public string sto_time { get; set; }
        public byte weekday { get; set; }
        public string subject_name { get; set; }
        public string weekday_name { get; set; }
    }
}