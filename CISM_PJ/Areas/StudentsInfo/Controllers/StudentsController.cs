using CISM_PJ.Areas.Setup.Models;
using CISM_PJ.Areas.StudentsInfo.Models;
using CISM_PJ.Common;
using CISM_PJ.Helpers;
using CISM_PJ.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static CISM_PJ.Models.MyEnum;

namespace CISM_PJ.Areas.StudentsInfo.Controllers
{
    public class StudentsController : BaseController
    {
        private ErrorMessage message = new ErrorMessage();
        private Common_Msg com_msg = new Common_Msg();
        private string msg_ = string.Empty;
        private string errmsg_ = string.Empty;
        // GET: Student/Students
        #region Student Listing
        // GET: WOs/WorkOrderListing
        public ActionResult StudentList()
        {
            Get_Authorization_byMenuID();
            return View();
        }
        public async Task<JsonResult> GetStudentListAsPage(DataTableAjaxPostModel model)
        {
            try
            {
                int total = 0;
                int pageSize = model.length;
                int skip = model.start;
                int size = model.length;

                // sort Column Name + Sort Column Direction (asc, desc)
                var sortExpression = model.columns[model.order[0].column].name + " " + model.order[0].dir;

                // Search Value from (Search box)
                var searchValue = model.search.value;

                var colSearch = string.Empty;
                if (!string.IsNullOrEmpty(searchValue))
                {
                    foreach (Column col in model.columns)
                    {
                        if (col.searchable)
                        {
                            if (string.IsNullOrEmpty(colSearch))
                            {
                                colSearch += col.name + ".Contains(@0)";
                            }
                            else
                            {
                                colSearch += " OR " + col.name + ".Contains(@0)";
                            }
                        }
                    }
                }

                var searchExpression = colSearch;

                if (pageSize < 0) pageSize = total;

                var studentDataList = db.Students.Select<Student, StudentViewModel>(x => x);

                var recordData = string.IsNullOrEmpty(searchExpression) ?
                                (from stu in db.Students
                                 where stu.isDelete == false
                                 select new StudentViewModel
                                 {
                                     sr = "1",
                                     student_id = stu.student_id,
                                     name = stu.name,
                                     reg_no = stu.reg_no,
                                     address = stu.address,
                                     class_name = stu.ClassByGrade.class_name,
                                     sdob = stu.dob != null ?
                                     DbFunctions.Right("0" + SqlFunctions.DatePart("day", stu.dob).ToString(), 2) + "/" +
                                     DbFunctions.Right("0" + SqlFunctions.DatePart("m", stu.dob).ToString(), 2) + "/" +
                                     SqlFunctions.DatePart("year", stu.dob) : "",
                                     dob = (DateTime)stu.dob,
                                     contract_no = stu.contract_no +
                                     (stu.contract_no != null & stu.contract_no != "" ? ", " : "") + stu.contract_no1
                                 }).OrderBy(sortExpression) :
                               (from stu in db.Students
                                where stu.isDelete == false
                                select new StudentViewModel
                                {
                                    sr = "1",
                                    student_id = stu.student_id,
                                    name = stu.name,
                                    reg_no = stu.reg_no,
                                    address = stu.address,
                                    class_name = stu.ClassByGrade.class_name,
                                    sdob = stu.dob != null ?
                                     DbFunctions.Right("0" + SqlFunctions.DatePart("day", stu.dob).ToString(), 2) + "/" +
                                     DbFunctions.Right("0" + SqlFunctions.DatePart("m", stu.dob).ToString(), 2) + "/" +
                                     SqlFunctions.DatePart("year", stu.dob) : "",
                                    dob = (DateTime)stu.dob,
                                    contract_no = stu.contract_no +
                                     (stu.contract_no != null & stu.contract_no != "" ? ", " : "") + stu.contract_no1
                                }).Where(searchExpression, searchValue)
                                .OrderBy(sortExpression);
                List<StudentViewModel> llist = recordData.ToList();

                total = recordData.Count();

                return Json(new { model.draw, recordsFiltered = total, recordsTotal = total, data = recordData.Skip(skip).Take(pageSize).ToArray() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var ar = ex.Message;
                throw;
            }
        }
        #endregion

        #region Student     
        public ActionResult Student()
        {
            Get_Authorization_byMenuID();
            return View();
        }
        public async Task<ActionResult> StudentMerge(StudentViewModel model, List<PreviousSchoolRecordViewModel> previous_school_details)
        {
            string stating = string.Empty;
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:
                    msg_ = com_msg.Save_msg;
                    errmsg_ = com_msg.Save_Err_msg;
                    int currentNum = 0;
                    string regNO = string.Empty;

                    #region Num Value from num master to get reg no

                    RegNoSetup num = db.RegNoSetups.Where(x => x.doc_control == "STU").Select(x => x).FirstOrDefault();
                    if (num != null)
                    {
                        currentNum = Convert.ToInt32(num.next_no.Trim());
                        regNO = num.doc_prefix.Trim() + DateTime.Now.ToString("yy") + "R" + currentNum.ToString(num.doc_format.Trim());

                        num.next_no = (currentNum + 1).ToString();
                    }

                    #endregion Num Value from num master to get wo_no

                    var entity = db.Students.Create();

                    entity.student_id = Guid.NewGuid();
                    entity.reg_no = regNO;
                    entity.name = model.name;
                    entity.dob = model.dob;
                    entity.grade_id = model.grade_id;
                    entity.class_id = model.class_id;
                    entity.citizen_id = model.citizen_id;
                    entity.paymentmode_id = model.paymentmode_id;
                    entity.father_name = model.father_name;
                    entity.mother_name = model.mother_name;
                    entity.address = model.address;
                    entity.contract_no = model.contract_no;
                    entity.contract_no1 = model.contract_no1;
                    entity.email = model.email;
                    entity.remark = model.remark;
                    entity.year_id = model.year_id;
                    entity.date_of_join = model.date_of_join;
                    entity.date_of_exit = model.date_of_exit;
                    entity.nationality = model.nationality;
                    entity.status = model.date_of_exit == null? (byte)StudentStatus.Active : (byte)StudentStatus.InActive;
                    entity.createddate = DateTime.Now;
                    entity.createduser = GetUserID();
                    entity.modifieddate = DateTime.Now;
                    entity.modifieduser = GetUserID();

                    #region Previous School Record
                    if (previous_school_details != null)
                    {
                        foreach (PreviousSchoolRecordViewModel item in previous_school_details)
                        {
                            PreviosSchoolRecordByStudent _entity_prvSchool = db.PreviosSchoolRecordByStudents.Create();
                            _entity_prvSchool.prv_school_id = Guid.NewGuid();
                            _entity_prvSchool.reg_no = item.reg_no;
                            _entity_prvSchool.student_id = item.student_id;
                            _entity_prvSchool.year_from = item.year_from;
                            _entity_prvSchool.year_to = item.year_to;
                            _entity_prvSchool.passed_grade = item.passed_grade;
                            _entity_prvSchool.attended_school = item.attended_school;
                            _entity_prvSchool.country = item.country;
                            _entity_prvSchool.createddate = DateTime.Now;
                            _entity_prvSchool.createduser = GetUserID();
                            _entity_prvSchool.modifieddate = DateTime.Now;
                            _entity_prvSchool.modifieduser = GetUserID();
                            entity.PreviosSchoolRecordByStudents.Add(_entity_prvSchool);
                        }
                    }
                    #endregion
                    db.Students.Add(entity);
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    errmsg_ = com_msg.Updated_Err_msg;

                    Student _exit = db.Students.Where(x => x.student_id == model.student_id).Select(x => x).FirstOrDefault();

                    _exit.name = model.name;
                    _exit.dob = model.dob;
                    _exit.class_id = model.class_id;
                    _exit.grade_id = model.grade_id;
                    _exit.citizen_id = model.citizen_id;
                    _exit.paymentmode_id = model.paymentmode_id;
                    _exit.father_name = model.father_name;
                    _exit.mother_name = model.mother_name;
                    _exit.address = model.address;   // no need now
                    _exit.contract_no = model.contract_no;     // no need now
                    _exit.contract_no1 = model.contract_no1;
                    _exit.email = model.email;
                    _exit.remark = model.remark;
                    _exit.year_id = model.year_id;
                    _exit.date_of_join = model.date_of_join;
                    _exit.date_of_exit = model.date_of_exit;
                    _exit.nationality = model.nationality;
                    _exit.status = model.date_of_exit == null ? (byte)StudentStatus.Active : (byte)StudentStatus.InActive;

                    #region Previous School Record
                    var exit_prvSchool_list = db.PreviosSchoolRecordByStudents.Where(x => x.student_id == model.student_id).Select(x=>x).ToList();
                    db.PreviosSchoolRecordByStudents.RemoveRange(exit_prvSchool_list);
                    if (previous_school_details != null)
                    {
                        foreach (PreviousSchoolRecordViewModel item in previous_school_details)
                        {
                            PreviosSchoolRecordByStudent _entity_prvSchool = db.PreviosSchoolRecordByStudents.Create();
                            _entity_prvSchool.prv_school_id = Guid.NewGuid();
                            _entity_prvSchool.reg_no = item.reg_no;
                            _entity_prvSchool.student_id = item.student_id;
                            _entity_prvSchool.year_from = item.year_from;
                            _entity_prvSchool.year_to = item.year_to;
                            _entity_prvSchool.passed_grade = item.passed_grade;
                            _entity_prvSchool.attended_school = item.attended_school;
                            _entity_prvSchool.country = item.country;
                            _entity_prvSchool.createddate = DateTime.Now;
                            _entity_prvSchool.createduser = GetUserID();
                            _entity_prvSchool.modifieddate = DateTime.Now;
                            _entity_prvSchool.modifieduser = GetUserID();
                            _exit.PreviosSchoolRecordByStudents.Add(_entity_prvSchool);
                        }
                    }
                    #endregion

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

                if (success > 0 || !isChange)
                {
                    // SaveFileToDB(model.wo_id, false);
                    message.message = msg_;
                    message.errorcode = 0;
                }
                else
                {
                    message.message = isChange ? errmsg_ : msg_;
                    message.errorcode = isChange ? com_msg.errorcode : com_msg.successcode;
                }
            }
            catch (Exception ex)
            {
                message.message = ex.Message;
                message.errorcode = -2;
            }
            return Json(message);
        }
        public ActionResult GetStudentByID(Guid? student_id)
        {
            var _data = db.Students.Where(x => x.student_id == student_id).ToList().Select<Student, StudentViewModel>(x => x).ToList();
            var _dataPreviousSchool = db.PreviosSchoolRecordByStudents.Where(x => x.student_id == student_id).ToList().Select<PreviosSchoolRecordByStudent, PreviousSchoolRecordViewModel>(x => x).ToList();
            var data = new
            {
                STU = _data,
                PreviousSchool = _dataPreviousSchool
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteStudent(Guid? student_id)
        {
            try
            {
                Student _exit = db.Students.Where(x => x.student_id == student_id).Select(x => x).FirstOrDefault();
                _exit.isDelete = true;
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

        #region Helper Method
        public ActionResult GetPaymentMode()
        {
            var _data = db.PaymentModes.ToList().Select<PaymentMode, PaymentModeModel>(x => x).ToList();
            return Json(_data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCitizenShip()
        {
            var _data = db.CitizenShips.ToList().Select<CitizenShip, CitizenShipModel>(x => x).ToList();
            return Json(_data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetClass()
        {
            var _data = db.Classes.ToList().Select<Class, ClassViewModel>(x => x).ToList();
            return Json(_data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
    }
}