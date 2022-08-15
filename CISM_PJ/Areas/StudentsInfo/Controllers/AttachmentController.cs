using CISM_PJ.Areas.Setup.Models;
using CISM_PJ.Areas.StudentsInfo.Models;
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
using System.Web.Http;
using System.IO;
using System.Web.Script.Serialization;
using System.Net;

namespace CISM_PJ.Areas.StudentsInfo.Controllers
{
    public class AttachmentController : BaseController
    {
        private string ftp_URL;
        private string ftp_UserID;
        private string ftp_Password;
        private ErrorMessage message = new ErrorMessage();
        // GET: WOs/Attachment
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult FileUpload_ToTmp()
        {
            try
            {
                string path = "~/Uploads/" + System.Web.HttpContext.Current.Session.SessionID;
                string foldercreate = System.Web.HttpContext.Current.Server.MapPath(path);
                if (!Directory.Exists(foldercreate))
                {
                    Directory.CreateDirectory(foldercreate);

                }
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var fileupload = System.Web.HttpContext.Current.Request.Files["UploadFiles"];
                    if (fileupload != null)
                    {
                        var savefiles = Path.Combine(System.Web.HttpContext.Current.
                            Server.MapPath(path), fileupload.FileName);
                        fileupload.SaveAs(savefiles);
                    }
                    message.message = "File Upload Success";
                    message.errorcode = 0;
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    message.message = "File Upload Fail";
                    message.errorcode = -1;
                    return Json(message, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                message.message = "File Upload Fail";
                message.errorcode = -1;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        //[System.Web.Http.HttpPost]
        //public ActionResult MultipleFileUpload_ToTmp(string str_)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(str_))
        //        {
        //            //var file_array = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(str_);
        //            JavaScriptSerializer js = new JavaScriptSerializer();
        //            AttachmentModel[] persons = js.Deserialize<AttachmentModel[]>(str_);

        //            string path = "~/Uploads/" + System.Web.HttpContext.Current.Session.SessionID;
        //            string foldercreate = System.Web.HttpContext.Current.Server.MapPath(path);
        //            if (!Directory.Exists(foldercreate))
        //            {
        //                Directory.CreateDirectory(foldercreate);

        //            }
        //            foreach (var a in persons)
        //            {
        //                byte[] imageBytes = Convert.FromBase64String(a.file64String);

        //                //Save the Byte Array as Image File.
        //                string filePath = Server.MapPath(path + "/" + a.filename);
        //                System.IO.File.WriteAllBytes(filePath, imageBytes);
        //            }


        //            return Json("File Upload Success", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("An error occured. please contact Administartor", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Json("Fail to upload", JsonRequestBehavior.AllowGet);
        //    }

        //}

        //[System.Web.Http.HttpPost]
        //public ActionResult DeleteFile(string fileName_)
        //{
        //    try
        //    {
        //        string rootFolder = "Uploads\\" + System.Web.HttpContext.Current.Session.SessionID;


        //        string AppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        //        string fullpath = AppPath + rootFolder + "\\" + fileName_;
        //        // Check if file exists with its full path 
        //        if (System.IO.File.Exists(fullpath))
        //        {
        //            // If file found, delete it    
        //            System.IO.File.Delete(fullpath);
        //            DirectoryInfo info = new DirectoryInfo(AppPath + rootFolder);
        //            long totalSize = info.EnumerateFiles().Sum(file => file.Length);
        //            if (totalSize <= 0) ///if no data in session folder , delete this session folder
        //            {
        //                Directory.Delete(AppPath + rootFolder);
        //                //  System.IO.File.Delete(AppPath + rootFolder);
        //            }
        //            return Json("File Deleted Successfully", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("File not Found", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (IOException ioExp)
        //    {
        //        return Json(ioExp.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public void DeleteFile_FTPandDB(string fileName, string wo_id)
        //{
        //    try
        //    {
        //        var compSettings = db.a00_comp_setting.ToList();

        //        ftp_URL = compSettings.Where(w => w.col_flag_name == "ftp_link").Select(s => s.col_flag_value).FirstOrDefault() + "/WO/";
        //        ftp_UserID = compSettings.Where(w => w.col_flag_name == "ftp_user").Select(s => s.col_flag_value).FirstOrDefault();
        //        ftp_Password = compSettings.Where(w => w.col_flag_name == "ftp_pwd").Select(s => s.col_flag_value).FirstOrDefault();

        //        string ftp_folder = ftp_URL + wo_id;
        //        ///////////////Start of Delete file from FTP
        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp_folder + "/" + fileName);
        //        request.Method = WebRequestMethods.Ftp.DeleteFile;
        //        request.Credentials = new NetworkCredential(ftp_UserID, ftp_Password);


        //        Guid _woid = new Guid(wo_id);
        //        var _attchFile = db.wo_photo.Where(x => x.wo_id == _woid && x.wo_photo_name == fileName).Select(x => x).ToList();
        //        db.wo_photo.RemoveRange(_attchFile);

        //        var wo_info = db.wo_mst.Where(x => x.wo_id == _woid).Select(x => x).FirstOrDefault();

        //        wo_info.modifieddate = DateTime.Now;
        //        wo_info.modifieduser = GetUserID();

        //        db.SaveChanges();
        //        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        //    }
        //    catch (IOException ioExp)
        //    {
        //        throw ioExp;
        //    }
        //}

        //public ActionResult GetFileList_FromTmp()
        //{
        //    try
        //    {
        //        string rootFolder = "Uploads\\" + System.Web.HttpContext.Current.Session.SessionID;

        //        string AppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        //        string fullDirpath = AppPath + rootFolder;
        //        FileInfo fileInfo = new FileInfo(fullDirpath);
        //        // Check if file exists with its full path     //System.IO.File.Exists(fullDirpath)
        //        if (Directory.Exists(fullDirpath))
        //        {
        //            List<string> fileList = (Directory
        //                .GetFiles(fullDirpath, "*", SearchOption.AllDirectories)
        //                .Select(f => Path.GetFileName(f))).ToList();

        //            return Json(fileList, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            List<string> EmptyList = new List<string>();
        //            return Json(EmptyList, JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public ActionResult GetFileList_FromDB(string wo_id)
        //{
        //    try
        //    {
        //        Guid guid = string.IsNullOrEmpty(wo_id) ? Guid.Empty : new Guid(wo_id);
        //        //List<wo_photo> list = db.wo_photo.AsEnumerable().Where(x => x.wo_id == guid).Select(x => x).ToList();
        //        List<ViewAttachmentModel> directories = new List<ViewAttachmentModel>();

        //        var list = (from p in db.wo_photo
        //                    where p.wo_id == guid
        //                    select new ViewAttachmentModel()
        //                    {
        //                        wo_photo_id = p.wo_photo_id,
        //                        wo_photo_name = p.wo_photo_name,
        //                        photo_comments = string.IsNullOrEmpty(p.photo_comments) ? string.Empty : p.photo_comments
        //                    }).ToList();
        //        foreach (ViewAttachmentModel file in list)
        //        {
        //            if (file.wo_photo_name.ToLower() != (wo_id + "sign.png").ToLower())
        //                directories.Add(file);
        //        }
        //        return Json(directories, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public ActionResult GetFileList_FromDB_ForSubWO(string sub_wo_id)
        //{
        //    try
        //    {
        //        Guid guid = string.IsNullOrEmpty(sub_wo_id) ? Guid.Empty : new Guid(sub_wo_id);
        //        //List<wo_photo> list = db.wo_photo.AsEnumerable().Where(x => x.wo_id == guid).Select(x => x).ToList();
        //        List<ViewAttachmentModel> directories = new List<ViewAttachmentModel>();

        //        var list = (from p in db.wo_photo
        //                    where p.sub_wo_id == guid
        //                    select new ViewAttachmentModel()
        //                    {
        //                        wo_photo_id = p.wo_photo_id,
        //                        wo_photo_name = p.wo_photo_name,
        //                        photo_comments = string.IsNullOrEmpty(p.photo_comments) ? string.Empty : p.photo_comments
        //                    }).ToList();
        //        foreach (ViewAttachmentModel file in list)
        //        {
        //            if (file.wo_photo_name.ToLower() != (sub_wo_id + "sign.png").ToLower())
        //                directories.Add(file);
        //        }
        //        return Json(directories, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public ActionResult GetFileList_FromFTP(string wo_id)
        //{
        //    try
        //    {
        //        var compSettings = db.a00_comp_setting.ToList();

        //        ftp_URL = compSettings.Where(w => w.col_flag_name == "ftp_link").Select(s => s.col_flag_value).FirstOrDefault() + "/WO/";
        //        ftp_UserID = compSettings.Where(w => w.col_flag_name == "ftp_user").Select(s => s.col_flag_value).FirstOrDefault();
        //        ftp_Password = compSettings.Where(w => w.col_flag_name == "ftp_pwd").Select(s => s.col_flag_value).FirstOrDefault();

        //        WorkOrderController wo = new WorkOrderController();
        //        string ftp_folder = ftp_URL + wo_id;
        //        if (wo.DoesFtpDirectoryExist(ftp_folder))
        //        {

        //            // Check if file exists with its full path     //System.IO.File.Exists(fullDirpath)
        //            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftp_folder);

        //            ftpRequest.Credentials = new NetworkCredential(ftp_UserID, ftp_Password);
        //            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
        //            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
        //            StreamReader streamReader = new StreamReader(response.GetResponseStream());

        //            List<string> directories = new List<string>();

        //            string line = streamReader.ReadLine();
        //            while (!string.IsNullOrEmpty(line))
        //            {
        //                directories.Add(line);
        //                line = streamReader.ReadLine();

        //            }

        //            streamReader.Close();
        //            return Json(directories, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public ActionResult DownLoadFromFTP(string fileName, string wo_id)
        //{
        //    var compSettings = db.a00_comp_setting.ToList();

        //    ftp_URL = compSettings.Where(w => w.col_flag_name == "ftp_link").Select(s => s.col_flag_value).FirstOrDefault() + "/WO/";
        //    ftp_UserID = compSettings.Where(w => w.col_flag_name == "ftp_user").Select(s => s.col_flag_value).FirstOrDefault();
        //    ftp_Password = compSettings.Where(w => w.col_flag_name == "ftp_pwd").Select(s => s.col_flag_value).FirstOrDefault();

        //    string ftp_folder = ftp_URL + wo_id;

        //    string full_Filepath = ftp_folder + "/" + fileName;
        //    // Get the object used to communicate with the server.
        //    //Create FTP Request
        //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(full_Filepath);
        //    request.Method = WebRequestMethods.Ftp.DownloadFile;

        //    //Enter FTP Server credentials.
        //    request.Credentials = new NetworkCredential(ftp_UserID, ftp_Password);
        //    request.UsePassive = true;
        //    request.UseBinary = true;
        //    request.EnableSsl = false;

        //    var ftpResponse = (FtpWebResponse)request.GetResponse();
        //    /* Get the FTP Server's Response Stream */
        //    var ftpStream = ftpResponse.GetResponseStream();

        //    // TODO: you might need to extract these settings from the FTP response
        //    const string contentType = "application/zip";
        //    //  const string fileNameDisplayedToUser = "FileName.zip";

        //    return File(ftpStream, contentType, fileName);

        //}
    }
}