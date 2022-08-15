using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Setup.Models
{
    public class HolidaySetupModel
    {
        public int holiday_id { get; set; }
        public string name { get; set; }
        public System.DateTime holiday_date { get; set; }
        public string sholiday_date { get; set; }
        public string description { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public ModelState state { get; set; }
        public static implicit operator HolidaySetupModel(Holiday data)
        {
            return new HolidaySetupModel
            {
                holiday_id = data.holiday_id,
                name = data.name,
                holiday_date = data.holiday_date,
                sholiday_date = data.holiday_date.ToString("dd/MM/yyyy"),
                description = data.description,
                createddate = data.createddate,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}