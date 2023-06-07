using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Textpad.Helpers;
using Textpad.Models;

namespace Textpad.Areas.Api.Controllers
{
    public class LogController : ApiController
    {
        public const string FolderPath = @"Data\Logs";
        [HttpPost]
        [Route("api/Log/WriteLog")]
        public IHttpActionResult WriteLog(LogModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.MaThietBi))
                {
                    throw new Exception("Chưa đăng ký thiết bị");
                }
                model.ThoiGian = DateTime.Now;
                model.DanhSachChayThatBaiList = model.DanhSachChayList?.Where(t => !(model.DanhSachChayThanhCongList?.Contains(t) ?? false))?.ToList();
                model.DanhSachChay = string.Join("|", model.DanhSachChayList ?? new List<string>());
                model.DanhSachChayThanhCong = string.Join("|", model.DanhSachChayThanhCongList ?? new List<string>());
                model.DanhSachChayThatBai = string.Join("|", model.DanhSachChayThatBaiList ?? new List<string>());
                var oldLogs = new List<LogModel>();
                var fileName = $"{model.MaThietBi}.txt";
                var path = HttpRuntime.AppDomainAppPath + FolderPath;
                var exists = FileHelper.FindOneFileByUID(path, model.MaThietBi);
                if (!string.IsNullOrWhiteSpace(exists))
                {
                    oldLogs = FileHelper.GetDataFromPath<List<LogModel>>(exists);
                }

                var updateLog = oldLogs.FirstOrDefault(t => t.MaThietBi == model.MaThietBi && t.MaCauHinh == model.MaCauHinh && t.UngDung == model.UngDung
                 && t.TaiKhoan == model.TaiKhoan && t.Nhip == model.Nhip);

                if (updateLog == null)
                {
                    oldLogs.Add(model);
                }
                else
                {
                    updateLog.ThoiGian = DateTime.Now;
                    updateLog.DanhSachChayList = model.DanhSachChayList;
                    updateLog.DanhSachChayThanhCongList = model.DanhSachChayThanhCongList;
                    updateLog.DanhSachChayThatBaiList = model.DanhSachChayThatBaiList;
                    updateLog.DanhSachChay = model.DanhSachChay;
                    updateLog.DanhSachChayThanhCong = model.DanhSachChayThanhCong;
                    updateLog.DanhSachChayThatBai = model.DanhSachChayThatBai;
                }

                oldLogs = oldLogs.OrderByDescending(t => t.ThoiGian).ToList();
                System.IO.File.WriteAllText($"{path}\\{fileName}", JsonConvert.SerializeObject(oldLogs, Formatting.Indented, new JsonSerializerSettings()
                { ContractResolver = new IgnorePropertiesResolver(new[] { "DanhSachChayList", "DanhSachChayThanhCongList", "DanhSachChayThatBaiList" }) }));

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class IgnorePropertiesResolver : DefaultContractResolver
    {
        private readonly HashSet<string> ignoreProps;
        public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore)
        {
            this.ignoreProps = new HashSet<string>(propNamesToIgnore);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (this.ignoreProps.Contains(property.PropertyName))
            {
                property.ShouldSerialize = _ => false;
            }
            return property;
        }
    }
}
