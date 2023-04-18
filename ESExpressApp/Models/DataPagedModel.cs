using System;
using System.Collections.Generic;
using System.Text;

namespace ESExpressApp.Models
{
    public class DataPagedModel
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public string KeyWord { get; set; }
        public int Total { get; set; }
        public int TotalPage { get; set; }
        public object DataList { get; set; }
    }
}
