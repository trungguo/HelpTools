using log4net;
using MongoDB.Bson;
using MongoDB.Driver;
using SaleDXGui.Domains;
using SaleDXGui.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SaleDXGui.Areas.Api.Controllers
{
    public class AutoLoginController : ApiController
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(AutoLoginController));
        protected static readonly string PhysicalPath = ConfigurationSettings.AppSettings.Get("PhysicalPath");

        private AutoLoginConfigService AutoLoginService = new AutoLoginConfigService();

        [HttpPost]
        [Route("api/AutoLogin/UpdateStatus")]
        public IHttpActionResult UpdateStatus(string code, AutoLogin model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) || model == null || string.IsNullOrWhiteSpace(model.Name))
                {
                    throw new Exception("Thiếu tham số");
                }

                var entity = AutoLoginService.GetByCode(code);
                if (entity == null)
                {
                    throw new Exception("Thiếu tham số");
                }

                if (entity.Config != null && entity.Config.Count > 0)
                {
                    var al = entity.Config.FirstOrDefault(t => t.Name == model.Name);
                    al.Status = model.Status;
                    al.CheckedDate = DateTime.Now;

                    var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);
                    AutoLoginService.FindOneAndReplace(filter, entity);
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
