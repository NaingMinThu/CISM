using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Setup.Models
{
    public class CitizenShipModel
    {
        public int citizen_id { get; set; }
        public string citizen_name { get; set; }
        public string description { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public ModelState state { get; set; }
        public static implicit operator CitizenShipModel(CitizenShip data)
        {
            return new CitizenShipModel
            {
                citizen_id = data.citizen_id,
                citizen_name = data.citizen_name,
                description = data.description,
                createddate = data.createddate,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}