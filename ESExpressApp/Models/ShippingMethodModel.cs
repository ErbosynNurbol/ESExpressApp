using System;
namespace ESExpressApp.Models
{
    public class ShippingMethodModel
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public string WarehouseAddress { get; set; }
        public string ThumbnailUrl { get; set; }
        public uint DisplayOrder { get; set; }
    }
}
