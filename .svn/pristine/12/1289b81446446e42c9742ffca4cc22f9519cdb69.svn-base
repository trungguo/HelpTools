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
    public class AlbumController : Controller
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(AlbumController));
        protected static readonly string PhysicalPath = ConfigurationSettings.AppSettings.Get("PhysicalPath");

        private AlbumService AlbumService = new AlbumService();
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
                    filter &= (Builders<BsonDocument>.Filter.Eq("Code", reg) | Builders<BsonDocument>.Filter.Eq("Name", reg));
                }

                var sort = Builders<BsonDocument>.Sort.Ascending("Code");

                long total;
                var data = AlbumService.Paging(filter, sort, (searchModel.PageIndex - 1) * searchModel.PageSize, searchModel.PageSize, out total);
                var result = new SearchResultModel<List<Album>>()
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

                var viewModel = new Album();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    viewModel = AlbumService.FindOne(id);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return View(new Album());
            }
        }

        public ActionResult Save(Album model)
        {
            _log.Info($"model: {JsonConvert.SerializeObject(model)}");
            try
            {
                var org = (CustomPrincipal)User;

                if (string.IsNullOrEmpty(model.Code))
                {
                    throw new Exception("Thiếu thông tin");
                }


                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.OrgCode = org.Id;
                    AlbumService.InsertOne(model);
                }
                else
                {
                    var old = AlbumService.FindOne(model.Id);

                    old.Code = model.Code;
                    old.Name = model.Name;
                    old.VideoDescriptions = model.VideoDescriptions;
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", old.Id);
                    AlbumService.FindOneAndReplace(filter, old);
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
                AlbumService.DeleteByFilter(filter);

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
                var list = ExcelHelper.GetDataFromFile<VideoDescription>(file);
                if (list == null || list.Count == 0)
                    throw new Exception("File không có dữ liệu");

                return Json(new ResponseList<VideoDescription>(true, string.Empty, list, list.Count), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new ResponseList<VideoDescription>(false, ex.Message, null, 0), JsonRequestBehavior.AllowGet);
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
                var model = AlbumService.FindOne(id);
                if (model == null)
                {
                    throw new Exception("Không tìm thấy bản ghi");
                }
                var list = new List<VideoDescription>();

                if (model.VideoDescriptions != null)
                {
                    list = model.VideoDescriptions.Select(t => new VideoDescription()
                    {
                        Description = t
                    }).ToList();
                }

                var properties = new List<string>() { "Description" };

                var stream = ExcelHelper.ExportToFile(list, properties);
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
    }
}