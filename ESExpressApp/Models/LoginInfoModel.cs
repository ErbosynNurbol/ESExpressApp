using System;
using System.Collections.Generic;
using System.Text;

namespace ESExpressApp.Models
{
    public class LoginInfoModel
    {
        public string QarToken { get; set; }
        public int UID { get; set; }
        public PersonModel UserInfo { get; set; }
    }
}
