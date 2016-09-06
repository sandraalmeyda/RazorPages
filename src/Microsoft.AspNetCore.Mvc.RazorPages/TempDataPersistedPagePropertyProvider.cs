using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.RazorPages
{
    public class TempDataPersistedPagePropertyProvider : IPersistedPagePropertyProvider
    {
        private static readonly string _prefix = "Page_";
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

                if (properties.Any(pi => !(pi.SetMethod.IsPublic && pi.GetMethod.IsPublic)))
                {
                    throw new InvalidOperationException("Persisted page properties must have a public getter and setter.");
                }

                return properties;
            });
        }
    }
}