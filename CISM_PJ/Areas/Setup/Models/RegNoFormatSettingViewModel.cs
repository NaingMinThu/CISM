using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Setup.Models
{
    public class RegNoFormatSettingViewModel
    {
        public int num_id { get; set; }
        public Nullable<int> format_no { get; set; }
        public string doc_control { get; set; }
        public string doc_prefix { get; set; }
        public string doc_format { get; set; }
        public string next_no { get; set; }
        public string end_no { get; set; }
        public string restart_no { get; set; }
        public ModelState state { get; set; }
        public static implicit operator RegNoFormatSettingViewModel(RegNoSetup data)
        {
            return new RegNoFormatSettingViewModel
            {
                num_id = data.num_id,
                format_no = data.format_no,
                doc_control = data.doc_control,
                doc_prefix = data.doc_prefix,
                doc_format = data.doc_format,
                next_no = data.next_no,
                end_no = data.end_no,
                restart_no = data.restart_no
            };
        }
    }
}