using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SaleDXGui.Domains
{
    public class AutoLoginConfig
    {
        public string Id { get; set; }
        public string OrgCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Được sinh ra từ cấu hình
        /// </summary>
        public string Content { get; set; }
        public string Note { get; set; }
        public List<AutoLogin> Config { get; set; }
    }

    public class AutoLogin
    {
        public string Id { get; set; }
        [Description("Tên")]
        public string Name { get; set; }
        [Description("Tài khoản")]
        public string Account { get; set; }
        [Description("Mật khẩu")]
        public string Password { get; set; }
        [Description("N2FA")]
        public string N2FA { get; set; }
        /// <summary>
        /// Live | Gone
        /// Sống | Chết
        /// </summary>
        public string Status { get; set; }
        public DateTime? CheckedDate { get; set; }
    }
}