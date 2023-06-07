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
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleDXGui.Controllers
{
    [CustomAuthorize]
    public class IMAccountController : Controller
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(IMAccountController));
        private AccountService AccountService = new AccountService();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _List(SearchModel<AccountSearchModel> searchModel)
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
                    filter &= (Builders<BsonDocument>.Filter.Eq("Name", reg) | Builders<BsonDocument>.Filter.Eq("Phone", reg)
                        | Builders<BsonDocument>.Filter.Eq("Email", reg) | Builders<BsonDocument>.Filter.Eq("Note", reg));
                }

                if (searchModel.Model?.Types != null && searchModel.Model?.Types.Count > 0)
                {
                    filter &= Builders<BsonDocument>.Filter.In("Type", searchModel.Model?.Types);
                }

                var sort = Builders<BsonDocument>.Sort.Ascending("Name");

                long total;
                var data = AccountService.Paging(filter, sort, (searchModel.PageIndex - 1) * searchModel.PageSize, searchModel.PageSize, out total);
                var result = new SearchResultModel<AccountSearchModel, List<Account>>()
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

                var viewModel = new Account();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    viewModel = AccountService.FindOne(id);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return View(new Account());
            }
        }

        public ActionResult Save(Account model)
        {
            _log.Info($"model: {JsonConvert.SerializeObject(model)}");
            try
            {
                var org = (CustomPrincipal)User;

                if (string.IsNullOrEmpty(model.Type) || string.IsNullOrEmpty(model.Name))
                {
                    throw new Exception("Thiếu thông tin");
                }

                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.OrgCode = org.Id;
                    AccountService.InsertOne(model);
                }
                else
                {
                    var old = AccountService.FindOne(model.Id);

                    old.Type = model.Type;
                    old.Name = model.Name;
                    old.Phone = model.Phone;
                    old.Email = model.Email;
                    old.Note = model.Note;
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", old.Id);
                    AccountService.FindOneAndReplace(filter, old);
                }
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
                AccountService.DeleteByFilter(filter);

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
                var list = ExcelHelper.GetDataFromFile<Account>(file);
                if (list == null || list.Count == 0)
                    throw new Exception("File không có dữ liệu");

                list.ForEach(t =>
                {
                    t.Id = Guid.NewGuid().ToString();
                    t.OrgCode = org.Id;
                });

                AccountService.InsertMany(list);

                return Json(new Response(true, $"Thêm thành công {list.Count} tài khoản"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new Response(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Export(SearchModel<AccountSearchModel> searchModel)
        {
            try
            {
                var org = (CustomPrincipal)User;

                var filter = Builders<BsonDocument>.Filter.Eq("OrgCode", org.Id);
                if (!string.IsNullOrWhiteSpace(searchModel.Keyword))
                {
                    searchModel.Keyword = searchModel.Keyword.Trim();
                    var reg = new BsonRegularExpression(searchModel.Keyword, "i");
                    filter &= (Builders<BsonDocument>.Filter.Eq("Name", reg) | Builders<BsonDocument>.Filter.Eq("Phone", reg)
                        | Builders<BsonDocument>.Filter.Eq("Email", reg) | Builders<BsonDocument>.Filter.Eq("Note", reg));
                }

                if (searchModel.Model?.Types != null && searchModel.Model?.Types.Count > 0)
                {
                    filter &= Builders<BsonDocument>.Filter.In("Type", searchModel.Model?.Types);
                }

                var sort = Builders<BsonDocument>.Sort.Ascending("Name");

                long total;
                var data = AccountService.Paging(filter, sort, (searchModel.PageIndex - 1) * searchModel.PageSize, 0, out total);

                if (data == null)
                {
                    data = new List<Account>();
                }

                var properties = new List<string>() { "Type", "Name", "Phone", "Email", "Note" };

                var stream = ExcelHelper.ExportToFile(data, properties);
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

        public ActionResult GetAccount(List<string> ids)
        {
            _log.Info($"ids: {JsonConvert.SerializeObject(ids)}");
            try
            {
                var org = (CustomPrincipal)User;

                if (ids == null || ids.Count == 0)
                {
                    throw new Exception("Thiếu thông tin");
                }

                var data = new List<Account>();
                foreach (var i in ids)
                {
                    var model = AccountService.FindOne(i);
                    data.Add(model);
                }
                var total = data.Count();
                return Json(new ResponseList<Account>(true, null, data, total));
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new Response(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
    }
}