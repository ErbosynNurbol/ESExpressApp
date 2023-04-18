using System;
namespace ESExpressApp.Models
{
    public class PersonWaybillModel
    {
        public uint Id { get; set; }
        public uint ParentId { get; set; }
        public string WaybillNumber { get; set; }
        public string ProductName { get; set; }
        public string ClientRemark { get; set; }
        public uint PackagingTypeId { get; set; }
        public uint ShippingMethodId { get; set; }
    }
}
