
namespace Microsoft.AspNetCore.Mvc.RazorPages
{
    public interface IPersistedPagePropertyProvider
    {
        PersistedPagePropertyTracker LoadAndTrackChanges(Page page);
    }
}
