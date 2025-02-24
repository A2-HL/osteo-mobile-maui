using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Services.Interfaces
{
    public interface IAppSettingsService
    {
        Task InitializeAppSettings();
        Task UpdateMenuRefreshFlag(bool flag);
        Task UpdateCardRefreshFlag(bool flag);
        Task UpdateAppRefreshFlags(bool flag);
        Task<bool> GetMenuRefreshFlag();
        Task<bool> GetCardRefreshFlag();

    }
}
