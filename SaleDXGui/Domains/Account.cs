using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SaleDXGui.Domains
{
    public class Account
    {
        public string Id { get; set; }
        public string OrgCode { get; set; }
        /// <summary>
        /// Telegram|Zalo|Messenger
        /// </summary>
        [Description("Loại")]
        public string Type { get; set; }
        [Description("Tên")]
        public string Name { get; set; }
        [Description("SĐT")]
        public string Phone { get; set; }
        [Description("Email")]
        public string Email { get; set; }
        [Description("Ghi chú")]
        public string Note { get; set; }
    }
}