using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleDXGui.Domains
{
    public class IMAccountConfig
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string OrgCode { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Được sinh ra từ cấu hình
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// [TNTK] IMess|IMess
        /// [TNFF] Bám đuổi khách hàng : 0968668368|0972218687|0988915267|0963794411|0918839995|0903409798|0904861368
        /// </summary>
        public string FriendsIMess { get; set; }
        public List<IMAccountConfigItem> Items { get; set; }
    }

    public class IMAccountConfigItem
    {
        public string Id { get; set; }
        /// <summary>
        /// [TNTK] Sale_1|Telegram
        /// </summary>
        public string Account { get; set; }
        public Channel Channel { get; set; }
        /// <summary>
        /// [TNFF] Bám đuổi khách hàng : +84904866668|+84904861368|+84842489429|+84904861368|+84842489429
        /// </summary>
        public string Groups { get; set; }
        /// <summary>
        /// [TNGF] Bám đuổi hội nhóm : IkI4i4ZD_mw0Njg1|uxAqvDM6zHo4OTNl
        /// </summary>
        public string Friends { get; set; }
    }
}