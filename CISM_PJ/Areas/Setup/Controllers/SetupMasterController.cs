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

namespace CISM_PJ.Areas.Setup.Controllers
{
    public class SetupMasterController : BaseController
    {
        private ErrorMessage message = new ErrorMessage();
        private Common_Msg com_msg = new Common_Msg();
        private string msg_ = string.Empty;
        private string err_msg_ = string.Empty;

        #region Grade

        public ActionResult Index()
        {
            Get_Authorization_byMenuID();
            List<GradeViewModel> list = db.Grades.ToList().Select<Grade, GradeViewModel>(x => x).ToList();
            return View(list);
        }

        public ActionResult YearMerge(GradeViewModel model)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    string umcode = db.Grades.Where(x => x.grade_name == model.grade_name).Select(x => x.grade_name).FirstOrDefault();
                    if (!string.IsNullOrEmpty(umcode))
                    {
                        message.message = umcode + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    db.Grades
                    .Add(
                    new Grade
                    {
                        grade_name = model.grade_name,
                        description = model.description,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    });
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    Grade _exit = db.Grades.Where(x => x.grade_id == model.grade_id).Select(x => x).FirstOrDefault();
                    _exit.grade_name = model.grade_name;
                    _exit.description = model.description;
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

        public ActionResult GetDataById(int year_id)
        {
            var data = db.Grades.Where(x => x.grade_id == year_id).ToList().Select<Grade, GradeViewModel>(x => x).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteData(int year_id)
        {
            try
            {
                Grade _data = db.Grades.Where(x => x.grade_id == year_id).Select(x => x).FirstOrDefault();
                db.Grades.Remove(_data);
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

        #endregion

        #region Subject

        public ActionResult Subject()
        {
            Get_Authorization_byMenuID();
            List<SubjectViewModel> list = db.Subjects.ToList().Select<Subject, SubjectViewModel>(x => x).ToList();
            return View(list);
        }

        public ActionResult SubjectMerge(SubjectViewModel model)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    string subj_name = db.Subjects.Where(x => x.subj_name == model.subj_name).Select(x => x.subj_name).FirstOrDefault();
                    if (!string.IsNullOrEmpty(subj_name))
                    {
                        message.message = subj_name + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    db.Subjects
                    .Add(
                    new Subject
                    {
                        subj_name = model.subj_name,
                        description = model.description,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    });
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    Subject _exit = db.Subjects.Where(x => x.subject_id == model.subject_id).Select(x => x).FirstOrDefault();
                    _exit.subj_name = model.subj_name;
                    _exit.description = model.description;
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

        public ActionResult GetDataByIdSubject(int subject_id)
        {
            var data = db.Subjects.Where(x => x.subject_id == subject_id).ToList().Select<Subject, SubjectViewModel>(x => x).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDataSubject(int subject_id)
        {
            try
            {
                Subject _data = db.Subjects.Where(x => x.subject_id == subject_id).Select(x => x).FirstOrDefault();
                db.Subjects.Remove(_data);
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

        #endregion unitmeasure

        #region OLD
        #region Subject Of Academic
        //public ActionResult SubjectByAcademic()
        //{
        //    Get_Authorization_byMenuID();
        //    return View();
        //}
        //public ActionResult SubjectByAcademicMerge(SubjectOfAcademciViewModel model, List<SubjectOfAcademciViewModel> subjectList)
        //{
        //    bool isChange = false;
        //    switch (model.state)
        //    {
        //        case CISM_PJ.Models.ModelState.Added:

        //            #region Check duplicate_code

        //            string yearCode = db.SubjectByAcademics.Where(x => x.grade_id == model.grade_id).Select(x => x.Grade.grade_name).FirstOrDefault();
        //            if (!string.IsNullOrEmpty(yearCode))
        //            {
        //                message.message = yearCode + com_msg.Duplicated_msg;
        //                message.errorcode = -1;
        //                return Json(message);
        //            }

        //            #endregion Check duplicate_code

        //            msg_ = com_msg.Save_msg;
        //            err_msg_ = com_msg.Save_Err_msg;
        //            foreach (SubjectOfAcademciViewModel _item in subjectList)
        //            {
        //                db.SubjectByAcademics
        //                .Add(
        //                new SubjectByAcademic
        //                {
        //                    grade_id = Convert.ToInt32(model.grade_id),
        //                    year = Convert.ToInt32(model.year),
        //                    subject_id = _item.subject_id,
        //                    createddate = DateTime.Now,
        //                    createduser = GetUserID(),
        //                    modifieddate = DateTime.Now,
        //                    modifieduser = GetUserID()
        //                });
        //            }
        //            break;

        //        case CISM_PJ.Models.ModelState.Modified:
        //            msg_ = com_msg.Updated_msg;
        //            err_msg_ = com_msg.Updated_Err_msg;
        //            var list = db.SubjectByAcademics.Where(x => x.grade_id == model.grade_id &&
        //            x.year == model.year).Select(x => x).ToList();
        //            db.SubjectByAcademics.RemoveRange(list);
        //            foreach (SubjectOfAcademciViewModel _item in subjectList)
        //            {
        //                db.SubjectByAcademics
        //                .Add(
        //                new SubjectByAcademic
        //                {
        //                    grade_id = Convert.ToInt32(model.grade_id),
        //                    year = Convert.ToInt32(model.year),
        //                    subject_id = _item.subject_id,
        //                    createddate = DateTime.Now,
        //                    createduser = GetUserID(),
        //                    modifieddate = DateTime.Now,
        //                    modifieduser = GetUserID()
        //                });
        //            }
        //            //#region OLD
        //            //subjectList = subjectList.Where(x => x.state_status != "" && x.state_status != null).ToList();
        //            //foreach (SubjectOfAcademciViewModel _item in subjectList)
        //            //{
        //            //    SubjectByAcademic data;
        //            //    if (_item.yearsubj_id == null && _item.yearsubj_id == 0)
        //            //    {
        //            //        data = db.SubjectByAcademics.Create();
        //            //        data.createddate = DateTime.Now;
        //            //        data.createduser = GetUserID();
        //            //        data.modifieddate = DateTime.Now;
        //            //        data.modifieduser = GetUserID();
        //            //    }
        //            //    else
        //            //    {
        //            //        data = db.SubjectByAcademics.Where(x => x.yearsubj_id == _item.yearsubj_id).Select(x => x).FirstOrDefault();
        //            //        if (_item.Sstate == CISM_PJ.Models.ModelState.Deleted.ToString())
        //            //        {
        //            //            db.SubjectByAcademics.Remove(data);
        //            //        }
        //            //        if (_item.Sstate != CISM_PJ.Models.ModelState.Deleted.ToString())
        //            //        {
        //            //            data.grade_id = Convert.ToInt32(model.grade_id);
        //            //            data.year = Convert.ToInt32(model.year);
        //            //            data.subject_id = _item.subject_id;
        //            //            isChange = db.ChangeTracker.HasChanges();
        //            //            if (isChange)
        //            //            {
        //            //                data.modifieddate = DateTime.Now;
        //            //                data.modifieduser = GetUserID();
        //            //            }
        //            //            if (_item.yearsubj_id == null)
        //            //            {
        //            //                db.SubjectByAcademics.Add(data);
        //            //            }
        //            //        }
        //            //    }
        //            //}
        //            //#endregion
        //            break;
        //    }

        //    try
        //    {
        //        int success = db.SaveChanges();

        //        if (success > 0)
        //        {
        //            message.message = msg_;
        //            message.errorcode = com_msg.successcode;
        //        }
        //        else
        //        {
        //            message.message = isChange ? err_msg_ : msg_;
        //            message.errorcode = isChange ? com_msg.errorcode : com_msg.successcode;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message.message = ex.Message;
        //        message.errorcode = com_msg.errorcode;
        //    }

        //    return Json(message);
        //}
        ////public ActionResult GetSubjecListByAcademic(int year_id)
        ////{
        ////    var data = (from sa in db.SubjectByAcademics
        ////               where sa.year_id == year_id
        ////               select new SubjectOfAcademciViewModel()
        ////               {
        ////                   yearsubj_id = sa.yearsubj_id,
        ////                   year_id = year_id,
        ////                   year_name = sa.Grade.year_name,
        ////                   subject_id = sa.subject_id,
        ////                   subject_name = sa.Subject.subj_name
        ////               }).ToList();

        ////    return Json(data, JsonRequestBehavior.AllowGet);
        ////}
        
        //public async Task<JsonResult> GetSubjectofAcademicList(DataTableAjaxPostModel model)
        //{
        //    try
        //    {
        //        int total = 0;
        //        int pageSize = model.length;
        //        int skip = model.start;
        //        int size = model.length;

        //        // sort Column Name + Sort Column Direction (asc, desc)
        //        var sortExpression = model.columns[model.order[0].column].name + " " + model.order[0].dir;

        //        // Search Value from (Search box)
        //        var searchValue = model.search.value;

        //        var colSearch = string.Empty;
        //        if (!string.IsNullOrEmpty(searchValue))
        //        {
        //            foreach (Column col in model.columns)
        //            {
        //                if (col.searchable)
        //                {
        //                    if (string.IsNullOrEmpty(colSearch))
        //                    {
        //                        colSearch += col.name + ".ToString().Contains(@0)";
        //                    }
        //                    else
        //                    {
        //                        colSearch += " OR " + col.name + ".ToString().Contains(@0)";
        //                    }
        //                }
        //            }
        //        }

        //        var searchExpression = colSearch;

        //        if (pageSize < 0) pageSize = total;

        //        var customer = string.IsNullOrEmpty(searchExpression) ?
        //            (from sa in db.SubjectByAcademics
        //             select new SubjectOfAcademciViewModel
        //             {
        //                 grade_id = sa.grade_id,
        //                 grade_name = sa.Grade.grade_name,
        //                 year = sa.year,
        //                 syear = sa.year + ""
        //             }).Distinct().OrderBy(sortExpression) :
        //            (from sa in db.SubjectByAcademics
        //             select new SubjectOfAcademciViewModel
        //             {
        //                 grade_id = sa.grade_id,
        //                 grade_name = sa.Grade.grade_name,
        //                 year = sa.year,
        //                 syear = sa.year + ""
        //             }).Distinct().Where(searchExpression, searchValue)
        //             .OrderBy(sortExpression);
        //        total = await customer.CountAsync();

        //        return Json(new { model.draw, recordsFiltered = total, recordsTotal = total, data = await customer.Skip(skip).Take(pageSize).ToArrayAsync() }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        var ar = ex.Message;
        //        throw;
        //    }
        //}
        //public ActionResult GetYearList()
        //{
        //    List<YearViewModel> list = db.Grades.ToList().Select<Grade, YearViewModel>(x => x).ToList();
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetSubjectList()
        //{
        //    List<SubjectViewModel> list = db.Subjects.ToList().Select<Subject, SubjectViewModel>(x => x).ToList();
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region Class Of Academic
        //public ActionResult ClassByAcademic()
        //{
        //    Get_Authorization_byMenuID();
        //    return View();
        //}
        //public ActionResult ClassByAcademicMerge(ClassofAcademicViewModel model, List<ClassofAcademicViewModel> subjectList)
        //{
        //    bool isChange = false;
        //    switch (model.state)
        //    {
        //        case CISM_PJ.Models.ModelState.Added:

        //            #region Check duplicate_code

        //            string yearCode = db.ClassByAcademics.Where(x => x.grade_id == model.grade_id).Select(x => x.Grade.grade_name).FirstOrDefault();
        //            if (!string.IsNullOrEmpty(yearCode))
        //            {
        //                message.message = yearCode + com_msg.Duplicated_msg;
        //                message.errorcode = -1;
        //                return Json(message);
        //            }

        //            #endregion Check duplicate_code

        //            msg_ = com_msg.Save_msg;
        //            err_msg_ = com_msg.Save_Err_msg;
        //            foreach (ClassofAcademicViewModel _item in subjectList)
        //            {
        //                Class ent = db.Classes.Create();
        //                ent.calss_name = _item.calss_name;
        //                ent.description = _item.description;
        //                ent.createddate = DateTime.Now;
        //                ent.createduser = GetUserID();
        //                ent.modifieddate = DateTime.Now;
        //                ent.modifieduser = GetUserID();

        //                db.Classes
        //                    .Add(ent);

        //                db.ClassByAcademics
        //                .Add(
        //                new ClassByAcademic
        //                {
        //                    grade_id = Convert.ToInt32(model.grade_id),
        //                    class_id = ent.class_id,//_item.class_id,
        //                    createddate = DateTime.Now,
        //                    createduser = GetUserID(),
        //                    modifieddate = DateTime.Now,
        //                    modifieduser = GetUserID()
        //                });
        //            }
        //            break;

        //        case CISM_PJ.Models.ModelState.Modified:
        //            msg_ = com_msg.Updated_msg;
        //            err_msg_ = com_msg.Updated_Err_msg;
        //            var list = db.ClassByAcademics.Where(x => x.grade_id == model.grade_id).Select(x => x).ToList();
        //            db.ClassByAcademics.RemoveRange(list);
        //            foreach (ClassofAcademicViewModel _item in subjectList)
        //            {
        //                db.ClassByAcademics
        //                .Add(
        //                new ClassByAcademic
        //                {
        //                    grade_id = Convert.ToInt32(model.grade_id),
        //                    class_id = _item.class_id,
        //                    createddate = DateTime.Now,
        //                    createduser = GetUserID(),
        //                    modifieddate = DateTime.Now,
        //                    modifieduser = GetUserID()
        //                });
        //            }

        //            break;
        //    }

        //    try
        //    {
        //        int success = db.SaveChanges();

        //        if (success > 0)
        //        {
        //            message.message = msg_;
        //            message.errorcode = com_msg.successcode;
        //        }
        //        else
        //        {
        //            message.message = isChange ? err_msg_ : msg_;
        //            message.errorcode = isChange ? com_msg.errorcode : com_msg.successcode;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message.message = ex.Message;
        //        message.errorcode = com_msg.errorcode;
        //    }

        //    return Json(message);
        //}

        ////public ActionResult ClassByAcademicMerge(ClassofAcademicViewModel model, List<ClassofAcademicViewModel> subjectList)
        ////{
        ////    bool isChange = false;
        ////    switch (model.state)
        ////    {
        ////        case CISM_PJ.Models.ModelState.Added:

        ////            #region Check duplicate_code

        ////            string yearCode = db.ClassByAcademics.Where(x => x.grade_id == model.grade_id).Select(x => x.Grade.grade_name).FirstOrDefault();
        ////            if (!string.IsNullOrEmpty(yearCode))
        ////            {
        ////                message.message = yearCode + com_msg.Duplicated_msg;
        ////                message.errorcode = -1;
        ////                return Json(message);
        ////            }

        ////            #endregion Check duplicate_code

        ////            msg_ = com_msg.Save_msg;
        ////            err_msg_ = com_msg.Save_Err_msg;
        ////            foreach (ClassofAcademicViewModel _item in subjectList)
        ////            {
        ////                db.ClassByAcademics
        ////                .Add(
        ////                new ClassByAcademic
        ////                {
        ////                    grade_id = Convert.ToInt32(model.grade_id),
        ////                    class_id = _item.class_id,
        ////                    createddate = DateTime.Now,
        ////                    createduser = GetUserID(),
        ////                    modifieddate = DateTime.Now,
        ////                    modifieduser = GetUserID()
        ////                });
        ////            }
        ////            break;

        ////        case CISM_PJ.Models.ModelState.Modified:
        ////            msg_ = com_msg.Updated_msg;
        ////            err_msg_ = com_msg.Updated_Err_msg;
        ////            var list = db.ClassByAcademics.Where(x => x.grade_id == model.grade_id).Select(x => x).ToList();
        ////            db.ClassByAcademics.RemoveRange(list);
        ////            foreach (ClassofAcademicViewModel _item in subjectList)
        ////            {
        ////                db.ClassByAcademics
        ////                .Add(
        ////                new ClassByAcademic
        ////                {
        ////                    grade_id = Convert.ToInt32(model.grade_id),
        ////                    class_id = _item.class_id,
        ////                    createddate = DateTime.Now,
        ////                    createduser = GetUserID(),
        ////                    modifieddate = DateTime.Now,
        ////                    modifieduser = GetUserID()
        ////                });
        ////            }

        ////            break;
        ////    }

        ////    try
        ////    {
        ////        int success = db.SaveChanges();

        ////        if (success > 0)
        ////        {
        ////            message.message = msg_;
        ////            message.errorcode = com_msg.successcode;
        ////        }
        ////        else
        ////        {
        ////            message.message = isChange ? err_msg_ : msg_;
        ////            message.errorcode = isChange ? com_msg.errorcode : com_msg.successcode;
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        message.message = ex.Message;
        ////        message.errorcode = com_msg.errorcode;
        ////    }

        ////    return Json(message);
        ////}
        //public ActionResult GetClass()
        //{
        //    List<ClassViewModel> list = db.Classes.ToList().Select<Class, ClassViewModel>(x => x).ToList();
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}        
        //public ActionResult GetClassListByAcademic(int? grade_id)
        //{
        //    var data = (from s in db.Classes
        //                join sa in db.ClassByAcademics on s.class_id equals sa.class_id
        //                where sa.grade_id == grade_id
        //                select new ClassofAcademicViewModel()
        //                {
        //                    grade_class_id = sa.grade_class_id,
        //                    grade_id = grade_id,
        //                    class_id = s.class_id,
        //                    calss_name = s.calss_name,
        //                    description = s.description
        //                }).ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        //public async Task<JsonResult> GetClassofAcademicList(DataTableAjaxPostModel model)
        //{
        //    try
        //    {
        //        int total = 0;
        //        int pageSize = model.length;
        //        int skip = model.start;
        //        int size = model.length;

        //        // sort Column Name + Sort Column Direction (asc, desc)
        //        var sortExpression = model.columns[model.order[0].column].name + " " + model.order[0].dir;

        //        // Search Value from (Search box)
        //        var searchValue = model.search.value;

        //        var colSearch = string.Empty;
        //        if (!string.IsNullOrEmpty(searchValue))
        //        {
        //            foreach (Column col in model.columns)
        //            {
        //                if (col.searchable)
        //                {
        //                    if (string.IsNullOrEmpty(colSearch))
        //                    {
        //                        colSearch += col.name + ".ToString().Contains(@0)";
        //                    }
        //                    else
        //                    {
        //                        colSearch += " OR " + col.name + ".ToString().Contains(@0)";
        //                    }
        //                }
        //            }
        //        }

        //        var searchExpression = colSearch;

        //        if (pageSize < 0) pageSize = total;

        //        var customer = string.IsNullOrEmpty(searchExpression) ?
        //            (from sa in db.ClassByAcademics
        //             select new ClassofAcademicViewModel
        //             {
        //                 grade_id = sa.grade_id,
        //                 grade_name = sa.Grade.grade_name,
        //             }).Distinct().OrderBy(sortExpression) :
        //            (from sa in db.ClassByAcademics
        //             select new ClassofAcademicViewModel
        //             {
        //                 grade_id = sa.grade_id,
        //                 grade_name = sa.Grade.grade_name,
        //             }).Distinct().Where(searchExpression, searchValue)
        //             .OrderBy(sortExpression);
        //        total = await customer.CountAsync();

        //        return Json(new { model.draw, recordsFiltered = total, recordsTotal = total, data = await customer.Skip(skip).Take(pageSize).ToArrayAsync() }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        var ar = ex.Message;
        //        throw;
        //    }
        //}

        #endregion
        #endregion

        #region Subject Of Grade
        public ActionResult SubjectByGrade()
        {
            Get_Authorization_byMenuID();
            return View();
        }
        public ActionResult SubjectByGradeMerge(SubjectOfAcademciViewModel model, List<SubjectOfAcademciViewModel> detailList)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    string yearCode = db.SubjectByGrades.Where(x => x.grade_id == model.grade_id).Select(x => x.Grade.grade_name).FirstOrDefault();
                    if (!string.IsNullOrEmpty(yearCode))
                    {
                        message.message = yearCode + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    foreach (SubjectOfAcademciViewModel _item in detailList)
                    {
                        db.SubjectByGrades
                        .Add(
                        new SubjectByGrade
                        {
                            grade_id = Convert.ToInt32(model.grade_id),
                            year_id = model.year_id,
                            subject_id = Guid.NewGuid(),
                            subject_name = _item.subject_name,
                            description = _item.description,
                            createddate = DateTime.Now,
                            createduser = GetUserID(),
                            modifieddate = DateTime.Now,
                            modifieduser = GetUserID()
                        });
                    }
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;

                    if (detailList != null)
                    {
                        foreach (SubjectOfAcademciViewModel _item in detailList)
                        {
                            SubjectByGrade dtl_entity;
                            if (_item.subject_id == Guid.Empty)
                            {
                                dtl_entity = db.SubjectByGrades.Create();
                                dtl_entity.subject_id = Guid.NewGuid();

                                dtl_entity.createddate = DateTime.Now;
                                dtl_entity.createduser = GetUserID();
                                dtl_entity.modifieddate = DateTime.Now;
                                dtl_entity.modifieduser = GetUserID();
                            }
                            else
                            {
                                dtl_entity = db.SubjectByGrades.Where(x => x.subject_id == _item.subject_id).Select(x => x).FirstOrDefault();

                                if (_item.Sstate == CISM_PJ.Models.ModelState.Deleted.ToString())
                                {
                                    db.SubjectByGrades.Remove(dtl_entity);
                                }
                            }

                            if (_item.Sstate != CISM_PJ.Models.ModelState.Deleted.ToString())
                            {
                                dtl_entity.grade_id = (int)model.grade_id;
                                dtl_entity.subject_name = _item.subject_name;
                                dtl_entity.description = _item.description;
                                dtl_entity.year_id = model.year_id;
                                isChange = db.ChangeTracker.HasChanges();
                                if (isChange)
                                {
                                    dtl_entity.modifieddate = DateTime.Now;
                                    dtl_entity.modifieduser = GetUserID();
                                }
                            }
                            if (_item.subject_id == Guid.Empty && _item.Sstate != CISM_PJ.Models.ModelState.Deleted.ToString())
                            {
                                db.SubjectByGrades.Add(dtl_entity);
                            }
                        }
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
   
        public ActionResult GetSubjecListByGrade(int? grade_id, int? year_id)
        {
            var data = (from s in db.SubjectByGrades
                        where s.year_id == year_id && s.grade_id == grade_id
                        select new SubjectOfAcademciViewModel()
                        {
                            grade_id = grade_id,
                            year_id = (int)year_id,
                            subject_id = (Guid)s.subject_id,
                            subject_name = s.subject_name,
                            description = s.description
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetSubjectByGradeAsPage(DataTableAjaxPostModel model)
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
                                colSearch += col.name + ".ToString().Contains(@0)";
                            }
                            else
                            {
                                colSearch += " OR " + col.name + ".ToString().Contains(@0)";
                            }
                        }
                    }
                }

                var searchExpression = colSearch;

                if (pageSize < 0) pageSize = total;

                var customer = string.IsNullOrEmpty(searchExpression) ?
                    (from sa in db.SubjectByGrades
                     select new SubjectOfAcademciViewModel
                     {
                         grade_id = sa.grade_id,
                         grade_name = sa.Grade.grade_name,
                         year_id = sa.year_id,
                         syear = sa.Year.from_year + "-" + sa.Year.to_year
                     }).Distinct().OrderBy(sortExpression) :
                    (from sa in db.SubjectByGrades
                     select new SubjectOfAcademciViewModel
                     {
                         grade_id = sa.grade_id,
                         grade_name = sa.Grade.grade_name,
                         year_id = sa.year_id,
                         syear = sa.Year.from_year + "-" + sa.Year.to_year
                     }).Distinct().Where(searchExpression, searchValue)
                     .OrderBy(sortExpression);
                total = await customer.CountAsync();

                return Json(new { model.draw, recordsFiltered = total, recordsTotal = total, data = await customer.Skip(skip).Take(pageSize).ToArrayAsync() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var ar = ex.Message;
                throw;
            }
        }
        public ActionResult GetGradeList()
        {
            List<GradeViewModel> list = db.Grades.ToList().Select<Grade, GradeViewModel>(x => x).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubjectList()
        {
            List<SubjectViewModel> list = db.Subjects.ToList().Select<Subject, SubjectViewModel>(x => x).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetYearList()
        {
            List<YearViewModel> list = db.Years.Where(x=>x.isactive == true).ToList().Select<Year, YearViewModel>(x => x).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Class Of Grade
        public ActionResult ClassByGrade()
        {
            Get_Authorization_byMenuID();
            return View();
        }
        public ActionResult ClassByGradeMerge(ClassofAcademicViewModel model, List<ClassofAcademicViewModel> detailList)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    string yearCode = db.ClassByGrades.Where(x => x.grade_id == model.grade_id).Select(x => x.Grade.grade_name).FirstOrDefault();
                    if (!string.IsNullOrEmpty(yearCode))
                    {
                        message.message = yearCode + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    foreach (ClassofAcademicViewModel _item in detailList)
                    {
                        db.ClassByGrades
                        .Add(
                        new ClassByGrade
                        {
                            grade_id = Convert.ToInt32(model.grade_id),
                            class_id = Guid.NewGuid(),
                            class_name = _item.class_name,
                            description = _item.description,
                            year = 0,
                            createddate = DateTime.Now,
                            createduser = GetUserID(),
                            modifieddate = DateTime.Now,
                            modifieduser = GetUserID()
                        });
                    }
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                   
                    if (detailList != null)
                    {
                        foreach (ClassofAcademicViewModel _item in detailList)
                        {
                            ClassByGrade dtl_entity;
                            if (_item.class_id == Guid.Empty)
                            {
                                dtl_entity = db.ClassByGrades.Create();
                                dtl_entity.class_id = Guid.NewGuid();

                                dtl_entity.createddate = DateTime.Now;
                                dtl_entity.createduser = GetUserID();
                                dtl_entity.modifieddate = DateTime.Now;
                                dtl_entity.modifieduser = GetUserID();
                            }
                            else
                            {
                                dtl_entity = db.ClassByGrades.Where(x => x.class_id == _item.class_id).Select(x => x).FirstOrDefault();

                                if (_item.Sstate == CISM_PJ.Models.ModelState.Deleted.ToString())
                                {
                                    db.ClassByGrades.Remove(dtl_entity);
                                }
                            }

                            if (_item.Sstate != CISM_PJ.Models.ModelState.Deleted.ToString())
                            {
                                dtl_entity.grade_id =(int)model.grade_id;
                                dtl_entity.class_name = _item.class_name;
                                dtl_entity.description = _item.description;
                                dtl_entity.year = 0;
                                isChange = db.ChangeTracker.HasChanges();
                                if (isChange)
                                {
                                    dtl_entity.modifieddate = DateTime.Now;
                                    dtl_entity.modifieduser = GetUserID();
                                }
                            }
                            if (_item.class_id == Guid.Empty && _item.Sstate != CISM_PJ.Models.ModelState.Deleted.ToString())
                            {
                                db.ClassByGrades.Add(dtl_entity);
                            }
                        }
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

        public ActionResult GetClass()
        {
            List<ClassViewModel> list = db.Classes.ToList().Select<Class, ClassViewModel>(x => x).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetClassListByGrade(int? grade_id)
        {
            var data = (from s in db.ClassByGrades
                        where s.grade_id == grade_id
                        select new ClassofAcademicViewModel()
                        {
                            year = s.year,
                            grade_id = grade_id,
                            class_id = s.class_id,
                            class_name = s.class_name,
                            description = s.description
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetClassofGradeAsPage(DataTableAjaxPostModel model)
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
                                colSearch += col.name + ".ToString().Contains(@0)";
                            }
                            else
                            {
                                colSearch += " OR " + col.name + ".ToString().Contains(@0)";
                            }
                        }
                    }
                }

                var searchExpression = colSearch;

                if (pageSize < 0) pageSize = total;
                var list = db.ClassByGrades.Select(x => new
                {
                    grade_id = x.grade_id,
                    grade_name = x.Grade.grade_name
                }).Distinct().Select(x => new ClassofAcademicViewModel
                {
                    grade_id = x.grade_id,
                    grade_name = x.grade_name,
                    classList = db.ClassByGrades.Where(xx => xx.grade_id == x.grade_id).ToList()
                }).ToList();

                var classList = (from sa in list
                                 select new ClassofAcademicViewModel
                                 {
                                     grade_id = sa.grade_id,
                                     grade_name = sa.grade_name,
                                     class_list = string.Join(",", sa.classList.Select(x=>x.class_name).ToList())
                                 }).ToList();

                var customer = string.IsNullOrEmpty(searchExpression) ?
                    (from sa in db.ClassByGrades
                     select new ClassofAcademicViewModel
                     {
                         grade_id = sa.grade_id,
                         grade_name = sa.Grade.grade_name,
                         //class_list = sa.class_list
                     }).OrderBy(sortExpression) :
                    (from sa in db.ClassByGrades
                     select new ClassofAcademicViewModel
                     {
                         grade_id = sa.grade_id,
                         grade_name = sa.Grade.grade_name,
                         //class_list = sa.class_list
                     }).Where(searchExpression, searchValue)
                     .OrderBy(sortExpression);
                total = await customer.CountAsync();

                return Json(new { model.draw, recordsFiltered = total, recordsTotal = total, data = await customer.Skip(skip).Take(pageSize).ToArrayAsync() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var ar = ex.Message;
                throw;
            }
        }

        #endregion

        #region Class

        public ActionResult Class()
        {
            Get_Authorization_byMenuID();
            List<ClassViewModel> list = db.Classes.ToList().Select<Class, ClassViewModel>(x => x).ToList();
            return View(list);
        }

        public ActionResult ClassMerge(ClassViewModel model)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    string calss_name = db.Classes.Where(x => x.calss_name == model.calss_name).Select(x => x.calss_name).FirstOrDefault();
                    if (!string.IsNullOrEmpty(calss_name))
                    {
                        message.message = calss_name + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    db.Classes
                    .Add(
                    new Class
                    {
                        calss_name = model.calss_name,
                        description = model.description,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    });
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    Class _exit = db.Classes.Where(x => x.class_id == model.class_id).Select(x => x).FirstOrDefault();
                    _exit.calss_name = model.calss_name;
                    _exit.description = model.description;
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

        public ActionResult GetDataByIdClass(int class_id)
        {
            var data = db.Classes.Where(x => x.class_id == class_id).ToList().Select<Class, ClassViewModel>(x => x).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDataClass(int class_id)
        {
            try
            {
                Class _data = db.Classes.Where(x => x.class_id == class_id).Select(x => x).FirstOrDefault();
                db.Classes.Remove(_data);
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

        #endregion

        #region Num Master
        public ActionResult NumMaster()
        {
            Get_Authorization_byMenuID();
            List<RegNoFormatSettingViewModel> list = db.RegNoSetups.ToList().Select<RegNoSetup, RegNoFormatSettingViewModel>(x => x).ToList();
            return View(list);
        }
        public ActionResult NumMasterMerge(RegNoFormatSettingViewModel model)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    db.RegNoSetups
                    .Add(
                    new RegNoSetup
                    {
                        num_id = model.num_id,
                        format_no = model.format_no,
                        doc_control = model.doc_control,
                        doc_prefix = model.doc_prefix,
                        doc_format = model.doc_format,
                        next_no = model.next_no,
                        end_no = model.end_no,
                        restart_no = model.restart_no,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    });
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    RegNoSetup _exit = db.RegNoSetups.Where(x => x.num_id == model.num_id).Select(x => x).FirstOrDefault();
                    _exit.num_id = model.num_id;
                    _exit.format_no = model.format_no;
                    _exit.doc_control = model.doc_control;
                    _exit.doc_prefix = model.doc_prefix;
                    _exit.doc_format = model.doc_format;
                    _exit.next_no = model.next_no;
                    _exit.end_no = model.end_no;
                    _exit.restart_no = model.restart_no;
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
        public ActionResult GetNumMasterByID(int? num_id)
        {
            var data = db.RegNoSetups.Where(x => x.num_id == num_id).ToList().Select<RegNoSetup, RegNoFormatSettingViewModel>(x => x).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetNumMasterAsPage(DataTableAjaxPostModel model)
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

                var subj = string.IsNullOrEmpty(searchExpression) ?
                    (from s in db.RegNoSetups
                     select new RegNoFormatSettingViewModel
                     {
                         num_id = s.num_id,
                         format_no = s.format_no,
                         doc_control = s.doc_control,
                         doc_prefix = s.doc_prefix,
                         doc_format = s.doc_format,
                         next_no = s.next_no,
                         end_no = s.end_no,
                         restart_no = s.restart_no,
                     }).OrderBy(sortExpression) :
                    (from s in db.RegNoSetups
                     select new RegNoFormatSettingViewModel
                     {
                         num_id = s.num_id,
                         format_no = s.format_no,
                         doc_control = s.doc_control,
                         doc_prefix = s.doc_prefix,
                         doc_format = s.doc_format,
                         next_no = s.next_no,
                         end_no = s.end_no,
                         restart_no = s.restart_no,
                     }).Where(searchExpression, searchValue).
                     OrderBy(sortExpression);

                total = await subj.CountAsync();

                return Json(new { model.draw, recordsFiltered = total, recordsTotal = total, data = await subj.Skip(skip).Take(pageSize).ToArrayAsync() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var ar = ex.Message;
                throw;
            }
        }
        public ActionResult DeleteNumMaster(int? num_id)
        {
            try
            {
                RegNoSetup um = db.RegNoSetups.Where(x => x.num_id == num_id).Select(x => x).FirstOrDefault();
                db.RegNoSetups.Remove(um);
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
        [HttpPost]
        public async Task<JsonResult> CheckCode_NumMaster(string checkData, string checkDataID = "")
        {
            if (string.IsNullOrEmpty(checkData))
            {
                throw new ArgumentException("message", nameof(checkData));
            }
            bool isDataExists = false;
            checkData = checkData.Trim().ToUpper();
            //short.TryParse(checkDataID, out short id);
            //byte active = (byte)SetupStatus.Active;
            if (string.IsNullOrEmpty(checkDataID))
            {
                isDataExists = await db.RegNoSetups.AnyAsync(x => x.doc_prefix.Trim().Equals(checkData, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                isDataExists = await db.RegNoSetups.AnyAsync(x => x.doc_prefix.Trim().Equals(checkData, StringComparison.CurrentCultureIgnoreCase) && x.doc_prefix != checkDataID);
            }
            if (isDataExists == true)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PaymentMode

        public ActionResult PaymentMode()
        {
            Get_Authorization_byMenuID();
            List<PaymentModeModel> list = db.PaymentModes.ToList().Select<PaymentMode, PaymentModeModel>(x => x).ToList();
            return View(list);
        }

        public ActionResult PaymentModeMerge(PaymentModeModel model)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    string calss_name = db.PaymentModes.Where(x => x.pm_name == model.pm_name).Select(x => x.pm_name).FirstOrDefault();
                    if (!string.IsNullOrEmpty(calss_name))
                    {
                        message.message = calss_name + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    db.PaymentModes
                    .Add(
                    new PaymentMode
                    {
                        pm_name = model.pm_name,
                        description = model.description,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    });
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    PaymentMode _exit = db.PaymentModes.Where(x => x.paymentmode_id == model.paymentmode_id).Select(x => x).FirstOrDefault();
                    _exit.pm_name = model.pm_name;
                    _exit.description = model.description;
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

        public ActionResult GetDataByIdPM(int paymentmode_id)
        {
            var data = db.PaymentModes.Where(x => x.paymentmode_id == paymentmode_id).ToList().Select<PaymentMode, PaymentModeModel>(x => x).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDataPM(int paymentmode_id)
        {
            try
            {
                PaymentMode _data = db.PaymentModes.Where(x => x.paymentmode_id == paymentmode_id).Select(x => x).FirstOrDefault();
                db.PaymentModes.Remove(_data);
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

        #endregion

        #region StaffType
        public ActionResult StaffType()
        {
            Get_Authorization_byMenuID();
            List<EmployeeTypeModel> list = db.EmployeeTypes.ToList().Select<EmployeeType, EmployeeTypeModel>(x => x).ToList();
            return View(list);
        }

        public ActionResult StaffTypeMerge(EmployeeTypeModel model)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    string _name = db.EmployeeTypes.Where(x => x.emp_type == model.emp_type).Select(x => x.emp_type).FirstOrDefault();
                    if (!string.IsNullOrEmpty(_name))
                    {
                        message.message = _name + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    db.EmployeeTypes
                    .Add(
                    new EmployeeType
                    {
                        emp_type = model.emp_type,
                        description = model.description,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    });
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    EmployeeType _exit = db.EmployeeTypes.Where(x => x.emp_type_Id == model.emp_type_Id).Select(x => x).FirstOrDefault();
                    _exit.emp_type = model.emp_type;
                    _exit.description = model.description;
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

        public ActionResult GetDataByEmpType(int emp_type_Id)
        {
            var data = db.EmployeeTypes.Where(x => x.emp_type_Id == emp_type_Id).ToList().Select<EmployeeType, EmployeeTypeModel>(x => x).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteEmpType(int emp_type_Id)
        {
            try
            {
                EmployeeType _data = db.EmployeeTypes.Where(x => x.emp_type_Id == emp_type_Id).Select(x => x).FirstOrDefault();
                db.EmployeeTypes.Remove(_data);
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

        #endregion

        #region Grades List by Emp Type
        public ActionResult GradeListByEmployeeType()
        {
            Get_Authorization_byMenuID();
            return View();
        }
        public ActionResult GradeListByEmployeeTypeMerge(GradeListbyEmployeeTypeViewModel model, List<GradeListbyEmployeeTypeViewModel> detailList)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    string code = db.EmployeeTypeGrades.Where(x => x.emp_type_Id == model.emp_type_Id).Select(x => x.EmployeeType.emp_type).FirstOrDefault();
                    if (!string.IsNullOrEmpty(code))
                    {
                        message.message = code + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    foreach (GradeListbyEmployeeTypeViewModel _item in detailList)
                    {
                        db.EmployeeTypeGrades
                        .Add(
                        new EmployeeTypeGrade
                        {
                            emp_grade_id = _item.emp_grade_id,
                            emp_type_Id = _item.emp_type_Id,
                            createddate = DateTime.Now,
                            createduser = GetUserID(),
                            modifieddate = DateTime.Now,
                            modifieduser = GetUserID()
                        });
                    }
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    var list = db.EmployeeTypeGrades.Where(x => x.emp_type_Id == model.emp_type_Id).Select(x => x).ToList();
                    db.EmployeeTypeGrades.RemoveRange(list);
                    foreach (GradeListbyEmployeeTypeViewModel _item in detailList)
                    {
                        db.EmployeeTypeGrades
                        .Add(
                        new EmployeeTypeGrade
                        {
                            emp_grade_id = _item.emp_grade_id,
                            emp_type_Id = _item.emp_type_Id,
                            createddate = DateTime.Now,
                            createduser = GetUserID(),
                            modifieddate = DateTime.Now,
                            modifieduser = GetUserID()
                        });
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
        public ActionResult GetEmployeeType()
        {
            List<EmployeeTypeModel> list = db.EmployeeTypes.ToList().Select<EmployeeType, EmployeeTypeModel>(x => x).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmployeeGrade()
        {
            List<EmployeeGradeModel> list = db.EmployeeGrades.ToList().Select<EmployeeGrade, EmployeeGradeModel>(x => x).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCurrency()
        {
            var list = db.Currencies.Select(x => new
            {
                currency_id = x.currency_id,
                currency_code = x.currency_code
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetGradeListByEmpType(int? emp_type_Id)
        {
            var data = (from s in db.EmployeeGrades
                        join sa in db.EmployeeTypeGrades on s.emp_grade_id equals sa.emp_grade_id
                        where sa.emp_type_Id == emp_type_Id
                        select new EmpGradeListbyEmployeeTypeViewModel()
                        {
                            gradeType = sa.gradeType,
                            emp_grade_id = s.emp_grade_id,
                            emp_grade_name = s.emp_grade_name,
                            emp_type = sa.EmployeeType.emp_type,
                            emp_type_Id = sa.emp_type_Id,
                            description = s.description
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetGradeListByEmpTypeAsPage(DataTableAjaxPostModel model)
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
                                colSearch += col.name + ".ToString().Contains(@0)";
                            }
                            else
                            {
                                colSearch += " OR " + col.name + ".ToString().Contains(@0)";
                            }
                        }
                    }
                }

                var searchExpression = colSearch;

                if (pageSize < 0) pageSize = total;

                var customer = string.IsNullOrEmpty(searchExpression) ?
                    (from sa in db.EmployeeTypeGrades
                     select new EmpGradeListbyEmployeeTypeViewModel
                     {
                         emp_type_Id = sa.emp_type_Id,
                         emp_type = sa.EmployeeType.emp_type,
                         //emp_grade_name = sa.EmployeeGrade.emp_grade_name,
                     }).Distinct().OrderBy(sortExpression) :
                    (from sa in db.EmployeeTypeGrades
                     select new EmpGradeListbyEmployeeTypeViewModel
                     {
                         emp_type_Id = sa.emp_type_Id,
                         emp_type = sa.EmployeeType.emp_type,
                         //emp_grade_name = sa.EmployeeGrade.emp_grade_name,
                     }).Distinct().Where(searchExpression, searchValue)
                     .OrderBy(sortExpression);
                total = await customer.CountAsync();

                return Json(new { model.draw, recordsFiltered = total, recordsTotal = total, data = await customer.Skip(skip).Take(pageSize).ToArrayAsync() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var ar = ex.Message;
                throw;
            }
        }
        #endregion

        #region PaymentSetting
        public ActionResult EmployeePaymentSetting()
        {
            Get_Authorization_byMenuID();
            List<EmloyeeTypePaymentSettingViewModel> list = db.EmployeePaymentSettings.ToList().Select<EmployeePaymentSetting, EmloyeeTypePaymentSettingViewModel>(x => x).ToList();
            return View(list);
        }
        public ActionResult EmployeePaymentSettingMerge(EmloyeeTypePaymentSettingViewModel model)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    string calss_name = db.EmployeePaymentSettings.Where(x => x.emp_grade_id == model.emp_grade_id && x.emp_type_Id == model.emp_type_Id).Select(x => x.EmployeeType.emp_type + " and " + x.EmployeeGrade.emp_grade_name).FirstOrDefault();
                    if (!string.IsNullOrEmpty(calss_name))
                    {
                        message.message = calss_name + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    db.EmployeePaymentSettings
                    .Add(
                    new EmployeePaymentSetting
                    {
                        emp_grade_id = model.emp_grade_id,
                        emp_type_Id = model.emp_type_Id,
                        basic_pay = model.basic_pay,
                        increment = model.increment,
                        probation = model.probation,
                        currency_id = model.currency_id,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    });
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    EmployeePaymentSetting _exit = db.EmployeePaymentSettings.Where(x => x.emp_payment_setting_id == model.emp_payment_setting_id).Select(x => x).FirstOrDefault();
                    _exit.emp_grade_id = model.emp_grade_id;
                    _exit.emp_type_Id = model.emp_type_Id;
                    _exit.basic_pay = model.basic_pay;
                    _exit.increment = model.increment;
                    _exit.probation = model.probation;
                    _exit.currency_id = model.currency_id;
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
        public ActionResult GetEmployeePaymentSetting(int emp_payment_setting_id)
        {
            var data = db.EmployeePaymentSettings.Where(x => x.emp_payment_setting_id == emp_payment_setting_id).ToList().Select<EmployeePaymentSetting, EmloyeeTypePaymentSettingViewModel>(x => x).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteEmployeePaymentSetting(int emp_payment_setting_id)
        {
            try
            {
                EmployeePaymentSetting _data = db.EmployeePaymentSettings.Where(x => x.emp_payment_setting_id == emp_payment_setting_id).Select(x => x).FirstOrDefault();
                db.EmployeePaymentSettings.Remove(_data);                
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
        #endregion

        #region Bonus Type Setting
        public ActionResult BonusTypeSetting()
        {
            Get_Authorization_byMenuID();
            return View();
        }
        public ActionResult BonusTypeSettingMerge(BounusTypeSettingViewModel model, List<BounusTypeSettingViewModel> detailList)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    var code = db.BonusTypeSettings.Where(x => x.emp_type_Id == model.emp_type_Id && x.emp_grade_id == model.emp_grade_id).Select(x=>x).FirstOrDefault();
                    if (code != null)
                    {
                        message.message = "This data " + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    var entity = new BonusTypeSetting
                    {
                        bonus_setting_id = Guid.NewGuid(),
                        bonus_id = model.bonus_id,
                        emp_grade_id = model.emp_grade_id,
                        emp_type_Id = model.emp_type_Id,
                        description = model.description,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    };
                    
                    foreach (BounusTypeSettingViewModel _item in detailList)
                    {
                        entity.BonusTypeDetailSettings.Add(
                        new BonusTypeDetailSetting
                        {
                            bonus_setting_id = entity.bonus_setting_id,
                            from_amt = _item.from_amt,
                            to_amt = _item.to_amt,
                            from_year = _item.from_year,
                            to_year = _item.to_year,
                            description = _item.description,
                            createddate = DateTime.Now,
                            createduser = GetUserID(),
                            modifieddate = DateTime.Now,
                            modifieduser = GetUserID()
                        });
                    }
                    db.BonusTypeSettings.Add(entity);
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    var list = db.BonusTypeSettings.Where(x => x.bonus_setting_id == model.bonus_setting_id).Select(x => x).ToList();
                    var idList = list.Select(x=>x.bonus_setting_id).ToList();

                    var detail_list = db.BonusTypeDetailSettings.Where(x => idList.Contains(x.bonus_setting_id)).Select(x => x).ToList();
                    db.BonusTypeDetailSettings.RemoveRange(detail_list);
                    
                    foreach (BounusTypeSettingViewModel _item in detailList)
                    {
                        db.BonusTypeDetailSettings
                        .Add(
                        new BonusTypeDetailSetting
                        {
                            bonus_setting_id = model.bonus_setting_id,
                            from_amt = _item.from_amt,
                            to_amt = _item.to_amt,
                            from_year = _item.from_year,
                            to_year = _item.to_year,
                            description = _item.description,
                            createddate = DateTime.Now,
                            createduser = GetUserID(),
                            modifieddate = DateTime.Now,
                            modifieduser = GetUserID()
                        });
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
        public ActionResult BonusTypeSettingByBonusID(Guid? bonus_setting_id)
        {
            var data = (from s in db.BonusTypeDetailSettings
                        join sa in db.BonusTypeSettings on s.bonus_setting_id equals sa.bonus_setting_id
                        where sa.bonus_setting_id == bonus_setting_id
                        select new BounusTypeSettingViewModel()
                        {
                            from_amt = s.from_amt,
                            to_amt = s.to_amt,
                            from_year = s.from_year,
                            to_year = s.to_year,
                            emp_type = sa.EmployeeType.emp_type,
                            emp_type_Id = sa.emp_type_Id,
                            emp_grade_name = sa.EmployeeGrade.emp_grade_name,
                            emp_grade_id = sa.emp_grade_id,
                            description = s.description,
                            bonus_id = sa.bonus_id,
                            bonus_name =sa.BonusSetup.bonus_name
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> BonusTypeSettingAsPage(DataTableAjaxPostModel model)
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
                                colSearch += col.name + ".ToString().Contains(@0)";
                            }
                            else
                            {
                                colSearch += " OR " + col.name + ".ToString().Contains(@0)";
                            }
                        }
                    }
                }

                var searchExpression = colSearch;

                if (pageSize < 0) pageSize = total;

                var customer = string.IsNullOrEmpty(searchExpression) ?
                    (from sa in db.BonusTypeSettings
                     select new BounusTypeSettingViewModel
                     {
                         bonus_name = sa.BonusSetup.bonus_name,
                         bonus_setting_id = sa.bonus_setting_id,
                         emp_type = sa.EmployeeType.emp_type,
                         emp_type_Id = sa.emp_type_Id,
                         emp_grade_name = sa.EmployeeGrade.emp_grade_name,
                         emp_grade_id = sa.emp_grade_id
                     }).Distinct().OrderBy(sortExpression) :
                    (from sa in db.BonusTypeSettings
                     select new BounusTypeSettingViewModel
                     {
                         bonus_name = sa.BonusSetup.bonus_name,
                         bonus_setting_id = sa.bonus_setting_id,
                         emp_type = sa.EmployeeType.emp_type,
                         emp_type_Id = sa.emp_type_Id,
                         emp_grade_name = sa.EmployeeGrade.emp_grade_name,
                         emp_grade_id = sa.emp_grade_id
                     }).Distinct().Where(searchExpression, searchValue)
                     .OrderBy(sortExpression);
                total = await customer.CountAsync();

                return Json(new { model.draw, recordsFiltered = total, recordsTotal = total, data = await customer.Skip(skip).Take(pageSize).ToArrayAsync() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var ar = ex.Message;
                throw;
            }
        }
        public ActionResult GetBonusList()
        {
            var list = db.BonusSetups.Select(x => new
            {
                bonus_id = x.bonus_id,
                bonus_name = x.bonus_name
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Period
        public ActionResult Period()
        {
            Get_Authorization_byMenuID();
            List<PeriodSetupModel> list = db.PeriodSetups.ToList().Select<PeriodSetup, PeriodSetupModel>(x => x).ToList();
            return View(list);
        }

        public ActionResult PeriodMerge(PeriodSetupModel model)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    string calss_name = db.PeriodSetups.Where(x => x.period_name == model.period_name).Select(x => x.period_name).FirstOrDefault();
                    if (!string.IsNullOrEmpty(calss_name))
                    {
                        message.message = calss_name + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    db.PeriodSetups
                    .Add(
                    new PeriodSetup
                    {
                        period_name = model.period_name,
                        description = model.description,
                        from_time = model.from_time,
                        to_time = model.to_time,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    });
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    PeriodSetup _exit = db.PeriodSetups.Where(x => x.period_id == model.period_id).Select(x => x).FirstOrDefault();
                    _exit.period_name = model.period_name;
                    _exit.description = model.description;
                    _exit.from_time = model.from_time;
                    _exit.to_time = model.to_time;
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

        public ActionResult GetDataByIdPeriod(int period_id)
        {
            var data = db.PeriodSetups.Where(x => x.period_id == period_id).ToList().Select<PeriodSetup, PeriodSetupModel>(x => x).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDataPeriod(int period_id)
        {
            try
            {
                PeriodSetup _data = db.PeriodSetups.Where(x => x.period_id == period_id).Select(x => x).FirstOrDefault();
                db.PeriodSetups.Remove(_data);
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

        #endregion

        #region Exchange Rate
        public ActionResult ExchangeRate()
        {
            Get_Authorization_byMenuID();
            List<ExchangeRateModel> list = db.ExchangeRates.ToList().Select<ExchangeRate, ExchangeRateModel>(x => x).ToList();
            return View(list);
        }

        public ActionResult ExchangeRateMerge(ExchangeRateModel model)
        {
            bool isChange = false;
            int? to_currency_id = db.Schools.Select(x => x.currency_id).FirstOrDefault();
            switch (model.state)
            {   
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    //string calss_name = db.ExchangeRates.Where(x => x.period_name == model.period_name).Select(x => x.period_name).FirstOrDefault();
                    //if (!string.IsNullOrEmpty(calss_name))
                    //{
                    //    message.message = calss_name + com_msg.Duplicated_msg;
                    //    message.errorcode = -1;
                    //    return Json(message);
                    //}

                    #endregion Check duplicate_code
                
                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    db.ExchangeRates
                    .Add(
                    new ExchangeRate
                    {
                        exchange_rate_id = Guid.NewGuid(),
                        from_currency_id = model.from_currency_id,
                        to_currency_id = Convert.ToInt32(to_currency_id), //model.to_currency_id,
                        rate = model.rate,
                        date = model.date,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    });
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    ExchangeRate _exit = db.ExchangeRates.Where(x => x.exchange_rate_id == model.exchange_rate_id).Select(x => x).FirstOrDefault();
                    _exit.from_currency_id = model.from_currency_id;
                    _exit.to_currency_id = Convert.ToInt32(to_currency_id);//model.to_currency_id;
                    _exit.rate = model.rate;
                    _exit.date = model.date;
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

        public ActionResult GetDataByIdRate(Guid exchange_rate_id)
        {
            var data = db.ExchangeRates.Where(x => x.exchange_rate_id == exchange_rate_id).ToList().Select<ExchangeRate, ExchangeRateModel>(x => x).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDataRate(Guid exchange_rate_id)
        {
            try
            {
                ExchangeRate _data = db.ExchangeRates.Where(x => x.exchange_rate_id == exchange_rate_id).Select(x => x).FirstOrDefault();
                db.ExchangeRates.Remove(_data);
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
        #endregion

        #region Holiday
        public ActionResult HolidaySetup()
        {
            Get_Authorization_byMenuID();
            List<HolidaySetupModel> list = db.Holidays.ToList().Select<Holiday, HolidaySetupModel>(x => x).ToList();
            return View(list);
        }

        public ActionResult HolidayMerge(HolidaySetupModel model)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    #region Check duplicate_code

                    var exit = db.Holidays.Where(x => x.holiday_date == model.holiday_date).Select(x => x).FirstOrDefault();
                    if (exit != null)
                    {
                        message.message = "This data is " + com_msg.Duplicated_msg;
                        message.errorcode = -1;
                        return Json(message);
                    }

                    #endregion Check duplicate_code

                    msg_ = com_msg.Save_msg;
                    err_msg_ = com_msg.Save_Err_msg;
                    db.Holidays
                    .Add(
                    new Holiday
                    {
                        name = model.name,
                        holiday_date = model.holiday_date,
                        description = model.description,
                        createddate = DateTime.Now,
                        createduser = GetUserID(),
                        modifieddate = DateTime.Now,
                        modifieduser = GetUserID()
                    });
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    err_msg_ = com_msg.Updated_Err_msg;
                    Holiday _exit = db.Holidays.Where(x => x.holiday_id == model.holiday_id).Select(x => x).FirstOrDefault();
                    _exit.name = model.name;
                    _exit.holiday_date = model.holiday_date;
                    _exit.description = model.description;
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

        public ActionResult GetDataByIdHoliday(int holiday_id)
        {
            var data = db.Holidays.Where(x => x.holiday_id == holiday_id).ToList().Select<Holiday, HolidaySetupModel>(x => x).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDataHoliday(int holiday_id)
        {
            try
            {
                Holiday _data = db.Holidays.Where(x => x.holiday_id == holiday_id).Select(x => x).FirstOrDefault();
                db.Holidays.Remove(_data);
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

        #endregion
    }
}
