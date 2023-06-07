using Newtonsoft.Json;
using SaleDXGui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SaleDXGui.Security
{
    public class AuthenticationHelper
    {
        public static void Authenticate(LoginModel model)
        {
            if (model == null) return;
            model.Password = string.Empty;
            //set authen ticket
            double totalSeconds = FormsAuthentication.Timeout.TotalSeconds;
            string ticketName = string.Format("{0}@{1}@{2}", model.OrgCode, model.Account, model.Password);
            FormsAuthenticationTicket authticket = new FormsAuthenticationTicket(1,               // version 
                                                              ticketName,  // user name
                                                              DateTime.Now,    // create time
                                                              DateTime.Now.AddSeconds(totalSeconds), // expire time
                                                              false,           // persistent
                                                              JsonConvert.SerializeObject(model));             // user data

            string strEncryptedTicket = FormsAuthentication.Encrypt(authticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strEncryptedTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);
            return;
        }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();
            HttpCookie c = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            c.Expires = DateTime.Now.AddMonths(-1);
            HttpContext.Current.Response.Cookies.Add(c);
            return;
        }
    }
}