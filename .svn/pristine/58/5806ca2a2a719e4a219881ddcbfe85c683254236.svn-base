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
    public class NoteController : Controller
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(NoteController));
        protected static readonly string PhysicalPath = ConfigurationSettings.AppSettings.Get("PhysicalPath");

        private OrgService OrgService = new OrgService();
        // GET: AutoLogin
        public ActionResult Index()
        {
            var org = (CustomPrincipal)User;
            var entity = OrgService.GetByCode(org.Id);
            return View(entity);
        }

        public ActionResult Save(List<Note> model)
        {
            _log.Info($"model: {JsonConvert.SerializeObject(model)}");
            try
            {
                var org = (CustomPrincipal)User;
                var entity = OrgService.GetByCode(org.Id);
                entity.Notes = model;
                var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);
                OrgService.FindOneAndReplace(filter, entity);

                return Json(new Response(true, "Cập nhật thành công"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return Json(new Response(false, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
    }
}