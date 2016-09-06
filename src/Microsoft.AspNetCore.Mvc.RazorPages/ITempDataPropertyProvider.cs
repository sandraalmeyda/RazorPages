
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Microsoft.AspNetCore.Mvc.RazorPages
{
    public interface ITempDataPropertyProvider
    {
        TempDataPropertyTracker LoadAndTrackChanges(object subject, ITempDataDictionary tempData);
    }
}
