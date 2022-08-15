using CISM_PJ.Areas.Admin.Models;
using CISM_PJ.Helpers;
using CISM_PJ.Models;
using CISM_PJ.Uitls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CISM_PJ.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private ErrorMessage message = new ErrorMessage();
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Login(LoginViewModel model)
        {
            int company_id = 0;
            string db_conn = string.Empty;

            try
            {
                string connstr = ConfigurationManager.AppSettings["CISM_Entities"];//.DecryptStringAES(ConfigurationManager.AppSettings["CISMEntities"]);
                using (var db = new Entities())
                {
                    var user = await db.Users.FirstOrDefaultAsync(x => x.user_name.ToLower() == model.user_name.ToLower() && x.isDelete == false);

                    if (user == null)
                    {
                        message.message = "User Name is wrong!";
                        message.errorcode = -1;
                    }
                    else
                    {
                        //user.user_pwd = Utilities.DecryptStringAES(user.user_pwd);
                        string password = DB_Utility.DecryptStringAES(user.user_pwd);
                        if (password.Equals(model.user_pwd))
                        {
                            LoginSessionModel smodel = new LoginSessionModel
                            {
                                user_id = user.user_id,
                                company_id = company_id,
                                company_name = model.company_name,
                                user_name = user.user_name,
                                user_pwd = user.user_pwd,
                                db_conn = db_conn,
                                role_id = user.roleId,
                                is_sa_use = user.is_sa_use
                            };

                            Session["UserName"] = smodel.user_name;
                            SessionHelper.Save("Auth_Info", smodel);
                            var userInfo = (from userinfo in db.Users
                                            where userinfo.user_id == user.user_id
                                            select new
                                            {
                                                userID = user.user_id,
                                                username = user.user_name
                                            }).FirstOrDefault();
                            var menus = db.Menus
                                      .Where(m => m.parent_menuid == null && m.isactive)
                                                   .OrderBy(m => m.sort_seq)
                                                   .ThenBy(m => m.menu_name).ToList();

                            //menus.AddRange(db.Menus
                            //          .Where(m => m.parent_menuid == null && m.isactive && !m.isprogram)
                            //          .OrderBy(m => m.sort_seq)
                            //          .ThenBy(m => m.menu_name)
                            //          .ToList());
                            //var queries = menus.Select(xx => new MenuViewModel()
                            //{
                            //    MenusID = xx.menuid,
                            //    MenusName = xx.menu_name,
                            //    ParentMenuID = xx.parent_menuid,
                            //    ActionName = xx.action_name,
                            //    ControllerName = xx.controller_name,
                            //    Area = xx.area,
                            //    Active = xx.isactive,
                            //    SortSeq = xx.sort_seq,
                            //    isProgram = xx.isprogram,
                            //    is_SA_Use = xx.is_sa_use,
                            //    menu_icon = xx.menu_icon,
                            //    ChildMenus = UtilityManager.GetChildMenusByRoleID(xx.Menus1, true)
                            //}).ToList();
                            //SessionHelper.Save("MenuMaster", queries);


                            var permissions = db.RolePermissions.Where(w => w.roleId == smodel.role_id).ToList();

                            var queries = menus.Select(xx => new MenuViewModel()
                                {
                                    MenusID = xx.menuid,
                                    MenusName = xx.menu_name,
                                    ParentMenuID = xx.parent_menuid,
                                    ActionName = xx.action_name,
                                    ControllerName = xx.controller_name,
                                    Area = xx.area,
                                    Active = xx.isactive,
                                    SortSeq = xx.sort_seq,
                                    isProgram = xx.isprogram,
                                    is_SA_Use = xx.is_sa_use,
                                    menu_icon = xx.menu_icon,
                                    RolePermissions = xx.RolePermissions.ToList(),
                                    ChildMenus = GetChildMenusByRoleID(xx.Menus1, permissions, smodel.is_sa_use)
                                }).ToList();

                            var mList = queries.Where(x => (x.isProgram && (smodel.is_sa_use || x.RolePermissions.Contains(permissions
                                                                    .Where(p => p.menuID == x.MenusID).FirstOrDefault()))) || x.ChildMenus.Count > 0).ToList();

                            SessionHelper.Save("MenuMaster", mList);

                            message.message = "Login Successfully!";
                        }
                        else
                        {
                            message.message = "Password is wrong!";
                            message.errorcode = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message.message = ex.Message;
                message.errorcode = -2;
            }
            return Json(message);
        }

        public static List<MenuViewModel> GetChildMenusByRoleID(ICollection<Menu> childlist, List<RolePermission> permissions, bool isSystemUser)
        {
            if (childlist != null)
            {
                var queries = childlist
                        .Where(m => (m.isactive) && (isSystemUser ||
                                m.RolePermissions.Contains(permissions
                                                        .Where(p => p.menuID == m.menuid).FirstOrDefault())))
                        .OrderBy(m => m.sort_seq)
                        .ThenBy(m => m.menu_name)
                        .Select(xx => new MenuViewModel()
                        {
                            MenusID = xx.menuid,
                            MenusName = xx.menu_name,
                            ParentMenuID = xx.parent_menuid,
                            ActionName = xx.action_name,
                            ControllerName = xx.controller_name,
                            Area = xx.area,
                            Active = xx.isactive,
                            SortSeq = xx.sort_seq,
                            isProgram = xx.isprogram,
                            is_SA_Use = xx.is_sa_use,
                            menu_icon = xx.menu_icon,
                            ChildMenus = GetChildMenusByRoleID(xx.Menus1, permissions, isSystemUser)
                        }).ToList();
                //return queries.Where(x => x.isProgram || (!x.isProgram && x.ChildMenus.Count > 0)).ToList();
                return queries;
            }
            return null;
        }

        public bool Check_Company(string company_name, out string db_conn, out int company_id)
        {
            bool flag = false;
            company_id = 0;
            db_conn = string.Empty;
            string connstr = DB_Utility.DecryptStringAES(ConfigurationManager.AppSettings["ConnStr"]);
            string query = "SELECT l.company_id, company_name, company_fullname, db_conn, expired_date " +
                "FROM tbl_license l INNER JOIN tbl_company c ON l.company_id = c.company_id " +
                "WHERE company_name = @cname ";

            SqlConnection conn = new SqlConnection(connstr);
            SqlCommand comm = new SqlCommand(query, conn);
            comm.Parameters.AddWithValue("cname", company_name);

            SqlDataAdapter adapter = new SqlDataAdapter(comm);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            adapter.Dispose();
            if (dt.Rows.Count > 0)
            {
                company_id = Convert.ToInt32(dt.Rows[0]["company_id"]);
                db_conn = dt.Rows[0]["db_conn"].ToString();
                flag = true;
            }
            return flag;
        }

        public ActionResult Logout()
        {
            SessionHelper.Remove("Auth_Info");

            return RedirectToAction("Index", "Login");
        }
    }
}