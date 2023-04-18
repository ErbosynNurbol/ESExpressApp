using System;
using System.Collections.Generic;
using System.Text;

namespace ESExpressApp.Models
{
    public partial class AdvertiseModel
    {
        public int Id { get; set; }
        public string MediaUrl { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Url { get; set; }
    }
}
