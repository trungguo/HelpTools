using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleDXGui.Models
{
    public class CustomerSearchModel
    {
        public List<string> Labels { get; set; }
        public List<string> Channels { get; set; }
        public string Type { get; set; }
        public DateTime? DOBFrom { get; set; }
        public DateTime? DOBTo { get; set; }
    }
}