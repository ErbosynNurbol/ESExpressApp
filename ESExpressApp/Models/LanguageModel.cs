using System;
using System.Collections.Generic;
using System.Text;

namespace ESExpressApp.Models
{
    public partial class LanguageModel
    {
        public string Culture { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string FlagUrl { get; set; }
        public byte IsDefault { get; set; }
        public byte IsRtl { get; set; }
    }
}
