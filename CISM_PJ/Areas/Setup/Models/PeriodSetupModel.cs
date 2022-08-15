using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Setup.Models
{
    public class PeriodSetupModel
    {
        public int period_id { get; set; }
        public string period_name { get; set; }
        public string description { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public ModelState state { get; set; }
        public string from_time { get; set; }
        public string to_time { get; set; }
        public string time_period { get; set; }
        public static implicit operator PeriodSetupModel(PeriodSetup data)
        {
            return new PeriodSetupModel
            {
                period_id = data.period_id,
                period_name = data.period_name,
                description = data.description,
                createddate = data.createddate,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser,
                from_time = data.from_time,
                to_time = data.to_time,
                time_period = data.from_time + "-" + data.to_time
        };
        }
    }
}