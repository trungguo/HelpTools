using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleDXGui.Domains
{
    public class FBAccountConfig
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string OrgCode { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Được sinh ra từ cấu hình
        /// </summary>
        public string Content { get; set; }
        public List<FBAccountConfigItem> Items { get; set; }
    }

    public class FBAccountConfigItem
    {
        public string Id { get; set; }
        /// <summary>
        /// [FBTK] Sale_2
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// [FBGF] GroupFolow: 675559427316180|274016851104855|426295692056349
        /// </summary>
        public string Groups { get; set; }
        /// <summary>
        /// [FBTR] Reels Album: Reel_04
        /// </summary>
        public Reel Reel { get; set; }
    }

    public class Reel
    {
        public string AlbumId { get; set; }
        /// <summary>
        /// Reel_04
        /// </summary>
        public string Album { get; set; }
        /// <summary>
        /// V1|0 Khi nào quản lý được quyền đuổi nhân viên! #doxuantung #salesbanhang #salesthucchien #diSale #f1sale #foryou
        /// V2|1 Uy thế của một người làm Sales! #doxuantung #salesbanhang #salesthucchien #diSale #f1sale #foryou
        /// V3|2 Vì sao gọi là Nghệ thuật bán hàng đường phố #doxuantung #salesbanhang #salesthucchien #diSale #f1sale #foryou#voiceeffects
        /// V4|3 Điều kiện làm Giám đốc bán hàng ở tập đoàn P&G(phần 2) #doxuantung #salesbanhang #salesthucchien #diSale #f1sale #foryou
        /// V5|4 Điều kiện làm Giám đốc bán hàng ở tập đoàn P&G(phần 1) #doxuantung #salesbanhang #salesthucchien #diSale #f1sale #foryou
        /// </summary>
        public List<string> VideoDescriptions { get; set; }
    }
}