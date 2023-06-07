using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Textpad.Models
{
    public class NoteFile
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string FileContent { get; set; }
    }
}