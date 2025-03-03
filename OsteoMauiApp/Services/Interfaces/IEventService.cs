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
        #region Event
        Task<ResponseStatusModel> CreateAsync(EventModel model);
        Task<ResponseStatusModel> RescheduleEventAsync(RescheduleModel model);
        Task<ResponseStatusModel> EventInviteAsync(EventInviteModel model);
        Task<EventListResponseModel> LoadMyEvents(EventRequestModel model);
        Task<EventListResponseModel> LoadUpcomingNearbyEvents(EventRequestModel model);
        Task<EventResponseModel> EventDetail(string guid);
        Task<ParticepantResponseModel> LoadParticepants(EventSlotRequestModel model);
        #endregion

        #region Appointments
        Task<EventSlotResponseModel> LoadUpcommingEventSlots(EventSlotRequestModel model);
        #endregion
    }
}
