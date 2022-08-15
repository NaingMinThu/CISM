using CISM_PJ.Areas.Admin.Models;
using CISM_PJ.Areas.Setup.Models;
using CISM_PJ.Common;
using CISM_PJ.Helpers;
using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace CISM_PJ.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private ErrorMessage message = new ErrorMessage();
        private Common_Msg comMsg = new Common_Msg();
        private string msg = string.Empty;
        private string errMsg = string.Empty;

        // GET: Admin/User
        public ActionResult UserList()
        {
            Get_Authorization_byMenuID();
            var is_sa_user = CheckECSSAUser();
            List<UserViewModel> list = new List<UserViewModel>();
            if (is_sa_user)
            {
                list = db.Users.Where(x => x.isDelete == false).Select(x => new UserViewModel()
                {
                    roleId = x.roleId,
                    role_name = x.Role.roleName,
                    description = x.description,
                    user_id = x.user_id,
                    user_name = x.user_name,
                    user_pwd = x.user_pwd,
                    is_sa_use = x.is_sa_use
                }).ToList();
            }
            else
            {
                list = db.Users.Where(x => x.isDelete == false && x.is_sa_use == false).Select(x => new UserViewModel()
                {
                    roleId = x.roleId,
                    role_name = x.Role.roleName,
                    description = x.description,
                    user_id = x.user_id,
                    user_name = x.user_name,
                    user_pwd = x.user_pwd,
                    is_sa_use = x.is_sa_use
                }).ToList();
            }

            return View(list);
        }
        public ActionResult UserMerge(UserViewModel model)
        {
            try
            {
                switch (model.state)
                {
                    case CISM_PJ.Models.ModelState.Added:
                        msg = comMsg.Save_msg;
                        errMsg = comMsg.Save_Err_msg;
                        User entity = new User
                        {
                            user_name = model.user_name,
                            user_pwd = DB_Utility.EncryptStringAES(model.user_pwd),
                            description = model.description,
                            roleId = model.roleId,
                            createddate = DateTime.Now,
                            modifieddate = DateTime.Now,
                            createduser= GetUserID(),
                            modifieduser = GetUserID()
                        };
                        db.Users.Add(entity);
                        break;
                    case CISM_PJ.Models.ModelState.Modified:
                        msg = comMsg.Updated_msg;
                        errMsg = comMsg.Updated_Err_msg;
                        var dataInfo = db.Users.Where(x => x.user_id == model.user_id).FirstOrDefault();
                        if (dataInfo != null)
                        {
                            dataInfo.user_name = model.user_name;
                            dataInfo.roleId = model.roleId;
                            dataInfo.description = model.description;
                            dataInfo.user_pwd = DB_Utility.EncryptStringAES(model.user_pwd);
                            dataInfo.modifieddate = DateTime.Now;
                            dataInfo.modifieduser = GetUserID();
                        }
                        break;
                }
                int success = db.SaveChanges();
                if (success > 0)
                {
                    message.message = msg;
                    message.errorcode = comMsg.successcode;
                }
                else
                {
                    message.message = errMsg;
                    message.errorcode = comMsg.errorcode;
                }
            }
            catch (Exception ex)
            {
                message.message = msg;
                message.errorcode = comMsg.errorcode;

            }
            return Json(message);
        }
        public JsonResult GetUserbyID(int? user_id)
        {
            var list = db.Users.Where(x => x.user_id == user_id).Select(x => new UserViewModel()
            {
                roleId = x.roleId,
                role_name = x.Role.roleName,
                user_id = x.user_id,
                user_name = x.user_name,
                user_pwd = x.user_pwd
            }).ToList();
            foreach(var item in list)
            {
                item.user_pwd = DB_Utility.DecryptStringAES(item.user_pwd);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRoleList()
        {
            var list = db.Roles.Where(x => x.isDelete == false).Select(x => new RolePermissionViewModel()
            {
                roleId = x.roleId,
                roleName = x.roleName,
                description = x.description
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteUser(int? user_id)
        {
            msg = comMsg.Deleted_msg;
            errMsg = comMsg.Deleted_Err_msg;
            var dataInfo = db.Users.Where(x => x.user_id == user_id).FirstOrDefault();

            if (dataInfo != null)
            {
                dataInfo.isDelete = true;

                int success = db.SaveChanges();
                if (success > 0)
                {
                    message.message = msg;
                    message.errorcode = comMsg.successcode;
                }
                else
                {
                    message.message = errMsg;
                    message.errorcode = comMsg.errorcode;
                }
            }
            return Json(message);
        }

        [HttpPost]
        public async Task<JsonResult> CheckCode(string checkData, string checkDataID = "")
        {
            if (string.IsNullOrEmpty(checkData))
            {
                throw new ArgumentException("message", nameof(checkData));
            }
            bool isDataExists = false;
            checkData = checkData.Trim().ToUpper();
            short.TryParse(checkDataID, out short id);
            if (id == 0)
            {
                isDataExists = await db.Users.AnyAsync(x => x.isDelete == false && x.user_name.Trim().Equals(checkData, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                isDataExists = await db.Users.AnyAsync(x => x.isDelete == false && x.user_name.Trim().Equals(checkData, StringComparison.CurrentCultureIgnoreCase) && x.user_id != id);
            }
            if (isDataExists == true)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public async Task<JsonResult> CheckDefaultCode(string checkDataID = "", bool isDefault = false)
        //{
        //    bool isDataExists = false;
        //    short.TryParse(checkDataID, out short id);
        //    byte active = (byte)SetupStatus.Active;
        //    if (isDefault)
        //    {
        //        if (id == 0)
        //        {
        //            isDataExists = await db.a00_countrymst.AnyAsync(x => x.isactive == active && x.isdefault == isDefault);
        //        }
        //        else
        //        {
        //            isDataExists = await db.a00_countrymst.AnyAsync(x => x.isactive == active && x.isdefault == isDefault && x.country_id != id);
        //        }
        //        if (isDataExists == true)
        //        {
        //            return Json(false, JsonRequestBehavior.AllowGet);
        //        }
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}