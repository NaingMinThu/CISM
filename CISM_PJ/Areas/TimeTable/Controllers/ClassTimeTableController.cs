using CISM_PJ.Areas.Setup.Models;
using CISM_PJ.Areas.StudentsInfo.Models;
using CISM_PJ.Areas.TimeTable.Models;
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


namespace CISM_PJ.Areas.TimeTable.Controllers
{
    public class ClassTimeTableController : BaseController
    {
        private ErrorMessage message = new ErrorMessage();
        private Common_Msg com_msg = new Common_Msg();
        private string msg_ = string.Empty;
        private string errmsg_ = string.Empty;
        // GET: TimeTable/ClassTimeTable
        #region Class Timetable
        public ActionResult ClassTimeTable()
        {
            Get_Authorization_byMenuID();
            return View();
        }
        public ActionResult ClassTimeTableEntry()
        {
            Get_Authorization_byMenuID();
            return View();
        }
        public ActionResult ClassTimeTableMerge(ClassTimeTableViewModel model, List<ClassTimeTableDetailModel> detailList)
        {
            bool isChange = false;
            switch (model.state)
            {
                case CISM_PJ.Models.ModelState.Added:

                    msg_ = com_msg.Save_msg;
                    errmsg_ = com_msg.Save_Err_msg;

                    var time_entity = db.Timetables.Create();
                    time_entity.time_table_id = Guid.NewGuid();
                    time_entity.grade_id = Convert.ToInt32(model.grade_id);
                    time_entity.affected_date = model.affected_date;
                    time_entity.class_id = model.class_id;
                    time_entity.createddate = DateTime.Now;
                    time_entity.createduser = GetUserID();
                    time_entity.modifieddate = DateTime.Now;
                    time_entity.modifieduser = GetUserID();

                    foreach (ClassTimeTableDetailModel _item in detailList)
                    {
                        if (_item.monday_List != null)
                        {
                            foreach (SubjectbyGradeViewModel sub in _item.monday_List)
                            {
                                time_entity.ClassTimetables
                                                       .Add(
                                                       new ClassTimetable
                                                       {
                                                           class_time_table_id = Guid.NewGuid(),
                                                           time_table_id = time_entity.time_table_id,
                                                           subject_id = sub.subject_id,
                                                           weekday = (byte)WeeksDay.Monday,
                                                           period_id = _item.period_id,
                                                           createddate = DateTime.Now,
                                                           createduser = GetUserID(),
                                                           modifieddate = DateTime.Now,
                                                           modifieduser = GetUserID()
                                                       });
                            }
                        }

                        if (_item.tue_List != null)
                        {
                            foreach (SubjectbyGradeViewModel sub in _item.tue_List)
                            {
                                time_entity.ClassTimetables
                                                       .Add(
                                                       new ClassTimetable
                                                       {
                                                           class_time_table_id = Guid.NewGuid(),
                                                           time_table_id = time_entity.time_table_id,
                                                           subject_id = sub.subject_id,
                                                           weekday = (byte)WeeksDay.Thusday,
                                                           period_id = _item.period_id,
                                                           createddate = DateTime.Now,
                                                           createduser = GetUserID(),
                                                           modifieddate = DateTime.Now,
                                                           modifieduser = GetUserID()
                                                       });
                            }
                        }

                        if (_item.wed_List != null)
                        {
                            foreach (SubjectbyGradeViewModel sub in _item.wed_List)
                            {
                                time_entity.ClassTimetables
                                                       .Add(
                                                       new ClassTimetable
                                                       {
                                                           class_time_table_id = Guid.NewGuid(),
                                                           time_table_id = time_entity.time_table_id,
                                                           subject_id = sub.subject_id,
                                                           weekday = (byte)WeeksDay.Wednesday,
                                                           period_id = _item.period_id,
                                                           createddate = DateTime.Now,
                                                           createduser = GetUserID(),
                                                           modifieddate = DateTime.Now,
                                                           modifieduser = GetUserID()
                                                       });
                            }
                        }

                        if (_item.thur_List != null)
                        {
                            foreach (SubjectbyGradeViewModel sub in _item.thur_List)
                            {
                                time_entity.ClassTimetables
                                                       .Add(
                                                       new ClassTimetable
                                                       {
                                                           class_time_table_id = Guid.NewGuid(),
                                                           time_table_id = time_entity.time_table_id,
                                                           subject_id = sub.subject_id,
                                                           weekday = (byte)WeeksDay.Tursday,
                                                           period_id = _item.period_id,
                                                           createddate = DateTime.Now,
                                                           createduser = GetUserID(),
                                                           modifieddate = DateTime.Now,
                                                           modifieduser = GetUserID()
                                                       });
                            }
                        }

                        if (_item.fri_List != null)
                        {
                            foreach (SubjectbyGradeViewModel sub in _item.fri_List)
                            {
                                time_entity.ClassTimetables
                                                       .Add(
                                                       new ClassTimetable
                                                       {
                                                           class_time_table_id = Guid.NewGuid(),
                                                           time_table_id = time_entity.time_table_id,
                                                           subject_id = sub.subject_id,
                                                           weekday = (byte)WeeksDay.Friday,
                                                           period_id = _item.period_id,
                                                           createddate = DateTime.Now,
                                                           createduser = GetUserID(),
                                                           modifieddate = DateTime.Now,
                                                           modifieduser = GetUserID()
                                                       });
                            }
                        }


                    }
                    db.Timetables.Add(time_entity);
                    break;

                case CISM_PJ.Models.ModelState.Modified:
                    msg_ = com_msg.Updated_msg;
                    errmsg_ = com_msg.Updated_Err_msg;
                    var list = db.Timetables.Where(x => x.grade_id == model.grade_id &&
                    x.class_id == model.class_id && x.affected_date == model.affected_date).Select(x => x).ToList();

                    var idList = list.Select(x => x.time_table_id).ToList();

                    var detail_list = db.ClassTimetables.Where(x => idList.Contains(x.time_table_id)).Select(x => x).ToList();
                    db.ClassTimetables.RemoveRange(detail_list);

                    foreach (ClassTimeTableDetailModel _item in detailList)
                    {
                        if (_item.monday_List != null)
                        {
                            foreach (SubjectbyGradeViewModel sub in _item.monday_List)
                            {
                                db.ClassTimetables
                                                       .Add(
                                                       new ClassTimetable
                                                       {
                                                           class_time_table_id = Guid.NewGuid(),
                                                           time_table_id = model.time_table_id,
                                                           subject_id = sub.subject_id,
                                                           weekday = (byte)WeeksDay.Monday,
                                                           period_id = _item.period_id,
                                                           createddate = DateTime.Now,
                                                           createduser = GetUserID(),
                                                           modifieddate = DateTime.Now,
                                                           modifieduser = GetUserID()
                                                       });
                            }
                        }

                        if (_item.tue_List != null)
                        {
                            foreach (SubjectbyGradeViewModel sub in _item.tue_List)
                            {
                                db.ClassTimetables
                                                       .Add(
                                                       new ClassTimetable
                                                       {
                                                           class_time_table_id = Guid.NewGuid(),
                                                           time_table_id = model.time_table_id,
                                                           subject_id = sub.subject_id,
                                                           weekday = (byte)WeeksDay.Thusday,
                                                           period_id = _item.period_id,
                                                           createddate = DateTime.Now,
                                                           createduser = GetUserID(),
                                                           modifieddate = DateTime.Now,
                                                           modifieduser = GetUserID()
                                                       });
                            }
                        }

                        if (_item.wed_List != null)
                        {
                            foreach (SubjectbyGradeViewModel sub in _item.wed_List)
                            {
                                db.ClassTimetables
                                                       .Add(
                                                       new ClassTimetable
                                                       {
                                                           class_time_table_id = Guid.NewGuid(),
                                                           time_table_id = model.time_table_id,
                                                           subject_id = sub.subject_id,
                                                           weekday = (byte)WeeksDay.Wednesday,
                                                           period_id = _item.period_id,
                                                           createddate = DateTime.Now,
                                                           createduser = GetUserID(),
                                                           modifieddate = DateTime.Now,
                                                           modifieduser = GetUserID()
                                                       });
                            }
                        }

                        if (_item.thur_List != null)
                        {
                            foreach (SubjectbyGradeViewModel sub in _item.thur_List)
                            {
                                db.ClassTimetables
                                                       .Add(
                                                       new ClassTimetable
                                                       {
                                                           class_time_table_id = Guid.NewGuid(),
                                                           time_table_id = model.time_table_id,
                                                           subject_id = sub.subject_id,
                                                           weekday = (byte)WeeksDay.Tursday,
                                                           period_id = _item.period_id,
                                                           createddate = DateTime.Now,
                                                           createduser = GetUserID(),
                                                           modifieddate = DateTime.Now,
                                                           modifieduser = GetUserID()
                                                       });
                            }
                        }

                        if (_item.fri_List != null)
                        {
                            foreach (SubjectbyGradeViewModel sub in _item.fri_List)
                            {
                                db.ClassTimetables
                                                       .Add(
                                                       new ClassTimetable
                                                       {
                                                           class_time_table_id = Guid.NewGuid(),
                                                           time_table_id = model.time_table_id,
                                                           subject_id = sub.subject_id,
                                                           weekday = (byte)WeeksDay.Friday,
                                                           period_id = _item.period_id,
                                                           createddate = DateTime.Now,
                                                           createduser = GetUserID(),
                                                           modifieddate = DateTime.Now,
                                                           modifieduser = GetUserID()
                                                       });
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
                    message.message = isChange ? errmsg_ : msg_;
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
        public ActionResult GetClassTimeTable(Guid? time_table_id)
        {
            var periodList = db.PeriodSetups.OrderBy(x => x.period_name).ToList();
            List<ClassTimeTableDetailModel> detail_data = new List<ClassTimeTableDetailModel>();
            foreach (var item in periodList)
            {
                var classTimeTableInfo = new ClassTimeTableDetailModel();
                classTimeTableInfo.period_id = item.period_id;
                classTimeTableInfo.period_name = item.period_name;
                classTimeTableInfo.time_period = item.from_time + "-" + item.to_time;
                classTimeTableInfo.monday_List = (from t in db.ClassTimetables
                                                 where t.period_id == item.period_id && t.time_table_id == time_table_id
                                                 && t.weekday == (byte)WeeksDay.Monday
                                                 select new SubjectbyGradeViewModel
                                                 { 
                                                        subject_id = t.subject_id,
                                                        subj_name = t.SubjectByGrade.subject_name
                                                 }).ToList();
                classTimeTableInfo.monday_List_data = String.Join(",", classTimeTableInfo.monday_List.Select(x => x.subj_name).ToList());

                classTimeTableInfo.tue_List = (from t in db.ClassTimetables
                                                  where t.period_id == item.period_id && t.time_table_id == time_table_id
                                                  && t.weekday == (byte)WeeksDay.Thusday
                                               select new SubjectbyGradeViewModel
                                               {
                                                      subject_id = t.subject_id,
                                                      subj_name = t.SubjectByGrade.subject_name
                                               }).ToList();
                classTimeTableInfo.tue_List_data = String.Join(",", classTimeTableInfo.tue_List.Select(x => x.subj_name).ToList());

                classTimeTableInfo.wed_List = (from t in db.ClassTimetables
                                                  where t.period_id == item.period_id && t.time_table_id == time_table_id
                                                  && t.weekday == (byte)WeeksDay.Wednesday
                                                  select new SubjectbyGradeViewModel
                                                  {
                                                      subject_id = t.subject_id,
                                                      subj_name = t.SubjectByGrade.subject_name
                                                  }).ToList();
                classTimeTableInfo.wed_List_data = String.Join(",", classTimeTableInfo.wed_List.Select(x => x.subj_name).ToList());

                classTimeTableInfo.thur_List = (from t in db.ClassTimetables
                                                  where t.period_id == item.period_id && t.time_table_id == time_table_id
                                                  && t.weekday == (byte)WeeksDay.Tursday
                                                select new SubjectbyGradeViewModel
                                                {
                                                      subject_id = t.subject_id,
                                                      subj_name = t.SubjectByGrade.subject_name
                                                }).ToList();
                classTimeTableInfo.thur_List_data = String.Join(",", classTimeTableInfo.thur_List.Select(x => x.subj_name).ToList());

                classTimeTableInfo.fri_List = (from t in db.ClassTimetables
                                                  where t.period_id == item.period_id && t.time_table_id == time_table_id
                                                  && t.weekday == (byte)WeeksDay.Friday
                                                  select new SubjectbyGradeViewModel
                                                  {
                                                      subject_id = t.subject_id,
                                                      subj_name = t.SubjectByGrade.subject_name
                                                  }).ToList();
                classTimeTableInfo.fri_List_data = String.Join(",", classTimeTableInfo.fri_List.Select(x => x.subj_name).ToList());
                detail_data.Add(classTimeTableInfo);
            }

            var data = (from s in db.Timetables
                        join g in db.Grades on s.grade_id equals g.grade_id
                        join c in db.ClassByGrades on s.class_id equals c.class_id
                        where s.time_table_id == time_table_id
                        select new ClassTimeTableViewModel()
                        {
                            time_table_id = s.time_table_id,
                            grade_id = g.grade_id,
                            grade_name = g.grade_name,
                            class_id = c.class_id,
                            calss_name = c.class_name,
                            affected_date = s.affected_date,
                            year = s.affected_date.Year
                        }).ToList();           
            var dataList = new { Master = data, Detail = detail_data };

            return Json(dataList, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetClassTimeTableList(DataTableAjaxPostModel model)
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
                    (from sa in db.Timetables
                     select new ClassTimeTableViewModel
                     {
                         sr = "",
                         grade_id = sa.grade_id,
                         grade_name = sa.Grade.grade_name,
                         saffected_date = sa.affected_date != null ?
                                     (SqlFunctions.DatePart("day", sa.affected_date) > 9 ? SqlFunctions.DatePart("day", sa.affected_date).ToString() :
                                     ("0" + SqlFunctions.DatePart("day", sa.affected_date).ToString())) + "/" +
                                     (SqlFunctions.DatePart("m", sa.affected_date) > 9 ? SqlFunctions.DatePart("m", sa.affected_date).ToString() :
                                     ("0" + SqlFunctions.DatePart("m", sa.affected_date).ToString())) + "/" +
                                     SqlFunctions.DatePart("year", sa.affected_date) : "",
                         calss_name = sa.ClassByGrade.class_name,
                         time_table_id = sa.time_table_id
                     }).Distinct().OrderBy(sortExpression) :
                    (from sa in db.Timetables
                     select new ClassTimeTableViewModel
                     {
                         sr = "",
                         grade_id = sa.grade_id,
                         grade_name = sa.Grade.grade_name,
                         saffected_date = sa.affected_date != null ?
                                     (SqlFunctions.DatePart("day", sa.affected_date) > 9 ? SqlFunctions.DatePart("day", sa.affected_date).ToString() :
                                     ("0" + SqlFunctions.DatePart("day", sa.affected_date).ToString())) + "/" +
                                     (SqlFunctions.DatePart("m", sa.affected_date) > 9 ? SqlFunctions.DatePart("m", sa.affected_date).ToString() :
                                     ("0" + SqlFunctions.DatePart("m", sa.affected_date).ToString())) + "/" +
                                     SqlFunctions.DatePart("year", sa.affected_date) : "",
                         calss_name = sa.ClassByGrade.class_name,
                         time_table_id = sa.time_table_id
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
        public ActionResult GetSubjecListByAcademic(int? grade_id, int? year)
        {
            var year_id = db.Years.Where(x => year >= x.from_year && year <= x.to_year).OrderBy(x=>x.from_year).Select(x => x.year_id).FirstOrDefault();
            var data = (from s in db.SubjectByGrades
                        where s.year_id == year_id && s.grade_id == grade_id
                        select new SubjectOfAcademciViewModel()
                        {
                            subject_id = s.subject_id,
                            subject_name = s.subject_name,
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetClassListByAcademic(int? grade_id)
        {
            var data = (from s in db.ClassByGrades
                        where s.grade_id == grade_id
                        select new ClassofAcademicViewModel()
                        {
                            class_id = s.class_id,
                            class_name = s.class_name,
                            description = s.description
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}