using System;

namespace Microsoft.AspNetCore.Mvc.RazorPages
{
    public interface IPersistedPagePropertyProvider
    {
        void LoadPagePropertyState(Page page);

        void SavePagePropertyState(Page page);   
    }
}
