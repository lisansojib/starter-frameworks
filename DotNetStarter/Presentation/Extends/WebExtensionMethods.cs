using EPYSLACSCustomer.Core.Entities;
using Presentation.Models;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;

namespace EPYSLACSCustomer.Core.Statics
{
    public static class WebExtensionMethods
    {
        public static T ConvertToObject<T>(this NameValueCollection formData)
        {
            var objT = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties();
            foreach (var pro in properties)
            {
                if (formData.AllKeys.Any(x => x.Equals(pro.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    if (typeof(System.Data.Entity.EntityState).IsAssignableFrom(pro.PropertyType))
                    {
                        Enum.TryParse(formData.Get(pro.Name), out System.Data.Entity.EntityState entityState);
                        pro.SetValue(objT, entityState);
                    }
                    else
                    {
                        var value = string.IsNullOrEmpty(formData.Get(pro.Name)) ? null : Convert.ChangeType(formData.Get(pro.Name), pro.PropertyType);
                        pro.SetValue(objT, value);
                    }                    
                }
            }

            return objT;
        }

        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            string HttpContext = "MS_HttpContext";
            string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            return "";
        }



        public static bool IsEqual(this UKAndCELabellingChild obj1, UkAndCeLabellingChildBindingModel obj2)
        {
            return obj1.PackTypeId == obj2.PackTypeId
                && obj1.SeasonId == obj2.SeasonId
                && obj1.TPND == obj2.TPND
                && obj1.PONo == obj2.PONo
                && obj1.UKStyleRef == obj2.UKStyleRef
                && obj1.CEStyleRef == obj2.CEStyleRef
                && obj1.ShortDesc == obj2.ShortDesc
                && obj1.EQOSCode == obj2.EQOSCode
                && obj1.BarcodeNo == obj2.BarcodeNo
                && obj1.SupplierId == obj2.SupplierId
                && obj1.PackagingSupplierId == obj2.PackagingSupplierId
                && obj1.DeptId == obj2.DeptId
                && obj1.BrandId == obj2.BrandId
                && obj1.RfidCompliant == obj2.RfidCompliant
                && obj1.TagAtSource == obj2.TagAtSource;
        }
    }
}