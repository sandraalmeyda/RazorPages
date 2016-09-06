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

        public void LoadPagePropertyState(Page page)
        {
            var pageProperties = GetPageProperties(page);

            foreach (var property in pageProperties)
            {
                var value = page.TempData[_prefix + property.Name];
                if (value != null && property.PropertyType.IsAssignableFrom(value.GetType()))
                {
                    property.SetValue(page, value);
                }
            }
        }

        public void SavePagePropertyState(Page page)
        {
            var pageProperties = GetPageProperties(page);

            foreach (var property in pageProperties)
            {
                page.TempData[_prefix + property.Name] = property.GetValue(page);
            }
        }

        private IEnumerable<PropertyInfo> GetPageProperties(Page page)
        {
            return _pageProperties.GetOrAdd(page.GetType(), pageType =>
                pageType.GetRuntimeProperties()
                    .Where(pi => pi.GetCustomAttribute<TempDataAttribute>() != null));
        }
    }
}