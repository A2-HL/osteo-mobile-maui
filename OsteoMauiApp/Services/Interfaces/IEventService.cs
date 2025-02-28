using OsteoMAUIApp.Models;
using OsteoMAUIApp.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Services.Interfaces
{
    public interface IEventService
    {
        Task<ResponseStatusModel> CreateAsync(EventModel model);
        Task<ResponseStatusModel> RescheduleEventAsync(RescheduleModel model);
    }
}
