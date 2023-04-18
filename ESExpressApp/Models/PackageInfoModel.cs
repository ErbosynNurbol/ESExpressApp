using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ESExpressApp.Models
{
    public class PackageInfoModel
    {
        public uint Id { get; set; }
        public uint ParentId { get; set; }
        public uint IntransitOrderId { get; set; }
        public uint DeliveredOrderId { get; set; }
        public uint PackagingTypeId { get; set; }
        public uint ShippingMethodId { get; set; }
        public string PackagingType { get; set; }
        public string ShippingMethod { get; set; }
        public string ShippingImage { get; set; }
        public string WaybillNumber { get; set; }
        public string ProductName { get; set; }
        public string ClientRemark { get; set; }
        public string AdminRemark { get; set; }
        public uint IncomingTime { get; set; }
        public uint AddTime { get; set; }
        public uint ReceivingTime { get; set; }
        public uint DeliveredTime { get; set; }
        public List<string> PackagePictureList { get; set; }
        public List<string> ReceiptPictureList { get; set; }
        public List<PackageInfoModel> ChildList { get; set; }
        public byte PackingStatus { get; set; }
    }
}
