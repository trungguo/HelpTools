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
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SaleDXGui.Controllers
{
    [CustomAuthorize]
    public class CustomerController : Controller
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(CustomerController));
        private CustomerService CustomerService = new CustomerService();
        private ChannelService ChannelService = new ChannelService();
        // GET: Customer
        public ActionResult Index()
        {
            ViewBag.Channels = ChannelService.GetAll(null, Builders<BsonDocument>.Sort.Ascending("Name"));
            return View();
        }

        public ActionResult _List(SearchModel<CustomerSearchModel> searchModel)
        {
            _log.Info($"searchModel: {JsonConvert.SerializeObject(searchModel)}");
            try
            {
                var org = (CustomPrincipal)User;
                searchModel.OrgCode = org.Id;

                long total;
                var data = CustomerService.Search(searchModel, out total);
                var result = new SearchResultModel<CustomerSearchModel, List<Customer>>()
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
        public ActionResult _PopupList(SearchModel<CustomerSearchModel> searchModel)
        {
            _log.Info($"searchModel: {JsonConvert.SerializeObject(searchModel)}");
            try
            {
                var org = (CustomPrincipal)User;
                searchModel.OrgCode = org.Id;

                long total;
                var data = CustomerService.Search(searchModel, out total);
                var result = new SearchResultModel<CustomerSearchModel, List<Customer>>()
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

                var viewModel = new Customer();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    viewModel = CustomerService.FindOne(id);
                }

                var contactPoints = new List<ContactPoint>();
                var channels = ChannelService.GetAll(null, Builders<BsonDocument>.Sort.Ascending("Name"));
                if (channels != null)
                {
                    contactPoints = channels.Select(t =>
                    {
                        var value = viewModel.ContactPoints?.FirstOrDefault(k => k.Channel.Code == t.Code);
                        return new ContactPoint()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Channel = t,
                            UID = value?.UID ?? string.Empty
                        };
                    }).ToList();
                }
                viewModel.ContactPoints = contactPoints;

                ViewBag.Channels = channels;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return View(new Customer());
            }
        }

        public ActionResult Save(Customer model)
        {
            _log.Info($"model: {JsonConvert.SerializeObject(model)}");
            try
            {
                var org = (CustomPrincipal)User;
                if (string.IsNullOrEmpty(model.Code))
                {
                    throw new Exception("Thiếu thông tin");
                }

                var contactPoints = new List<ContactPoint>();
                if (model.ContactPoints != null)
                {
                    var channels = ChannelService.GetAll(null, Builders<BsonDocument>.Sort.Ascending("Name"));
                    contactPoints = model.ContactPoints?.Where(k => !string.IsNullOrEmpty(k.UID))?.Select(t =>
                      {
                          return new ContactPoint()
                          {
                              Id = Guid.NewGuid().ToString(),
                              UID = t.UID,
                              Channel = channels.FirstOrDefault(k => k.Code == t.Channel.Code)
                          };
                      })?.ToList();
                }

                model.ContactPoints = contactPoints;
                model.ContactPointString = GetContactPointString(model.ContactPoints);
                if (model.Labels != null)
                {
                    model.Labels.ForEach(k => k = k.Trim());
                }

                if (model.DOB.HasValue)
                {
                    model.DOBMonth = model.DOB.Value.ToLocalTime().Month;
                    model.DOBDay = model.DOB.Value.ToLocalTime().Day;
                }

                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.OrgCode = org.Id;
                    model.CreatedDate = DateTime.Today;
                    model.UpdatedDate = DateTime.Today;
                    CustomerService.InsertOne(model);
                }
                else
                {
                    var old = CustomerService.FindOne(model.Id);

                    old.Code = model.Code;
                    old.Type = model.Type;
                    old.Name = model.Name;
                    old.DOB = model.DOB;
                    old.DOBMonth = model.DOBMonth;
                    old.DOBDay = model.DOBDay;
                    //old.Sex = model.Sex;
                    old.Phone = model.Phone;
                    old.Email = model.Email;
                    old.Note = model.Note;
                    old.Labels = model.Labels;
                    //old.LabelsString = model.LabelsString;
                    old.ContactPoints = model.ContactPoints;
                    old.ContactPointString = model.ContactPointString;
                    old.UpdatedDate = DateTime.Today;
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", old.Id);
                    CustomerService.FindOneAndReplace(filter, old);
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
                CustomerService.DeleteByFilter(filter);

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
                var list = ExcelHelper.GetDataFromFile<Customer>(file);
                if (list == null || list.Count == 0)
                    throw new Exception("File không có dữ liệu");

                list.ForEach(t =>
                {
                    if (!string.IsNullOrWhiteSpace(t.ContactPointString))
                    {
                        t.ContactPoints = GetContactPointFromString(t.ContactPointString);
                    }
                    if (t.DOB.HasValue)
                    {
                        t.DOBMonth = t.DOB.Value.ToLocalTime().Month;
                        t.DOBDay = t.DOB.Value.ToLocalTime().Day;
                    }


                    t.Id = Guid.NewGuid().ToString();
                    t.OrgCode = org.Id;
                    t.Labels = t.LabelsString?.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? new List<string>();
                    t.Labels.ForEach(k => k = k.Trim());
                    t.CreatedDate = DateTime.Today;
                    t.UpdatedDate = DateTime.Today;
                });

                CustomerService.InsertMany(list);

                return Json(new Response(true, $"Thêm thành công {list.Count} khách hàng"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new Response(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Export(SearchModel<CustomerSearchModel> searchModel)
        {
            try
            {
                var org = (CustomPrincipal)User;

                searchModel.OrgCode = org.Id;
                searchModel.PageSize = 0;
                long total;
                var data = CustomerService.Search(searchModel, out total);
                if (data == null)
                {
                    data = new List<Customer>();
                }

                data.ForEach(t =>
                {
                    t.LabelsString = string.Join("|", t.Labels ?? new List<string>());
                    if (t.DOB.HasValue)
                    {
                        t.DOB = t.DOB.Value.ToLocalTime();
                    }
                });

                var properties = new List<string>() { "Code", "Type", "Name", "LabelsString", "ContactPointString", "DOB", "Phone", "Email" };

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

        public string GetContactPointString(List<ContactPoint> list)
        {
            var sb = new StringBuilder();

            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (!string.IsNullOrWhiteSpace(item.UID))
                    {
                        var subString = $"{item.Channel.Name}:{item.UID}";
                        sb.AppendLine(subString);
                    }
                }
            }

            return sb.ToString();
        }
        public List<ContactPoint> GetContactPointFromString(string s)
        {
            var result = new List<ContactPoint>();

            if (!string.IsNullOrWhiteSpace(s))
            {
                var channels = ChannelService.GetAll(null, Builders<BsonDocument>.Sort.Ascending("Name"));
                foreach (var item in s.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        var channelName = item.Split(':').FirstOrDefault();
                        var channelValue = item.Split(':').LastOrDefault();
                        var channel = channels.FirstOrDefault(t => t.Name.ToLower() == channelName.Trim().ToLower());
                        result.Add(new ContactPoint()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Channel = channel,
                            UID = channelValue?.Trim()
                        });
                    }
                }
            }

            return result;
        }
    }
}