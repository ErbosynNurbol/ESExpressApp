using System;
using System.Collections.Generic;
using System.Text;

namespace ESExpressApp.Models
{
    public class WaybillInfoModel
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Week { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public string Icon { get; set; }
        public string Font { get; set; }
        public string TextColor { get; set; }
        public int FontSize { get; set; }
        public bool ShowLine { get; set; }
    }
}
