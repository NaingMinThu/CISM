using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CISM_PJ.Areas.Admin.Models
{
    public class UserViewModel
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_pwd { get; set; }
        public byte isactive { get; set; }
        public Nullable<System.Guid> roleId { get; set; }
        public bool is_sa_use { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
        public string role_name { get; set; }
        public ModelState state { get; set; }
        public string description { get; set; }
        public static implicit operator UserViewModel(User data)
        {
            return new UserViewModel
            {
                user_id = data.user_id,
                user_name = data.user_name,
                user_pwd = data.user_pwd,
                roleId  = data.roleId,
                role_name = data.Role.roleName,
                description = data.description,
                createddate = data.createddate,
                modifieddate = data.modifieddate,
                createduser = data.createduser,
                modifieduser = data.modifieduser
            };
        }
    }
}