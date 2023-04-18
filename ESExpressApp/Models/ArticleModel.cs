using System;
namespace ESExpressApp.Models
{
	public class ArticleModel
	{
        public uint Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string CategoryName { get; set; }
        public string CategoryUrl { get; set; }
        public uint CategoryId { get; set; }
        public string AddTimeMMddyyyyHHmm { get; set; }
        public uint ViewCount { get; set; }
        public uint IsFocusNews { get; set; }
        public bool HasVideo { get; set; }
        public bool HasAudio { get; set; }
        public bool HasImage { get; set; }
        public string FocusAdditionalFile { get; set; }
    }
}

