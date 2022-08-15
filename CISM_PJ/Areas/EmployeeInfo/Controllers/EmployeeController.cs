using CISM_PJ.Areas.EmployeeInfo.Models;
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

namespace CISM_PJ.Areas.EmployeeInfo.Controllers
{
    public class EmployeeController : BaseController
    {
        private ErrorMessage message = new ErrorMessage();
        private Common_Msg com_msg = new Common_Msg();
        private string msg_ = string.Empty;
        private string errmsg_ = string.Empty;
        #region Employee Listing
        // GET: WOs/WorkOrderListing
        public ActionResult EmployeeList()
        {
            Get_Authorization_byMenuID();
            return View();
        }
        public async Task<JsonResult> GetEmployeeListAsPage(DataTableAjaxPostModel model)
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
                                (from emp in db.Employees
                                 where emp.status == (byte)StudentStatus.Active
                                 select new EmployeeViewModel
                                 {
                                     employee_id = emp.employee_id,                                    
                                     name = emp.name,
                                     staff_no = emp.staff_no,
                                     emp_type = emp.EmployeeType.emp_type,
                                     emp_type_Id = emp.emp_type_Id,
                                     grade_name = emp.Grade.grade_name,
                                     address = emp.address,                                     
                                     sdob = emp.dob != null ?
                                     DbFunctions.Right("0" + SqlFunctions.DatePart("day", emp.dob).ToString(), 2) + "/" +
                                     DbFunctions.Right("0" + SqlFunctions.DatePart("m", emp.dob).ToString(), 2) + "/" +
                                     SqlFunctions.DatePart("year", emp.dob) : "",
                                     dob = (DateTime)emp.dob,
                                     contract_no = emp.contract_no +
                                     (emp.contract_no != null & emp.contract_no != "" ? ", " : "") + emp.contract_no1
                                 }).OrderBy(sortExpression) :
                               (from emp in db.Employees
                                where emp.status == (byte)StudentStatus.Active
                                select new EmployeeViewModel
                                {
                                    employee_id = emp.employee_id,
                                    name = emp.name,
                                    staff_no = emp.staff_no,
                                    emp_type = emp.EmployeeType.emp_type,
                                    emp_type_Id = emp.emp_type_Id,
                                    grade_name = emp.Grade.grade_name,
                                    address = emp.address,
                                    sdob = emp.dob != null ?
                                     DbFunctions.Right("0" + SqlFunctions.DatePart("day", emp.dob).ToString(), 2) + "/" +
                                     DbFunctions.Right("0" + SqlFunctions.DatePart("m", emp.dob).ToString(), 2) + "/" +
                                     SqlFunctions.DatePart("year", emp.dob) : "",
                                    dob = (DateTime)emp.dob,
                                    contract_no = emp.contract_no +
                                     (emp.contract_no != null & emp.contract_no != "" ? ", " : "") + emp.contract_no1
                                }).Where(searchExpression, searchValue)
                                .OrderBy(sortExpression);
                List<EmployeeViewModel> llist = recordData.ToList();

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

        #region Employee     
        public ActionResult EmployeeEntry()
        {
            Get_Authorization_byMenuID();
            return View();
        }
        public async Task<ActionResult> EmployeeMerge(EmployeeViewModel model, List<HobbiesViewModel> hobbies,
            List<EducationalQualificationViewModel> educations, List<EmploymentHistoryViewModel> employmentList,
            List<FamilyDataViewModel> familyList, List<PersonalSkillViewModel> skillList)
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

                    RegNoSetup num = db.RegNoSetups.Where(x => x.doc_control == "EMP").Select(x => x).FirstOrDefault();
                    if (num != null)
                    {
                        currentNum = Convert.ToInt32(num.next_no.Trim());
                        regNO = num.doc_prefix.Trim() + DateTime.Now.ToString("yy") + "R" + currentNum.ToString(num.doc_format.Trim());

                        num.next_no = (currentNum + 1).ToString();
                    }

                    #endregion Num Value from num master to get wo_no

                    var entity = db.Employees.Create();

