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
    
    public partial class Holiday
    {
        public int holiday_id { get; set; }
        public string name { get; set; }
        public System.DateTime holiday_date { get; set; }
        public string description { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
    }
}
