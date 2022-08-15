using CISM_PJ.Areas.Admin.Models;
using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CISM_PJ.Uitls
{
    public static class UtilityManager
    {
        public static SelectList GetAllControllers()
        {
            //Assembly asm = Assembly.GetExecutingAssembly();

            var controllerAndActions = Assembly.GetExecutingAssembly().GetTypes()
                                   .Where(type => typeof(Controller).IsAssignableFrom(type))
                                   .SelectMany(type => type.GetMethods())
                                   .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute))
                                    && method.ReturnType == typeof(ActionResult) || method.ReturnType == typeof(Task<ActionResult>))
                                   .OrderBy(x => x.Name);

            //GetControllerByArea("WOReports");
            return new SelectList(controllerAndActions.Select
                (x => new
                {
                    //Remove "Controller" Keyword
                    controllerName = x.DeclaringType.Name.Remove(x.DeclaringType.Name.Length - 10)
                })
                .Distinct().OrderBy(x => x.controllerName).ToList(), "controllerName", "controllerName");
        }

        public static List<string> GetControllerByArea(string areaName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            var controllerAndActions = asm.GetTypes()
                                   .Where(type => typeof(Controller).IsAssignableFrom(type))
                                   .SelectMany(type => type.GetMethods())
                                   .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute))
                                    && method.ReturnType == typeof(ActionResult) || method.ReturnType == typeof(Task<ActionResult>))
                                   .OrderBy(x => x.Name);
            areaName = asm.GetName().Name + ".Areas." + areaName + ".Controllers";
            return controllerAndActions.Where(w => w.DeclaringType.Namespace == areaName)
                .Select(x => x.DeclaringType.Name.Remove(x.DeclaringType.Name.Length - 10))
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        public static List<string> GetActionByControllerName(string controllerName)
        {
            //Assembly asm = Assembly.GetExecutingAssembly();

            var controllerAndActions = Assembly.GetExecutingAssembly().GetTypes()
                                   .Where(type => typeof(Controller).IsAssignableFrom(type))
                                   .SelectMany(type => type.GetMethods())
                                   .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute))
                                    && method.ReturnType == typeof(ActionResult) || method.ReturnType == typeof(Task<ActionResult>))
                                   .OrderBy(x => x.Name);

            controllerName = controllerName + "Controller";
            return controllerAndActions.Where(x => x.DeclaringType.Name == controllerName)
                .Select(x => x.Name)
                .Distinct()
                .OrderBy(o => o)
                .ToList();
        }

        public static SelectList GetAreas(string directoryPath)
        {
            //FromAreasParts//Server.MapPath("~/Areas")
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            var folders = di.GetDirectories().AsEnumerable().ToList().Select(d => new { AreasName = d.Name });
            var foldersList = folders.AsEnumerable();

            return new SelectList(foldersList.ToList(), "AreasName", "AreasName");
        }

        public static List<MenuViewModel> GetChildMenusByRoleID(ICollection<Menu> childlist, bool isSystemUser)
        {
            if (childlist != null)
            {
                var queries = childlist
                        .Where(m => (m.isactive && isSystemUser) || (m.isactive && (!m.isprogram)))
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
                            ChildMenus = GetChildMenusByRoleID(xx.Menus1, isSystemUser)
                        })
                        .ToList();
                return queries.Where(x => x.isProgram || (!x.isProgram && x.ChildMenus.Count > 0)).ToList();
            }
            return null;
        }

        public static List<MenuViewModel> GetChildMenus(ICollection<Menu> childlist)
        {
            if (childlist != null)
            {
                return childlist.Where(x => x.isactive && !x.isprogram)
                .Select(x => new MenuViewModel
                {
                    MenusID = x.menuid,
                    MenusName = x.menu_name,
                    ParentMenuID = x.parent_menuid,
                    ActionName = x.action_name,
                    ControllerName = x.controller_name,
                    Area = x.area,
                    Active = x.isactive,
                    SortSeq = x.sort_seq,
                    ChildMenus = GetChildMenus(x.Menus1)
                }).OrderBy(x => x.SortSeq).ThenBy(o => o.MenusName).ToList();
            }
            return null;
        }
        public static async Task<byte[]> GetImgByteAsync(Uri uri, string ftpUsername, string ftpPassword)
        {
            if (uri.Scheme == Uri.UriSchemeFtp)
            {
                using (WebClient ftpClient = new WebClient())
                {
                    ftpClient.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    return await ftpClient.DownloadDataTaskAsync(uri);
                };
            }
            return null;
        }
        public static byte[] GetImgByte(Uri uri, string ftpUsername, string ftpPassword)
        {
            if (uri.Scheme == Uri.UriSchemeFtp)
            {
                using (WebClient ftpClient = new WebClient())
                {
                    ftpClient.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    return ftpClient.DownloadData(uri);
                };
            }
            return null;
        }
    }
}