                    entity.employee_id = Guid.NewGuid();
                    entity.staff_no = regNO;
                    entity.name = model.name;
                    entity.dob = model.dob;
                    entity.emp_type_Id = model.emp_type_Id;
                    entity.grade_id = model.grade_id;
                    entity.nrc_passport = model.nrc_passport;
                    entity.nationality = model.nationality;
                    entity.gender = model.gender;
                    entity.marital_status = model.marital_status;
                    entity.address = model.address;
                    entity.contract_no = model.contract_no;
                    entity.contract_no1 = model.contract_no1;
                    entity.email = model.email;
                    entity.remark = model.remark;                 
                    entity.status =(byte)StudentStatus.Active;
                    entity.createddate = DateTime.Now;
                    entity.createduser = GetUserID();
                    entity.modifieddate = DateTime.Now;
                    entity.modifieduser = GetUserID();

                    #region Hobbies
                    if (hobbies != null)
                    {
                        foreach (HobbiesViewModel item in hobbies)
                        {
                            EmployeeHobby _entity_hobby = db.EmployeeHobbies.Create();
                            _entity_hobby.hobbies_id = Guid.NewGuid();
                            _entity_hobby.employee_id = entity.employee_id;
                            _entity_hobby.name = item.name;
                            entity.EmployeeHobbies.Add(_entity_hobby);
                        }
                    }
                    #endregion

                    #region Education Qualification                   
                    if (educations != null)
                    {
                        foreach (EducationalQualificationViewModel item in educations)
                        {
                            EmployeeEducation _entity_education = db.EmployeeEducations.Create();
                            _entity_education.education_id = Guid.NewGuid();
                            _entity_education.reg_no = item.reg_no;
                            _entity_education.employee_id = entity.employee_id;
                            _entity_education.year_from = item.year_from;
                            _entity_education.year_to = item.year_to;
                            _entity_education.school = item.school;
                            _entity_education.courses = item.courses;
                            _entity_education.degree_obtained = item.degree_obtained;
                            _entity_education.createddate = DateTime.Now;
                            _entity_education.createduser = GetUserID();
                            _entity_education.modifieddate = DateTime.Now;
                            _entity_education.modifieduser = GetUserID();
                            entity.EmployeeEducations.Add(_entity_education);
                        }
                    }
                    #endregion

                    #region Employemnet Hisotry    
                    if (employmentList != null)
                    {
                        foreach (EmploymentHistoryViewModel item in employmentList)
                        {
                            EmploymentHistory _entity_history = db.EmploymentHistories.Create();
                            _entity_history.employment_history_id = Guid.NewGuid();
                            _entity_history.reg_no = item.reg_no;
                            _entity_history.employee_id = entity.employee_id;
                            _entity_history.year_from = item.year_from;
                            _entity_history.year_to = item.year_to;
                            _entity_history.employer = item.employer;
                            _entity_history.salary = item.salary;
                            _entity_history.job_title = item.job_title;
                            _entity_history.createddate = DateTime.Now;
                            _entity_history.createduser = GetUserID();
                            _entity_history.modifieddate = DateTime.Now;
                            _entity_history.modifieduser = GetUserID();
                            entity.EmploymentHistories.Add(_entity_history);
                        }
                    }
                    #endregion

                    #region Family Data
                    if (familyList != null)
                    {
                        foreach (FamilyDataViewModel item in familyList)
                        {
                            EmployeeFamily _entity_family = db.EmployeeFamilies.Create();
                            _entity_family.family_id = Guid.NewGuid();
                            _entity_family.reg_no = item.reg_no;
                            _entity_family.employee_id = entity.employee_id;
                            _entity_family.name = item.name;
                            _entity_family.relationship = item.relationship;
                            _entity_family.age = item.age;
                            _entity_family.occupation = item.occupation;
                            _entity_family.createddate = DateTime.Now;
                            _entity_family.createduser = GetUserID();
                            _entity_family.modifieddate = DateTime.Now;
                            _entity_family.modifieduser = GetUserID();
                            entity.EmployeeFamilies.Add(_entity_family);
                        }
                    }
                    #endregion

