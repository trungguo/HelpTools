using log4net;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SaleDXGui.Domains;
using SaleDXGui.Models;
using SaleDXGui.Security;
using SaleDXGui.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace SaleDXGui.Controllers
{
    [CustomAuthorize]
    public class OrgController : Controller
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(OrgController));
        protected static readonly string SysadminOrg = ConfigurationSettings.AppSettings.Get("SysadminOrg");
        private OrgService OrgService = new OrgService();
        // GET: Org
        public ActionResult Index()
        {
            var org = (CustomPrincipal)User;
            if (org.Id != SysadminOrg)
            {
                return Redirect("/Account/Logout");
            }
            return View();
        }

        public ActionResult _List(SearchModel searchModel)
        {
            _log.Info($"searchModel: {JsonConvert.SerializeObject(searchModel)}");
            try
            {
                var org = (CustomPrincipal)User;
                if (org.Id != SysadminOrg)
                {
                    return Redirect("/Account/Logout");
                }

                FilterDefinition<BsonDocument> filter = null;
                if (!string.IsNullOrWhiteSpace(searchModel.Keyword))
                {
                    searchModel.Keyword = searchModel.Keyword.Trim();
                    var reg = new BsonRegularExpression(searchModel.Keyword, "i");
                    filter = Builders<BsonDocument>.Filter.Eq("Code", reg) | Builders<BsonDocument>.Filter.Eq("Note", reg);
                }

                var sort = Builders<BsonDocument>.Sort.Ascending("Code");

                long total;
                var data = OrgService.Paging(filter, sort, (searchModel.PageIndex - 1) * searchModel.PageSize, searchModel.PageSize, out total);

                var result = new SearchResultModel<List<Org>>()
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
                if (org.Id != SysadminOrg)
                {
                    return Redirect("/Account/Logout");
                }

                var viewModel = new Org();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    viewModel = OrgService.FindOne(id);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return View(new Org());
            }
        }

        public ActionResult Save(Org model)
        {
            _log.Info($"model: {JsonConvert.SerializeObject(model)}");
            try
            {
                var org = (CustomPrincipal)User;
                if (org.Id != SysadminOrg)
                {
                    return Redirect("/Account/Logout");
                }

                if (string.IsNullOrEmpty(model.Code) || string.IsNullOrEmpty(model.Account) || string.IsNullOrEmpty(model.Password))
                {
                    throw new Exception("Thiếu thông tin");
                }

                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.CustomerLabels = new List<string>();// { "Telegram", "Zalo", "Messenger" };
                    OrgService.InsertOne(model);
                }
                else
                {
                    var old = OrgService.FindOne(model.Id);

                    //old.Code = model.Code;
                    old.Account = model.Account;
                    old.Password = model.Password;
                    old.Note = model.Note;
                    old.DeActived = model.DeActived;
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", old.Id);
                    OrgService.FindOneAndReplace(filter, old);
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
                if (org.Id != SysadminOrg)
                {
                    return Redirect("/Account/Logout");
                }

                if (ids == null || ids.Count == 0)
                {
                    throw new Exception("Thiếu thông tin");
                }

                var filter = Builders<BsonDocument>.Filter.In("_id", ids);
                OrgService.DeleteByFilter(filter);

                return Json(new Response(true, "Xóa thành công"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new Response(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCustomerLabels()
        {
            try
            {
                var org = (CustomPrincipal)User;

                var orgModel = OrgService.GetByCode(org.Id);
                if (orgModel == null)
                {
                    throw new Exception("Không tìn thấy bản ghi");
                }

                if (orgModel.CustomerLabels == null)
                {
                    orgModel.CustomerLabels = new List<string>();
                }
                return PartialView("CustomerLabels", orgModel.CustomerLabels);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return PartialView("CustomerLabels", new List<string>());
            }
        }

        public ActionResult SaveCustomerLabels(List<string> list)
        {
            _log.Info($"list: {JsonConvert.SerializeObject(list)}");
            try
            {
                var org = (CustomPrincipal)User;

                var orgModel = OrgService.GetByCode(org.Id);
                if (orgModel == null)
                {
                    throw new Exception("Không tìn thấy bản ghi");
                }

                orgModel.CustomerLabels = list;

                var filter = Builders<BsonDocument>.Filter.Eq("_id", orgModel.Id);
                OrgService.FindOneAndReplace(filter, orgModel);

                return Json(new Response(true, "Cập nhật thành công"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new Response(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchCustomerLabels(string keyword, int pageIndex, int pageSize)
        {
            _log.Info($"keyword: {keyword}; pageIndex: {pageIndex}; pageSize: {pageSize}");
            try
            {
                var org = (CustomPrincipal)User;

                var orgModel = OrgService.GetByCode(org.Id);
                if (orgModel == null)
                {
                    throw new Exception("Không tìn thấy bản ghi");
                }

                var list = orgModel.CustomerLabels ?? new List<string>();
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    keyword = keyword?.Trim()?.ToLower() ?? "";
                    list = list.Where(t => t.Contains(keyword)).ToList();
                }
                var total = list.Count;
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize == 0 ? total : pageSize).OrderBy(t => t).ToList();
                return Json(new ResponseList<string>(true, null, list, total), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new ResponseList<string>(false, ex.Message, new List<string>(), 0), JsonRequestBehavior.AllowGet);
            }
        }
    }
}