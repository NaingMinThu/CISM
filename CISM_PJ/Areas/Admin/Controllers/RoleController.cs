using CISM_PJ.Areas.Admin.Models;
using CISM_PJ.Areas.Setup.Models;
using CISM_PJ.Common;
using CISM_PJ.Models;
using Newtonsoft.Json;
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
    public class RoleController : BaseController
    {
        private ErrorMessage message = new ErrorMessage();
        private Common_Msg comMsg = new Common_Msg();
        private string msg = string.Empty;
        private string errMsg = string.Empty;
        // GET: Admin/Role
        #region Role
        public ActionResult RoleList()
        {
            Get_Authorization_byMenuID();
            List<MenuAuthorizationModel> menu_aut_list = ViewBag.ProgramPermission;
            if (menu_aut_list.Count > 0)
            {
                var list = db.Roles.Where(x => x.isDelete == false).Select(x => new RoleViewModel()
                {
                    roleId = x.roleId,
                    roleName = x.roleName,
                    description = x.description
                }).ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("PageAccessError", "Home");
            }
        }
        public ActionResult RoleEntry()
        {
            Get_Authorization_byMenuID();
            return View();
        }
        [HttpPost]
        public ActionResult RoleMerge(RoleViewModel model, List<RolePermissionViewModel> detailList)
        {
            try
            {
                switch (model.state)
                {
                    case CISM_PJ.Models.ModelState.Added:
                        msg = comMsg.Save_msg;
                        errMsg = comMsg.Save_Err_msg;
                        Role entity = new Role
                        {
                            roleId = Guid.NewGuid(),
                            roleName = model.roleName,
                            description = model.description,
                            createdDate = DateTime.Now,
                            modifiedDate = DateTime.Now,
                            createdUser = GetUserID(),
                            modifiedUser = GetUserID()
                        };
                       

                        #region Detail
                        foreach (RolePermissionViewModel mItem in detailList)
                        {
                            var itemDetailList = mItem.perTypeList.Where(x => x.isCheck == true).ToList();
                            if (itemDetailList.Count > 0)
                            {
                                RolePermission rpEnttiy = new RolePermission
                                {
                                    rolePermissionId = Guid.NewGuid(),
                                    roleId = entity.roleId,
                                    menuID = mItem.menuId,
                                    createdDate = DateTime.Now,
                                    createdUser = GetUserID()
                                };
                                entity.RolePermissions.Add(rpEnttiy);

                                foreach (RolePermissionTypeViewModel dItem in itemDetailList)
                                {
                                    RolePermissionDetail rpdEnttiy = new RolePermissionDetail
                                    {
                                        rolePermissionDetailId = Guid.NewGuid(),
                                        rolePermissionId = rpEnttiy.rolePermissionId,
                                        permissionTypeId = dItem.PermissionTypeId,
                                        createdDate = DateTime.Now,
                                        createdUser = GetUserID()
                                    };
                                    rpEnttiy.RolePermissionDetails.Add(rpdEnttiy);
                                }
                            }
                        }
                        db.Roles.Add(entity);
                        #endregion
                        break;
                    case CISM_PJ.Models.ModelState.Modified:
                        msg = comMsg.Updated_msg;
                        errMsg = comMsg.Updated_Err_msg;
                        var dataInfo = db.Roles.Where(x => x.roleId == model.roleId).FirstOrDefault();
                     
                        if (dataInfo != null)
                        {
                            dataInfo.roleName = model.roleName;
                            dataInfo.description = model.description;
                            dataInfo.modifiedDate = DateTime.Now;
                            dataInfo.modifiedUser = GetUserID();
                        }

                        var rolePermList = db.RolePermissions.Where(x => x.roleId == model.roleId).ToList();
                        db.RolePermissions.RemoveRange(rolePermList);

                        var perIdList = rolePermList.Select(x => x.rolePermissionId).ToList();
                        var rolePermDetailList = db.RolePermissionDetails.Where(x => perIdList.Contains(x.rolePermissionId)).ToList();
                        db.RolePermissionDetails.RemoveRange(rolePermDetailList);

                        #region Detail
                        foreach (RolePermissionViewModel mItem in detailList)
                        {
                            var itemDetailList = mItem.perTypeList.Where(x => x.isCheck == true).ToList();
                            if (itemDetailList.Count > 0)
                            {
                                RolePermission rpEnttiy = new RolePermission
                                {
                                    rolePermissionId = Guid.NewGuid(),
                                    roleId = dataInfo.roleId,
                                    menuID = mItem.menuId,
                                    createdDate = DateTime.Now,
                                    createdUser = GetUserID()
                                };
                                dataInfo.RolePermissions.Add(rpEnttiy);

                                foreach (RolePermissionTypeViewModel dItem in itemDetailList)
                                {
                                    RolePermissionDetail rpdEnttiy = new RolePermissionDetail
                                    {
                                        rolePermissionDetailId = Guid.NewGuid(),
                                        rolePermissionId = rpEnttiy.rolePermissionId,
                                        permissionTypeId = dItem.PermissionTypeId,
                                        createdDate = DateTime.Now,
                                        createdUser = GetUserID()
                                    };
                                    rpEnttiy.RolePermissionDetails.Add(rpdEnttiy);
                                }
                            }
                        }
                        #endregion
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
     
        public ActionResult DeleteRole(Guid roleid)
        {
            msg = comMsg.Deleted_msg;
            errMsg = comMsg.Deleted_Err_msg;
            var dataInfo = db.Roles.Where(x => x.roleId == roleid).FirstOrDefault();
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
            Guid id = string.IsNullOrEmpty(checkDataID) ? Guid.Empty : Guid.Parse(checkDataID);            
            if (id == Guid.Empty)
            {
                isDataExists = await db.Roles.AnyAsync(x => x.isDelete == false && x.roleName.Trim().Equals(checkData, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                isDataExists = await db.Roles.AnyAsync(x => x.isDelete == false && x.roleName.Trim().Equals(checkData, StringComparison.CurrentCultureIgnoreCase) && x.roleId != id);
            }
            if (isDataExists == true)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Role Permission      
        public ActionResult GetPermissionDetail(Guid? roleId)
        {
            var list = (from rp in db.Menus
                        where rp.isactive == true && rp.isprogram == true
                        orderby rp.parent_menuid,rp.sort_seq,rp.menu_name
                        select new RolePermissionViewModel
                        {
                            roleId = roleId,
                            roleName = roleId != null ? db.Roles.Where(x => x.roleId == roleId).Select(x => x.roleName).FirstOrDefault() : "",
                            description = roleId != null ? db.Roles.Where(x => x.roleId == roleId).Select(x => x.description).FirstOrDefault() : "",
                            menuId = rp.menuid,
                            menuName = rp.menu_name,
                            perTypeList = db.PermissionTypes
                            .Select(x => new RolePermissionTypeViewModel()
                            {
                                PermissionTypeId = x.permissionTypeId,
                                PermissionTypeName = x.permissionTypeName,
                                isCheck = roleId != null ? ((from rpp in db.RolePermissions
                                                             join rdd in db.RolePermissionDetails on rpp.rolePermissionId equals rdd.rolePermissionId
                                                             where rpp.menuID == rp.menuid && rpp.roleId == roleId && rdd.permissionTypeId == x.permissionTypeId
                                                             select new
                                                             {
                                                                 permissionTypeId = rdd.permissionTypeId
                                                             }).ToList().Count > 0 ? true : false) : false
                            }).ToList()
                        }).ToList();
            return Json(list);
        }
        #endregion
    }
}