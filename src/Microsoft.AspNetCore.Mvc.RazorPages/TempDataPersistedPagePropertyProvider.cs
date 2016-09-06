using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.RazorPages
{
    public class TempDataPersistedPagePropertyProvider : IPersistedPagePropertyProvider
    {
        private static readonly string _prefix = "PersistedProperty-";
        private ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> _pageProperties =
            new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        public PersistedPagePropertyTracker LoadAndTrackChanges(Page page)
        {
            return new PersistedPagePropertyTracker(page, LoadPagePropertyState(page), SavePropertyValue);
        }

        public IDictionary<PropertyInfo, object> LoadPagePropertyState(Page page)
        {
            var pageProperties = GetPageProperties(page);
            var result = new Dictionary<PropertyInfo, object>();

            foreach (var property in pageProperties)
            {
                var value = page.TempData[_prefix + property.Name];

                result[property] = value;

                // TODO: Clarify what behavior should be for null values here
                if (value != null && property.PropertyType.IsAssignableFrom(value.GetType()))
                {
                    property.SetValue(page, value);
                }
            }

            return result;
        }

        private void SavePropertyValue(Page page, PropertyInfo property, object value)
        {
            if (value != null)
            {
                page.TempData[_prefix + property.Name] = value;
            }
        }

        private IEnumerable<PropertyInfo> GetPageProperties(Page page)
        {
            return _pageProperties.GetOrAdd(page.GetType(), pageType =>
            {
                var properties = pageType.GetRuntimeProperties()
                    .Where(pi => pi.GetCustomAttribute<TempDataAttribute>() != null);

                if (properties.Any(pi => !(pi.SetMethod != null && pi.SetMethod.IsPublic && pi.GetMethod != null && pi.GetMethod.IsPublic)))
                {
                    throw new InvalidOperationException("Persisted page properties must have a public getter and setter.");
                }

                if (properties.Any(pi => !(pi.PropertyType.GetTypeInfo().IsPrimitive || pi.PropertyType == typeof(string))))
                {
                    throw new InvalidOperationException("Persisted page properties must be declared as primitive types or string only.");
                }

                return properties;
            });
        }
    }
}