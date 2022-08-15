using CISM_PJ.Areas.AllowanceModule.Models;
using CISM_PJ.Areas.Setup.Models;
using CISM_PJ.Common;
using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace CISM_PJ.Areas.AllowanceModule.Controllers
{
    public class AllowanceInfoController : BaseController
    {
        private ErrorMessage message = new ErrorMessage();
        private Common_Msg com_msg = new Common_Msg();
        private string msg_ = string.Empty;
        private string err_msg_ = string.Empty;

        #region Allowance
        public ActionResult AllowanceSetup()
        {
            Get_Authorization_byMenuID();
            List<AllowanceSetupModel> list = db.Allowances.ToList().Select<Allowance, AllowanceSetupModel>(x => x).ToList();
            return View(list);
        }
        public ActionResult AllowancePeriodMerge(AllowanceSetupModel model)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    var data = db.Allowances.Where(x => x.allowance_type_id == model.allowance_type_id &&
                    x.date == model.date && x.employee_id == model.employee_id).Select(x => x).FirstOrDefault();
                    if (data != null)
                    {
                        message.message = "This data is " + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    db.Allowances
                    .Add(
                    new Allowance
                    {
                        allowance_id = Guid.NewGuid(),
                        employee_id = model.employee_id,
                        allowance_type_id = model.allowance_type_id,
                        date = model.date,
                        amount = model.amount,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    });
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    Allowance _exit = db.Allowances.Where(x => x.allowance_id == model.allowance_id).Select(x => x).FirstOrDefault();
                    _exit.employee_id = model.employee_id;
                    _exit.allowance_type_id = model.allowance_type_id;
                    _exit.date = model.date;
                    _exit.amount = model.amount;
                    isChange = db.ChangeTracker.HasChanges();

                    if (isChange)
                    {
                        _exit.modifieddate = DateTime.Now;
                        _exit.modifieduser = GetUserID();
                    }
                    break;
            }

            try
            {
                int success = db.SaveChanges();

                if (success > 0)
                {
                    message.message = msg_;
                    message.errorcode = com_msg.successcode;
                }
                else
                {
                    message.message = isChange ? err_msg_ : msg_;
                    message.errorcode = isChange ? com_msg.errorcode : com_msg.successcode;
                }
            }
            catch (Exception ex)
            {
                message.message = ex.Message;
                message.errorcode = com_msg.errorcode;
            }

            return Json(message);
        }
        public ActionResult GetDataByIdAllowanceSetup(Guid allowance_id)
        {
            var data = db.Allowances.Where(x => x.allowance_id == allowance_id).ToList().Select<Allowance, AllowanceSetupModel>(x => x).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteDataAllowanceSetup(Guid allowance_id)
        {
            try
            {
                Allowance _data = db.Allowances.Where(x => x.allowance_id == allowance_id).Select(x => x).FirstOrDefault();
                db.Allowances.Remove(_data);
                int success = db.SaveChanges();

                if (success > 0)
                {
                    message.message = com_msg.Deleted_msg;
                    message.errorcode = com_msg.deletedCode;
                }
                else
                {
                    message.message = com_msg.Deleted_Err_msg;
                    message.errorcode = com_msg.errorcode;
                }

                return Json(message);
            }
            catch (Exception ex)
            {
                message.message = ex.InnerException.InnerException.Message.Contains("REFERENCE constraint") ? com_msg.Deleting_Error_msg : ex.Message;
                message.errorcode = ex.InnerException.InnerException.Message.Contains("REFERENCE constraint") ? 1 : -2;
            }
            return Json(message);
        }
        public ActionResult GetAllowanceList()
        {
            var list = db.AllowanceTypes.Select(x => new
            {
                allowance_type_id = x.allowance_type_id,
                name = x.name
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmployee()
        {
            var list = db.Employees.Select(x => new
            {
                employee_id = x.employee_id,
                name = x.name
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}