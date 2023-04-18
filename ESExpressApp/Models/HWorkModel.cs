using System;
using System.Collections.Generic;
using System.Text;

namespace ESExpressApp.Models
{
    public partial class HWorkModel
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Title { get; set; }
        public object IconUnicode { get; set; }
        public string IconFontFamily { get; set; }
    }
}
