using SaleDXGui.Domains;
using SaleDXGui.Security;
using SaleDXGui.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleDXGui.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        protected static readonly string SysadminOrg = ConfigurationSettings.AppSettings.Get("SysadminOrg");
        public ActionResult Index()
        {
            var org = (CustomPrincipal)User;
            if (org.Id.ToLower() != SysadminOrg.ToLower())
            {
                return Redirect("/Customer");
            }
            else
            {
                return Redirect("/Org");
            }
        }
    }
}
