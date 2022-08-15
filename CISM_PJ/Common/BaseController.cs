using CISM_PJ.Helpers;
using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CISM_PJ.Common
{
    public class BaseController : Controller
    {
        protected Entities db;
        
        protected LoginSessionModel login_model;
        private string area, controller, action;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            login_model = SessionHelper.Get<LoginSessionModel>("Auth_Info");
            if (login_model != null)
            {
                var descriptor = filterContext.ActionDescriptor;
                area = filterContext.RouteData.DataTokens["area"] != null ? filterContext.RouteData.DataTokens["area"].ToString() : "";
                controller = descriptor.ControllerDescriptor.ControllerName;
                action = descriptor.ActionName;
                // db = new CISM_Entities(DB_Utility.DecryptStringAES(login_model.db_conn));
                db = new Entities();
            }
            else
            {
                filterContext.Result = RedirectToAction("Index", "Login", new { area = "Admin" });
                return;
            }
        }
        protected int GetUserID()
        {
            return login_model.user_id;
        }
        protected int GetRoleID()
        {
            return Convert.ToInt32(login_model.role_id);
        }
        protected string GetUserName()
        {
            return login_model.user_name;
        }
        protected string GetUserInitial()
        {
            return login_model.loginInitail;
        }
        protected string GetPreviousUrl()
        {
            return System.Web.HttpContext.Current?.Request?.UrlReferrer?.AbsolutePath;
        }
        protected string GetUserIDStr()
        {
            return $"{login_model.user_id}";
        }
        protected bool CheckECSSAUser()
        {
            return Convert.ToBoolean(login_model.is_sa_use);
        }
        protected Guid? GetUserRole()
        {
            return login_model.role_id;
        }
        #region Program Permission
        public void Get_Authorization_byMenuID(string menu_ID = "")
        {
            int menuID = 0;
            if (string.IsNullOrEmpty(menu_ID))
            {
                menuID = Convert.ToInt32(Request.QueryString["MenuID"].ToString());
                //menuID = Convert.ToInt32(49);
            }
            else
            {
                menuID = Convert.ToInt32(menu_ID);
            }

            bool ECSSA_User = CheckECSSAUser();
            var roleID = GetUserRole();
            var all_aut = Get_All_Authorization_byMenuID(menuID, ECSSA_User, roleID);
            ViewBag.menuID = menuID;
            ViewBag.ProgramPermission = all_aut;
        }
        public List<MenuAuthorizationModel> Get_All_Authorization_byMenuID(int? menuID, bool ECSSA_User,Guid? roleID)
        {
            var menuAuthList = db.PermissionTypes
            .Select(x => new MenuAuthorizationModel
            {
                perms_type_code = x.permissionTypeName,
                perms_type_name = x.permissionTypeName,
                IsSaUse = ECSSA_User,
                perList = (from rpd in db.RolePermissionDetails
                           join rd in db.RolePermissions on rpd.rolePermissionId equals rd.rolePermissionId
                           where rd.menuID == menuID && rd.roleId == roleID && rpd.permissionTypeId == x.permissionTypeId
                           select new RolePermissionTypeViewModel
                           {
                               PermissionTypeId = rpd.permissionTypeId
                           }).ToList(),
                isGrant = false
            }).ToList();
            var ll = menuAuthList.Select(x => new MenuAuthorizationModel()
            {
                perms_type_code = x.perms_type_code,
                perms_type_name = x.perms_type_name,
                isGrant = x.IsSaUse ? true : (x.perList != null ? (x.perList.Count > 0 ? true : false) : false)
            }).ToList();
            return ll;
        }
      

        //public List<MenuAuthorizationModel> Get_All_Program_Authorization_ByMenuID(int menuID)
        //{
        //    int role_id = GetRoleID();

        //    List<MenuAuthorizationModel> aut = (from r in db.a00_role
        //                                        join rp in db.a00_role_perms on r.role_id equals rp.role_id
        //                                        join m in db.a00_menumst on rp.menu_id equals m.menuid
        //                                        join rpi in db.a00_role_perms_item on rp.role_perms_id equals rpi.role_perms_id
        //                                        join pt in db.a00_perms_type on rpi.perms_type_id equals pt.perms_type_id
        //                                        where r.role_id == role_id
        //                                        && rp.menu_id == menuID
        //                                        select new MenuAuthorizationModel
        //                                        {
        //                                            menu_id = m.menuid,
        //                                            perms_type_id = pt.perms_type_id,
        //                                            perms_type_code = pt.perms_type_code,
        //                                            perms_type_name = pt.perms_type_name,
        //                                            role_id = r.role_id,
        //                                            role_name = r.role_code,
        //                                            menu_name = m.menu_name
        //                                        }).ToList();

        //    return aut;
        //}

        public int Get_MenuIDBy_ActionController(string area, string controller, string action)
        {
            try
            {
                Menu menu_info = db.Menus.Where(x => x.area == area && x.controller_name == controller
                && x.action_name == action).FirstOrDefault();
                if (menu_info != null)
                    return menu_info.menuid;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
