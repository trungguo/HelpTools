using log4net;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SaleDXGui.Domains;
using SaleDXGui.Heplers;
using SaleDXGui.Models;
using SaleDXGui.Security;
using SaleDXGui.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SaleDXGui.Controllers
{
    [CustomAuthorize]
    public class AutoLoginController : Controller
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(AutoLoginController));
        protected static readonly string PhysicalPath = ConfigurationSettings.AppSettings.Get("PhysicalPath");

        private AutoLoginConfigService AutoLoginService = new AutoLoginConfigService();
        // GET: AutoLogin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _List(SearchModel searchModel)
        {
            _log.Info($"searchModel: {JsonConvert.SerializeObject(searchModel)}");
            try
            {
                var org = (CustomPrincipal)User;

                var filter = Builders<BsonDocument>.Filter.Eq("OrgCode", org.Id);
                if (!string.IsNullOrWhiteSpace(searchModel.Keyword))
                {
                    searchModel.Keyword = searchModel.Keyword.Trim();
                    var reg = new BsonRegularExpression(searchModel.Keyword, "i");
                    filter &= (Builders<BsonDocument>.Filter.Eq("UID", reg) | Builders<BsonDocument>.Filter.Eq("Name", reg)
                        | Builders<BsonDocument>.Filter.Eq("Phone", reg) | Builders<BsonDocument>.Filter.Eq("Email", reg)
                        | Builders<BsonDocument>.Filter.Eq("Note", reg));
                }

                var sort = Builders<BsonDocument>.Sort.Ascending("Code");

                long total;
                var data = AutoLoginService.Paging(filter, sort, (searchModel.PageIndex - 1) * searchModel.PageSize, searchModel.PageSize, out total);
                var result = new SearchResultModel<List<AutoLoginConfig>>()
                {
                    SearchModel = searchModel,
                    Data = data,
                    Total = total
                };

                return PartialView(result);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return PartialView(null);
            }
        }

        public ActionResult Details(string id)
        {
            _log.Info($"id: {id}");
            try
            {
                var org = (CustomPrincipal)User;

                var viewModel = new AutoLoginConfig();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    viewModel = AutoLoginService.FindOne(id);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return View(new AutoLoginConfig());
            }
        }

        public ActionResult Save(AutoLoginConfig model)
        {
            _log.Info($"model: {JsonConvert.SerializeObject(model)}");
            try
            {
                var org = (CustomPrincipal)User;

                if (string.IsNullOrEmpty(model.Code))
                {
                    throw new Exception("Thiếu thông tin");
                }

                if (model.Config != null)
                {
                    model.Config.ForEach(t =>
                    {
                        if (string.IsNullOrWhiteSpace(t.Id))
                        {
                            t.Id = Guid.NewGuid().ToString();
                        }
                    });
                }
                model.Content = GetContent(model.Config);

                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.OrgCode = org.Id;
                    AutoLoginService.InsertOne(model);
                }
                else
                {
                    var old = AutoLoginService.FindOne(model.Id);

                    old.Code = model.Code;
                    old.Name = model.Name;
                    old.Note = model.Note;
                    old.Config = model.Config;
                    old.Content = model.Content;
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", old.Id);
                    AutoLoginService.FindOneAndReplace(filter, old);
                }

                SyncToTextPad(model);

                return Json(new Response(true, "Cập nhật thành công"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new Response(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(List<string> ids)
        {
            _log.Info($"ids: {JsonConvert.SerializeObject(ids)}");
            try
            {
                var org = (CustomPrincipal)User;

                if (ids == null || ids.Count == 0)
                {
                    throw new Exception("Thiếu thông tin");
                }

                var filter = Builders<BsonDocument>.Filter.In("_id", ids);
                AutoLoginService.DeleteByFilter(filter);

                return Json(new Response(true, "Xóa thành công"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new Response(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Import(HttpPostedFileBase file)
        {
            try
            {
                var org = (CustomPrincipal)User;
                var list = ExcelHelper.GetDataFromFile<AutoLogin>(file);
                if (list == null || list.Count == 0)
                    throw new Exception("File không có dữ liệu");

                return Json(new ResponseList<AutoLogin>(true, string.Empty, list, list.Count), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new ResponseList<AutoLogin>(false, ex.Message, null, 0), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Export(string id)
        {
            try
            {
                var org = (CustomPrincipal)User;

                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("Thiếu thông tin");
                }
                var model = AutoLoginService.FindOne(id);
                if (model == null)
                {
                    throw new Exception("Không tìm thấy bản ghi");
                }
                if (model.Config != null)
                {
                    model.Config.ForEach(t =>
                    {
                        if (t.CheckedDate != null)
                        {
                            t.CheckedDate = t.CheckedDate.Value.ToLocalTime();
                        }
                    });
                }

                var properties = new List<string>() { "Name", "Account", "Password", "N2FA", "Status", "CheckedDate" };

                var stream = ExcelHelper.ExportToFile(model.Config, properties);
                var FileId = Guid.NewGuid().ToString();
                TempData[FileId] = stream.ToArray();
                return Json(new Response<string>(true, string.Empty, FileId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new Response(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public string GetContent(List<AutoLogin> config)
        {
            var sb = new StringBuilder();

            if (config != null && config.Count > 0)
            {
                foreach (var item in config)
                {
                    var subString = string.Join("|", new List<string> { item.Name, item.Account, item.Password, item.N2FA });
                    sb.AppendLine(subString);
                }
            }

            return sb.ToString();
        }

        public void SyncToTextPad(AutoLoginConfig model)
        {
            var listExists = FileHelper.FindAllFileByUID(PhysicalPath, model.Code);
            if (listExists != null && listExists.Count > 0)
            {
                foreach (var exists in listExists)
                {
                    System.IO.File.Delete(exists);
                }
            }

            var fileName = $"{model.Code}_{model.Name}.txt";
            var filePath = PhysicalPath + "\\" + fileName;

            System.IO.File.WriteAllText(filePath, model.Content);
            return;
        }
    }
}