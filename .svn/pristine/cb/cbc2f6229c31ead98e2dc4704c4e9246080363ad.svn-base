using log4net;
using SaleDXGui.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleDXGui.Controllers
{
    [CustomAuthorize]
    public class BaseController : Controller
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(BaseController));
        // GET: Base
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DownloadExcel(string fileId, string fileName)
        {
            try
            {
                if (TempData[fileId] != null)
                {
                    byte[] data = TempData[fileId] as byte[];
                    return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Failed: {ex.Message}\n {ex.StackTrace}");
                return new EmptyResult();
            }
        }
    }
}