                    #region Skills
                    if (skillList != null)
                    {
                        foreach (PersonalSkillViewModel item in skillList)
                        {
                            EmployeePersonalSkill _entity_skill = db.EmployeePersonalSkills.Create();
                            _entity_skill.personal_skill_id = Guid.NewGuid();
                            _entity_skill.skill_id = item.skill_id;
                            _entity_skill.employee_id = entity.employee_id;
                            _entity_skill.language = item.language;
                            _entity_skill.createddate = DateTime.Now;
                            _entity_skill.createduser = GetUserID();
                            _entity_skill.modifieddate = DateTime.Now;
                            _entity_skill.modifieduser = GetUserID();
                            entity.EmployeePersonalSkills.Add(_entity_skill);
                        }
                    }
                    #endregion
                    db.Employees.Add(entity);
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    errmsg_ = com_msg.Updated_Err_msg;

                    Employee _exit = db.Employees.Where(x => x.employee_id == model.employee_id).Select(x => x).FirstOrDefault();                    
                    _exit.name = model.name;
                    _exit.dob = model.dob;
                    _exit.emp_type_Id = model.emp_type_Id;
                    _exit.grade_id = model.grade_id;
                    _exit.nrc_passport = model.nrc_passport;
                    _exit.nationality = model.nationality;
                    _exit.gender = model.gender;
                    _exit.marital_status = model.marital_status;
                    _exit.address = model.address;
                    _exit.contract_no = model.contract_no;
                    _exit.contract_no1 = model.contract_no1;
                    _exit.email = model.email;
                    _exit.remark = model.remark;
                    _exit.status = (byte)StudentStatus.Active;

                    #region Hobbies
                    var exit_hobbies = db.EmployeeHobbies.Where(x => x.employee_id == model.employee_id).Select(x => x).ToList();
                    db.EmployeeHobbies.RemoveRange(exit_hobbies);
                    if (hobbies != null)
                    {
                        foreach (HobbiesViewModel item in hobbies)
                        {
                            EmployeeHobby _entity_hobby = db.EmployeeHobbies.Create();
                            _entity_hobby.hobbies_id = Guid.NewGuid();
                            _entity_hobby.employee_id = _exit.employee_id;
                            _entity_hobby.name = item.name;                          
                            _exit.EmployeeHobbies.Add(_entity_hobby);
                        }
                    }
                    #endregion

                    #region Previous School Record
                    var exit_educations_list = db.EmployeeEducations.Where(x => x.employee_id == model.employee_id).Select(x => x).ToList();
                    db.EmployeeEducations.RemoveRange(exit_educations_list);
                    if (educations != null)
                    {
                        foreach (EducationalQualificationViewModel item in educations)
                        {
                            EmployeeEducation _entity_education = db.EmployeeEducations.Create();
                            _entity_education.education_id = Guid.NewGuid();
                            _entity_education.reg_no = item.reg_no;
                            _entity_education.employee_id = _exit.employee_id;
                            _entity_education.year_from = item.year_from;
                            _entity_education.year_to = item.year_to;
                            _entity_education.school = item.school;
                            _entity_education.courses = item.courses;
                            _entity_education.degree_obtained = item.degree_obtained;
                            _entity_education.createddate = DateTime.Now;
                            _entity_education.createduser = GetUserID();
                            _entity_education.modifieddate = DateTime.Now;
                            _entity_education.modifieduser = GetUserID();
                            _exit.EmployeeEducations.Add(_entity_education);
                        }
                    }
                    #endregion

                    #region Employment Hisotry
                    var exit_histories_list = db.EmploymentHistories.Where(x => x.employee_id == model.employee_id).Select(x => x).ToList();
                    db.EmploymentHistories.RemoveRange(exit_histories_list);
                    if (employmentList != null)
                    {
                        foreach (EmploymentHistoryViewModel item in employmentList)
                        {
                            EmploymentHistory _entity_history = db.EmploymentHistories.Create();
                            _entity_history.employment_history_id = Guid.NewGuid();
                            _entity_history.reg_no = item.reg_no;
                            _entity_history.employee_id = model.employee_id;
                            _entity_history.year_from = item.year_from;
                            _entity_history.year_to = item.year_to;
                            _entity_history.employer = item.employer;
                            _entity_history.salary = item.salary;
                            _entity_history.job_title = item.job_title;
                            _entity_history.createddate = DateTime.Now;
                            _entity_history.createduser = GetUserID();
                            _entity_history.modifieddate = DateTime.Now;
                            _entity_history.modifieduser = GetUserID();
                            _exit.EmploymentHistories.Add(_entity_history);
                        }
                    }
                    #endregion

