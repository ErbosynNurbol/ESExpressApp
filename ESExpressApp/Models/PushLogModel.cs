using System;
namespace ESExpressApp.Models
{
    public class PushLogModel
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public string Data { get; set; }
        public string PushTime { get; set; }
    }
}

