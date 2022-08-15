using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CISM_PJ.Models;

namespace CISM_PJ.Areas.Setup.Models
{
    public class SubjectViewModel
    {
        public int subject_id { get; set; }
        public string subj_name { get; set; }
        public string description { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public ModelState state { get; set; }
        public static implicit operator SubjectViewModel(Subject data)
        {
            return new SubjectViewModel
            {
                subject_id = data.subject_id,
                subj_name = data.subj_name,
                description = data.description,
                createddate = data.createddate,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}