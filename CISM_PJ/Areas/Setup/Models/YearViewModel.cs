using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Setup.Models
{
    public class YearViewModel
    {
        public int year_id { get; set; }
        public int from_year { get; set; }
        public int to_year { get; set; }
        public string year { get; set; }
        public bool isactive { get; set; }
        public ModelState state { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public static implicit operator YearViewModel(Year data)
        {
            return new YearViewModel
            {
                year_id = data.year_id,
                from_year = data.from_year,
                to_year = data.to_year,
                year = data.from_year + "-" + data.to_year,
                createddate = data.createddate,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}