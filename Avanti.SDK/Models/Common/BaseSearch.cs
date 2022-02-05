using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avanti.SDK.Models.Common
{
    public class BaseSearch
    {
        public string ToQueryString()
        {
            StringBuilder queryString = new StringBuilder();

            List<PropertyInfo> properties = GetType()
                .GetProperties()
                .ToList();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(this, null);
                string name = property.Name;

                if (value != null)
                {
                    queryString.Append(
                        $"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(value.ToString())}");
                }
            }

            return queryString.ToString();
        }
    }
}
