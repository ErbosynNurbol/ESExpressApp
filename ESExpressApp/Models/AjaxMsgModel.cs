using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESExpressApp.Models
{
    public class AjaxMsgModel
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public string BackUrl { get; set; }
        public object Data { get; set; }
    }
}
