using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.RazorPages
{
    public class PersistedPagePropertyTracker
    {
        private readonly Page _page;
        private readonly Action<Page, PropertyInfo, object> _saveProperty;
        private readonly IDictionary<PropertyInfo, object> _trackedProperties = new Dictionary<PropertyInfo, object>();

        public PersistedPagePropertyTracker(Page page, IDictionary<PropertyInfo, object> trackedProperties, Action<Page, PropertyInfo, object> saveProperty)
        {
            _page = page;
            _trackedProperties = trackedProperties;
            _saveProperty = saveProperty;
        }

        public void SaveChanges()
        {
            foreach (var property in _trackedProperties)
            {
                var newValue = property.Key.GetValue(_page);
                if (newValue != null && newValue != property.Value)
                {
                    _saveProperty(_page, property.Key, newValue);
                }
            }
        }
    }
}