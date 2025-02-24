using OsteoMAUIApp.Models.Common;
using OsteoMAUIApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Services.Implementations
{
    public class AppSettingsService : IAppSettingsService
    {
        private readonly IRequestProvider request;
        private readonly IDatabaseService databaseService;
        public AppSettingsService(IRequestProvider _requestProvider, IDatabaseService _databaseService)
        {
            request = _requestProvider;
            databaseService = _databaseService;
        }

        public async Task InitializeAppSettings()
        {
            try
            {
                await databaseService.InsertAppSettingsAsync(new AppSettingsModel());
            }
            catch (Exception ex)
            {

            }
        }

        public async Task UpdateMenuRefreshFlag(bool flag)
        {
            var settings = await databaseService.GetAppSettingsAsync();
            if (settings != null)
            {
                settings.menuRefreshRequired = flag;
                await databaseService.UpdateAppSettingsAsync(settings);
            }
        }

        public async Task UpdateCardRefreshFlag(bool flag)
        {
            var settings = await databaseService.GetAppSettingsAsync();
            if (settings != null)
            {
                settings.cardRefreshRequired = flag;
                await databaseService.UpdateAppSettingsAsync(settings);
            }
        }

        public async Task UpdateAppRefreshFlags(bool flag)
        {
            var settings = await databaseService.GetAppSettingsAsync();
            if (settings != null)
            {
                settings.menuRefreshRequired = flag;
                settings.cardRefreshRequired = flag;
                await databaseService.UpdateAppSettingsAsync(settings);
            }
        }

        public async Task<bool> GetMenuRefreshFlag()
        {
            var settings = await databaseService.GetAppSettingsAsync();
            if (settings != null)
            {
                return settings.menuRefreshRequired;
            }
            return false;
        }

        public async Task<bool> GetCardRefreshFlag()
        {
            var settings = await databaseService.GetAppSettingsAsync();
            if (settings != null)
            {
                return settings.cardRefreshRequired;
            }
            return false;
        }
    }
}
