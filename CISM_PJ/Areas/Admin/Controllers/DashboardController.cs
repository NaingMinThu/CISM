using CISM_PJ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CISM_PJ.Models.MyEnum;

namespace CISM_PJ.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            var menuID = db.Menus.Where(x => x.controller_name.Contains("Dashboard")).Select(x => x.menuid).FirstOrDefault();
            Get_Authorization_byMenuID(menuID.ToString());
            var totalStudents = db.Students.Where(x=>x.isDelete == false && x.status == (byte)StudentStatus.Active).Count();
            ViewBag.TotalStudents = totalStudents;

            var totalTeacher = db.Employees.Where(x => x.status == (byte)StudentStatus.Active).Count();
            ViewBag.TotalTeachers = totalTeacher;
            return View();
        }
    }
}