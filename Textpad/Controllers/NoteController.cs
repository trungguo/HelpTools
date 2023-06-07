using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Textpad.Helpers;
using Textpad.Models;

namespace Textpad.Controllers
{
    public class NoteController : Controller
    {
        public const string FolderPath = @"\Data\";
        // GET: Note
        public ActionResult Index(string id)
        {
            try
            {
                var noteFile = new NoteFile();
                var path = Server.MapPath(FolderPath);
                if (string.IsNullOrWhiteSpace(id))
                {
                    return View(noteFile);
                }

                var exists = FileHelper.FindOneFileByUID(path, id);

                //var fileExists = System.IO.File.Exists(Server.MapPath(FolderPath + id + ".txt"));
                if (!string.IsNullOrWhiteSpace(exists))
                {
                    noteFile.Id = id;
                    noteFile.FileName = Path.GetFileName(exists).Replace($"{id}_", "").Replace(".txt", "");
                    noteFile.FileContent = FileHelper.GetStringFromPath(exists);
                }

                return View(noteFile);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Save(NoteFile file)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(file.Id))
                {
                    while (string.IsNullOrWhiteSpace(file.Id))
                    {
                        // sinh ra file id mới nếu là tạo mới
                        var fileId = StringHelper.RandomString(16);

                        //kiểm tra đã tồn tại chưa
                        var path = Server.MapPath(FolderPath);
                        var exists = FileHelper.FindOneFileByUID(path, fileId);
                        if (string.IsNullOrWhiteSpace(exists))
                        {
                            file.Id = fileId;
                        }

                        //var fileExists = System.IO.File.Exists(Server.MapPath(FolderPath + fileId + ".txt"));
                        //if (!fileExists)
                        //{
                        //    file.Id = fileId;
                        //}
                    }
                }
                else
                {
                    var path = Server.MapPath(FolderPath);
                    var listExists = FileHelper.FindAllFileByUID(path, file.Id);
                    if (listExists != null && listExists.Count > 0)
                    {
                        foreach (var exists in listExists)
                        {
                            System.IO.File.Delete(exists);
                        }
                    }
                }

                var fileName = $"{file.Id}_{file.FileName}.txt";
                var filePath = Server.MapPath(FolderPath + fileName);

                System.IO.File.WriteAllText(filePath, file.FileContent);

                return Json(new { data = file.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return Json(new { data = id }, JsonRequestBehavior.AllowGet);
                }

                var path = Server.MapPath(FolderPath);
                var exists = FileHelper.FindOneFileByUID(path, id);
                //var fileExists = System.IO.File.Exists(Server.MapPath(FolderPath + id + ".txt"));
                if (string.IsNullOrWhiteSpace(exists))
                {
                    return Json(new { data = id }, JsonRequestBehavior.AllowGet);
                }

                //var filePath = Server.MapPath(FolderPath + id + ".txt");
                System.IO.File.Delete(exists);

                return Json(new { data = id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Download(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new Exception("File không tồn tại");
                }

                //var fullPath = Server.MapPath(FolderPath + id + ".txt");
                var path = Server.MapPath(FolderPath);
                var exists = FileHelper.FindOneFileByUID(path, id);
                //var fileExists = System.IO.File.Exists(fullPath);
                if (string.IsNullOrWhiteSpace(exists))
                {
                    throw new Exception("File không tồn tại");
                }

                var filename = Path.GetFileName(exists);
                var ext = Path.GetExtension(filename);
                var mime = MimeMapping.GetMimeMapping(exists);
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = false,
                };

                Response.AppendHeader("Content-Disposition", cd.ToString());
                return File(exists, mime);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}