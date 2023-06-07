using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SaleDXGui.Domains
{
    public class Customer
    {
        public string Id { get; set; }
        public string OrgCode { get; set; }
        [Description("Mã")]
        public string Code { get; set; }
        /// <summary>
        /// Nhóm|Cá nhân
        /// </summary>
        [Description("Loại")]
        public string Type { get; set; }
        [Description("Tên")]
        public string Name { get; set; }
        [Description("SĐT")]
        public string Phone { get; set; }
        [Description("Email")]
        public string Email { get; set; }
        [Description("Ngày sinh")]
        public DateTime? DOB { get; set; }
        public int? DOBMonth { get; set; }
        public int? DOBDay { get; set; }
        [Description("Giới tính")]
        public string Sex { get; set; }

        public List<ContactPoint> ContactPoints { get; set; }
        [Description("Điểm chạm")]
        public string ContactPointString { get; set; }
        [Description("Ghi chú")]
        public string Note { get; set; }
        public List<string> Labels { get; set; }
        [BsonIgnore]
        [Description("Nhãn")]
        public string LabelsString { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class ContactPoint
    {
        public string Id { get; set; }
        public Channel Channel { get; set; }
        public string UID { get; set; }
    }
}