using System;
using System.Collections.Generic;
using System.Text;

namespace ESExpressApp.Models
{
    public class WaybillStatusItem
    {
        public string Title { get; set; }
        public string Key { get; set; }
        public string Icon { get; set; }
        public string Font { get; set; }
        public bool HasCount { get;set; }
        public int Count { get; set; }
    }
}
