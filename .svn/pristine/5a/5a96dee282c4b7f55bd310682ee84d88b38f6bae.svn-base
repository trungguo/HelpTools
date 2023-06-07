using Newtonsoft.Json;
using SaleDXGui.Models;
using SaleDXGui.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SaleDXGui.Security
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected string SysadminOrg = ConfigurationSettings.AppSettings.Get("SysadminOrg");
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);

            if (httpContext == null || httpContext.Request == null)
            {
                filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl + $"?returnUrl={httpContext.Request.RawUrl}");
                return;
            }

            HttpCookie authenCookie = httpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (authenCookie == null)
            {
                filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl + $"?returnUrl={httpContext.Request.RawUrl}");
                return;
            };

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authenCookie.Value);
            //validate
            if (ticket.Expired)
            {
                filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl + $"?returnUrl={httpContext.Request.RawUrl}");
                return;
            }

            var ticketData = ticket.UserData;
            var ssoticket = JsonConvert.DeserializeObject<LoginModel>(ticketData);

            if (ssoticket == null)
            {
                filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl + $"?returnUrl={httpContext.Request.RawUrl}");
                return;
            }

            if (ssoticket.OrgCode.ToLower() != SysadminOrg.ToLower())
            {
                var orgService = new OrgService();
                var org = orgService.GetByCode(ssoticket.OrgCode);
                if (org == null || org.DeActived)
                {
                    filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl + $"?returnUrl={httpContext.Request.RawUrl}");
                    return;
                }
            }

            var principal = new CustomPrincipal(ssoticket.OrgCode)
            {
                IsAuthenticated = true,
            };

            HttpContext.Current.User = principal;
            return;
        }
    }
}