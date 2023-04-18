using System;
using System.Collections.Generic;

namespace ESExpressApp.Models
{
    public class PersonIncomingPackageModel
    {
        public PersonWaybillModel PersonWaybill { get; set; }
        public List<PackagingTypeModel> PackagingTypeList { get; set; }
        public List<ShippingMethodModel> ShippingMethodList { get; set; }
    }
}