                    #region Family Data
                    var exit_family_list = db.EmployeeFamilies.Where(x => x.employee_id == model.employee_id).Select(x => x).ToList();
                    db.EmployeeFamilies.RemoveRange(exit_family_list);
                    if (familyList != null)
                    {
                        foreach (FamilyDataViewModel item in familyList)
                        {
                            EmployeeFamily _entity_family = db.EmployeeFamilies.Create();
                            _entity_family.family_id = Guid.NewGuid();
                            _entity_family.reg_no = item.reg_no;
                            _entity_family.employee_id = model.employee_id;
                            _entity_family.name = item.name;
                            _entity_family.relationship = item.relationship;
                            _entity_family.age = item.age;
                            _entity_family.occupation = item.occupation;
                            _entity_family.createddate = DateTime.Now;
                            _entity_family.createduser = GetUserID();
                            _entity_family.modifieddate = DateTime.Now;
                            _entity_family.modifieduser = GetUserID();
                            _exit.EmployeeFamilies.Add(_entity_family);
                        }
                    }
                    #endregion

                    #region Skills
                    var exit_skill_list = db.EmployeePersonalSkills.Where(x => x.employee_id == model.employee_id).Select(x => x).ToList();
                    db.EmployeePersonalSkills.RemoveRange(exit_skill_list);
                    if (skillList != null)
                    {
                        foreach (PersonalSkillViewModel item in skillList)
                        {
                            EmployeePersonalSkill _entity_skill = db.EmployeePersonalSkills.Create();
                            _entity_skill.personal_skill_id = Guid.NewGuid();
                            _entity_skill.skill_id = item.skill_id;
                            _entity_skill.employee_id = model.employee_id;
                            _entity_skill.language = item.language;
                            _entity_skill.createddate = DateTime.Now;
                            _entity_skill.createduser = GetUserID();
                            _entity_skill.modifieddate = DateTime.Now;
                            _entity_skill.modifieduser = GetUserID();
                            _exit.EmployeePersonalSkills.Add(_entity_skill);
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
        public ActionResult GetEmployeeByID(Guid? employee_id)
        {
            var _data = db.Employees.Where(x => x.employee_id == employee_id).ToList().Select<Employee, EmployeeViewModel>(x => x).ToList();
            var _dataHobbies = db.EmployeeHobbies.Where(x => x.employee_id == employee_id).ToList().Select<EmployeeHobby, HobbiesViewModel>(x => x).ToList();
            var _dataQualification = db.EmployeeEducations.Where(x => x.employee_id == employee_id).ToList().Select<EmployeeEducation, EducationalQualificationViewModel>(x => x).ToList();
            var _dataHistories = db.EmploymentHistories.Where(x => x.employee_id == employee_id).ToList().Select<EmploymentHistory, EmploymentHistoryViewModel>(x => x).ToList();
            var _dataFamily = db.EmployeeFamilies.Where(x => x.employee_id == employee_id).ToList().Select<EmployeeFamily, FamilyDataViewModel>(x => x).ToList();
            var _dataSkill = db.EmployeePersonalSkills.Where(x => x.employee_id == employee_id).ToList().Select<EmployeePersonalSkill, PersonalSkillViewModel>(x => x).ToList();
            var data = new
            {
                EMP = _data,
                Hobbies = _dataHobbies,
                Qualification = _dataQualification,
                Hisotries = _dataHistories,
                Family = _dataFamily,
                Skill = _dataSkill
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteEmployee(Guid? employee_id)
        {
            try
            {
                Employee _exit = db.Employees.Where(x => x.employee_id == employee_id).Select(x => x).FirstOrDefault();
                _exit.status = (byte)StudentStatus.InActive;
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
        public ActionResult GetEmployeeType()
        {
            var _data = db.EmployeeTypes.ToList().Select<EmployeeType, EmployeeTypeModel>(x => x).ToList();
            return Json(_data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSkill()
        {
            var _data = db.Skills.ToList().Select<Skill, SkillViewModel>(x => x).ToList();
            return Json(_data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
    }
}