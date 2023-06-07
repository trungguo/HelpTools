using log4net;
using SaleDXGui.Models;
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
    [AllowAnonymous]
    public class AccountController : Controller
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(AccountController));
        protected static readonly string SysadminOrg = ConfigurationSettings.AppSettings.Get("SysadminOrg");
        protected static readonly string SysadminAcc = ConfigurationSettings.AppSettings.Get("SysadminAcc");
        protected static readonly string SysadminPass = ConfigurationSettings.AppSettings.Get("SysadminPass");
        private OrgService orgService = new OrgService();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.OrgCode) || string.IsNullOrWhiteSpace(model.Account) || string.IsNullOrWhiteSpace(model.Password))
                {
                    throw new Exception("Thiếu thông tin đăng nhập");
                }

                var valid = false;
                //check sysadmin account
                if (model.OrgCode.ToLower() == SysadminOrg.ToLower() && model.Account.ToLower() == SysadminAcc.ToLower() && model.Password.ToLower() == SysadminPass.ToLower())
                {
                    valid = true;
                }
                else
                {
                    var org = orgService.GetByCode(model.OrgCode);
                    if (org != null && !org.DeActived)
                    {
                        if (model.OrgCode.ToLower() == org.Code.ToLower() && model.Account.ToLower() == org.Account.ToLower() && model.Password.ToLower() == org.Password.ToLower())
                        {
                            valid = true;
                        }
                    }
                }

                if (!valid)
                {
                    throw new Exception("Đăng nhập thất bại! Vui lòng kiểm tra lại tài khoản");
                }

                AuthenticationHelper.Authenticate(model);
                return Json(new Response(true, "Đăng nhập thành công"));
            }
            catch (Exception ex)
            {
                AuthenticationHelper.SignOut();
                return Json(new Response(false, ex.Message));
            }
        }

        public ActionResult Logout()
        {
            AuthenticationHelper.SignOut();
            return View("Login");
        }
    }
}