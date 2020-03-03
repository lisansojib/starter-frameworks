using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;

namespace ApplicationCore.Statics
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
    }
}