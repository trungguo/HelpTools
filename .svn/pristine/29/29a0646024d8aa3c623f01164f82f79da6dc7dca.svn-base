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
    public class IMAccountConfigController : Controller
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(IMAccountConfigController));
        private CustomerService CustomerService = new CustomerService();
        private IMAccountConfigService IMAccountConfigService = new IMAccountConfigService();
        private AccountService AccountService = new AccountService();
        private ChannelService ChannelService = new ChannelService();
        protected static readonly string PhysicalPath = ConfigurationSettings.AppSettings.Get("PhysicalPath");

        // GET: IMAccountConfig
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
                    filter &= (Builders<BsonDocument>.Filter.Eq("Name", reg) | Builders<BsonDocument>.Filter.Eq("Phone", reg)
                        | Builders<BsonDocument>.Filter.Eq("Email", reg) | Builders<BsonDocument>.Filter.Eq("Note", reg));
                }

                var sort = Builders<BsonDocument>.Sort.Ascending("Name");

                long total;
                var data = IMAccountConfigService.Paging(filter, sort, (searchModel.PageIndex - 1) * searchModel.PageSize, searchModel.PageSize, out total);
                var result = new SearchResultModel<List<IMAccountConfig>>()
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

                var viewModel = new IMAccountConfig();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    viewModel = IMAccountConfigService.FindOne(id);
                }

                ViewBag.Channels = ChannelService.GetAll(null, Builders<BsonDocument>.Sort.Ascending("Name"));
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return View(new IMAccountConfig());
            }
        }

        public ActionResult Save(IMAccountConfig model)
        {
            _log.Info($"model: {JsonConvert.SerializeObject(model)}");
            try
            {
                var org = (CustomPrincipal)User;

                if (string.IsNullOrEmpty(model.Code) || string.IsNullOrEmpty(model.Name))
                {
                    throw new Exception("Thiếu thông tin");
                }

                if (model.Items != null)
                {
                    model.Items.ForEach(t =>
                    {
                        if (string.IsNullOrWhiteSpace(t.Id))
                        {
                            t.Id = Guid.NewGuid().ToString();
                            t.Channel = ChannelService.GetByCode(t.Channel.Code);
                        }
                    });
                }
                model.Content = GetContent(model.Items, model.FriendsIMess);

                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.OrgCode = org.Id;
                    IMAccountConfigService.InsertOne(model);

                }
                else
                {
                    var old = IMAccountConfigService.FindOne(model.Id);
                    old.Code = model.Code;
                    old.Name = model.Name;
                    old.FriendsIMess = model.FriendsIMess;
                    old.Items = model.Items;
                    old.Content = model.Content;

                    var filter = Builders<BsonDocument>.Filter.Eq("_id", old.Id);
                    IMAccountConfigService.FindOneAndReplace(filter, old);
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
                IMAccountConfigService.DeleteByFilter(filter);

                return Json(new Response(true, "Xóa thành công"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new Response(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public string GetContent(List<IMAccountConfigItem> items, string IMess)
        {
            var sb = new StringBuilder();

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (!string.IsNullOrWhiteSpace(item.Account) && !string.IsNullOrWhiteSpace(item.Channel?.Code))
                    {
                        //[TNTK] Sale_1|Telegram
                        sb.AppendLine($"[TNTK] {item.Account}|{item.Channel.Code}");
                        sb.AppendLine($"[TNFF] Bám đuổi khách hàng : {item.Friends}");
                        sb.AppendLine($"[TNGF] Bám đuổi hội nhóm : {item.Groups}");
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(IMess))
            {
                sb.AppendLine($"[TNTK] IMess|IMess");
                sb.AppendLine($"[TNFF] Bám đuổi khách hàng : {IMess}");
            }

            return sb.ToString();
        }

        public void SyncToTextPad(IMAccountConfig model)
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