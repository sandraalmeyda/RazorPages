using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Microsoft.AspNetCore.Mvc.RazorPages
{
    public class TempDataPropertyTracker
    {
        private readonly object _subject;
        private readonly Action<object, ITempDataDictionary, PropertyInfo, object> _saveProperty;
        private readonly IDictionary<PropertyInfo, object> _trackedProperties = new Dictionary<PropertyInfo, object>();
        private readonly ITempDataDictionary _tempData;

        public TempDataPropertyTracker(object subject, ITempDataDictionary tempData, IDictionary<PropertyInfo, object> trackedProperties, Action<object, ITempDataDictionary, PropertyInfo, object> saveProperty)
        {
            _subject = subject;
            _tempData = tempData;
            _trackedProperties = trackedProperties;
            _saveProperty = saveProperty;
        }

        public void SaveChanges()
        {
            foreach (var property in _trackedProperties)
            {
                var newValue = property.Key.GetValue(_subject);
                if (newValue != null && newValue != property.Value)
                {
                    _saveProperty(_subject, _tempData, property.Key, newValue);
                }
            }
        }
    }
}