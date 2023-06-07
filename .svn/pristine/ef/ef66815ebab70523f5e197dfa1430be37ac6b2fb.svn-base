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
    public class ChannelController : Controller
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(OrgController));
        protected static readonly string SysadminOrg = ConfigurationSettings.AppSettings.Get("SysadminOrg");
        private ChannelService ChannelService = new ChannelService();
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
                    filter = Builders<BsonDocument>.Filter.Eq("Code", reg) | Builders<BsonDocument>.Filter.Eq("Name", reg);
                }

                var sort = Builders<BsonDocument>.Sort.Ascending("Code");

                long total;
                var data = ChannelService.Paging(filter, sort, (searchModel.PageIndex - 1) * searchModel.PageSize, searchModel.PageSize, out total);

                var result = new SearchResultModel<List<Channel>>()
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

                var viewModel = new Channel();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    viewModel = ChannelService.FindOne(id);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return View(new Org());
            }
        }

        public ActionResult Save(Channel model)
        {
            _log.Info($"model: {JsonConvert.SerializeObject(model)}");
            try
            {
                var org = (CustomPrincipal)User;
                if (org.Id != SysadminOrg)
                {
                    return Redirect("/Account/Logout");
                }

                if (string.IsNullOrEmpty(model.Code) || string.IsNullOrEmpty(model.Name))
                {
                    throw new Exception("Thiếu thông tin");
                }

                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    ChannelService.InsertOne(model);
                }
                else
                {
                    var old = ChannelService.FindOne(model.Id);

                    old.Code = model.Code;
                    old.Name = model.Name;
                    old.IsActive = model.IsActive;
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", old.Id);
                    ChannelService.FindOneAndReplace(filter, old);
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
                ChannelService.DeleteByFilter(filter);

                return Json(new Response(true, "Xóa thành công"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new Response(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchChannel(string keyword, int pageIndex, int pageSize)
        {
            _log.Info($"keyword: {keyword}; pageIndex: {pageIndex}; pageSize: {pageSize}");
            try
            {
                FilterDefinition<BsonDocument> filter = null;
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    keyword = keyword.Trim();
                    var reg = new BsonRegularExpression(keyword, "i");
                    filter = Builders<BsonDocument>.Filter.Eq("Code", reg) | Builders<BsonDocument>.Filter.Eq("Name", reg);
                }

                var sort = Builders<BsonDocument>.Sort.Ascending("Name");

                long total;
                var data = ChannelService.Paging(filter, sort, (pageIndex - 1) * pageSize, pageSize, out total);

                return Json(new ResponseList<Channel>(true, null, data, total), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new ResponseList<Channel>(false, ex.Message, new List<Channel>(), 0), JsonRequestBehavior.AllowGet);
            }
        }
    }
}