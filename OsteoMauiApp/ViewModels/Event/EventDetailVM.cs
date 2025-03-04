using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using OsteoMAUIApp.Helpers;
using OsteoMAUIApp.Models.Event;
using OsteoMAUIApp.Services.Implementations;
using OsteoMAUIApp.Services.Interfaces;
using Syncfusion.Maui.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OsteoMAUIApp.ViewModels.Event
{
   public class EventDetailVM: BaseViewModel
    {
        private readonly IEventService _eventService;
        public EventDetailVM(INavigation navigation)
        {
            requestModel = new EventRequestModel();
            slotrequestModel = new EventSlotRequestModel();
            Detail = new EventDetailModel();
            Reschedule = new RescheduleModel();
            _eventService = new EventService(new RequestProvider());
            _navigation = navigation;
            LoadMyEvents = new Command(async () => await LoadEvents());
            LoadAllUpcommingEvents = new Command(async () => await LoadAllUpcommingNearEvents());
            LoadDetail = new Command(async () => await LoadEventDetail());
            LoadUpcommingEventSlots = new Command(async () => await LoadUpcommingSlots());
            LoadParticepants = new Command(async () => await LoadParticepantList());
            RescheduleCommand = new Command(RescheduleEventClicked);
            LoadResheduleDetail = new Command(async () => await RescheduleDetail());
            TreatmentLengthOptions = new List<TreatmentLengthModel>
            {
                new TreatmentLengthModel { Minuts = 15, Text = "15 Minutes" },
                new TreatmentLengthModel { Minuts = 30, Text = "30 Minutes" }
            };
        }
        #region |Private|
        INavigation _navigation;
        RescheduleModel _rescheduleModel;
        public ObservableCollection<EventDetailModel> Events { get; set; } = new ObservableCollection<EventDetailModel>();
        #endregion
        #region |Commands|
        public Command LoadMyEvents;
        public Command LoadAllUpcommingEvents;
        public Command LoadDetail;
        public Command LoadUpcommingEventSlots;
        public Command LoadParticepants;
        public Command RescheduleCommand;
        public Command LoadResheduleDetail;
        #endregion
        public RescheduleModel Reschedule
        {
            get { return this._rescheduleModel; }
            set
            {
                if (this._rescheduleModel == value)
                {
                    return;
                }

                SetProperty(ref _rescheduleModel, value);
            }
        }
        public List<EventDetailModel> _upcommingEvents;
        public List<EventDetailModel> UpcommingEvents
        {
            get => _upcommingEvents;
            set
            {
                if (this._upcommingEvents == value)
                {
                    return;
                }

                SetProperty(ref _upcommingEvents, value);
            }
        }

        public List<EventDetailModel> _completedEvents;
        public List<EventDetailModel> CompletedEvents
        {
            get => _completedEvents;
            set
            {
                if (this._completedEvents == value)
                {
                    return;
                }

                SetProperty(ref _completedEvents, value);
            }
        }
        public List<EventDetailModel> _cancelledEvents;
        public List<EventDetailModel> CancelledEvents
        {
            get => _cancelledEvents;
            set
            {
                if (this._cancelledEvents == value)
                {
                    return;
                }

                SetProperty(ref _cancelledEvents, value);
            }
        }

        public EventDetailModel _detail;
        public EventDetailModel Detail
        {
            get => _detail;
            set
            {
                if (this._detail == value)
                {
                    return;
                }

                SetProperty(ref _detail, value);
            }
        }

        public List<EventSlotModel> _upcommingSlots;
        public List<EventSlotModel> upcommingSlots
        {
            get => _upcommingSlots;
            set
            {
                if (this._upcommingSlots == value)
                {
                    return;
                }

                SetProperty(ref _upcommingSlots, value);
            }
        }
        public List<ParticepantModel> _particepants;
        public List<ParticepantModel> particepants
        {
            get => _particepants;
            set
            {
                if (this._particepants == value)
                {
                    return;
                }

                SetProperty(ref _particepants, value);
            }
        }
        public List<TreatmentLengthModel> TreatmentLengthOptions { get; set; }
        #region |Events|
        public EventRequestModel _requestModel;
        public EventRequestModel requestModel
        {
            get => _requestModel;
            set
            {
                if (this._requestModel == value)
                {
                    return;
                }

                SetProperty(ref _requestModel, value);
            }
        }
        public string RescheduleDateFormate { get; set; }
        public string _guid;
        public string guid
        {
            get => _guid;
            set
            {
                if (this._guid == value)
                {
                    return;
                }

                SetProperty(ref _guid, value);
            }
        }
        private async Task LoadEvents()
        {
            if (IsBusy) return;
            StartBusyIndicator();
            await Task.Delay(500);
            try
            {
                var res = await _eventService.LoadMyEvents(requestModel);
                if (!string.IsNullOrEmpty(res.Status) && res.Status =="success")
                {
                    UpcommingEvents = res.UpcomingHostedEvents.Select(MapToEventDetailModel).ToList();
                    CancelledEvents = res.CancelledEvents.Select(MapToEventDetailModel).ToList();
                    CompletedEvents = res.CompletedEvents.Select(MapToEventDetailModel).ToList();
                }
                else
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoSignupMessage, "OK");
                }
            }
            catch (System.Exception ex)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
            }
            StopBusyIndicator();
        }

        private async Task LoadAllUpcommingNearEvents()
        {
            if (IsBusy) return;
            StartBusyIndicator();
            await Task.Delay(500);
            try
            {
                var res = await _eventService.LoadUpcomingNearbyEvents(requestModel);
                if (!string.IsNullOrEmpty(res.Status) && res.Status == "success")
                {
                    UpcommingEvents = res.Data.AllEvents.Select(MapToEventDetailModel).ToList();
                }
                else
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoSignupMessage, "OK");
                }
            }
            catch (System.Exception ex)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
            }
            StopBusyIndicator();
        }
        public async Task LoadEventDetail()
        {
            //if (IsBusy) return;
            //StartBusyIndicator();
            //await Task.Delay(500);
            try
            {
                var res = await _eventService.EventDetail(guid);
                if (!string.IsNullOrEmpty(res.Status) && res.Status == "success")
                {
                    Detail = res.Data;
                 }
                else
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoSignupMessage, "OK");
                }
            }
            catch (System.Exception ex)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
            }
            //StopBusyIndicator();
        }

        private async void RescheduleEventClicked(object obj)
        {
            if (IsBusy) return;

            if (await Reschedule.ValidateModelForRescheudleEvent())
            {
                Reschedule.guid = guid;
                StartBusyIndicator();
                await Task.Delay(500);
                try
                {
                    if (!string.IsNullOrEmpty(RescheduleDateFormate) && RescheduleDateFormate.Contains(" - "))
                    {
                        var splitdDate = RescheduleDateFormate.Split(" - ");
                        Reschedule.fromDateStr = splitdDate[0];
                        Reschedule.toDateStr = splitdDate[1];
                    }
                    if(!string.IsNullOrEmpty(RescheduleDateFormate) && !RescheduleDateFormate.Contains(" - "))
                    {
                        Reschedule.fromDateStr = RescheduleDateFormate;
                        Reschedule.toDateStr = RescheduleDateFormate;
                    }
                    var res = await _eventService.RescheduleEventAsync(Reschedule);
                    if (res != null)
                    {
                        if (res.status == "success")
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Success", res.statusMessage, "OK");
                            await _navigation.PopAsync();
                        }
                        else if (res.status == "error")
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Error", res.statusMessage, "OK");
                        }
                        else
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
                        }
                    }
                    else
                    {
                        await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoSignupMessage, "OK");
                    }
                }
                catch (System.Exception ex)
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
                }
            }
            StopBusyIndicator();
        }
        public async Task RescheduleDetail()
        {
            if (IsBusy) return;
            StartBusyIndicator();
            await Task.Delay(500);
            try
            {
                var res = await _eventService.RescheduleDetail(guid);
                if (!string.IsNullOrEmpty(res.Status) && res.Status == "success")
                {
                    //var data = (RescheduleModel)res.Data;
                    //data.IsVisibleAll = true;
                    Reschedule = res.Data;
                }
                else
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoSignupMessage, "OK");
                }
            }
            catch (System.Exception ex)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
            }
            StopBusyIndicator();
        }
        #endregion

        #region Appointments
        public EventSlotRequestModel _slotrequestModel;
        public EventSlotRequestModel slotrequestModel
        {
            get => _slotrequestModel;
            set
            {
                if (this._slotrequestModel == value)
                {
                    return;
                }

                SetProperty(ref _slotrequestModel, value);
            }
        }
        private async Task LoadUpcommingSlots()
        {
            //if (IsBusy) return;
            //StartBusyIndicator();
            await Task.Delay(500);
            try
            {
                slotrequestModel.guid = guid;
                var res = await _eventService.LoadUpcommingEventSlots(slotrequestModel);
                if (!string.IsNullOrEmpty(res.Status) && res.Status == "success")
                {
                    upcommingSlots = res.Data.Select(x=>new EventSlotModel { 
                    
                        Datestr = x.Date.ToString("MM-dd-yyyy"),
                        Time=$"{x.FTime} - {x.TTime}",
                        AppointTypeStr=x.AppointTypeStr,
                        ParticipateInfo = x.ParticipateInfo.Replace("<br>", "\n"),
                        ColorTitle=x.ColorTitle,
                        StatusType  = x.ColorTitle == "Blocked" ? BadgeType.Error : x.ColorTitle == "Confirmed" ? BadgeType.Success : x.ColorTitle == "Booked" ? BadgeType.Primary : x.ColorTitle == "Available" ? BadgeType.Secondary : x.ColorTitle == "Completed" ? BadgeType.Success : x.ColorTitle == "Cancelled" ? BadgeType.Info : BadgeType.Secondary,
                    }).ToList();
                }
                else
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoSignupMessage, "OK");
                }
            }
            catch (System.Exception ex)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
            }
            //StopBusyIndicator();
        }
        #endregion

        #region Particepants
        private async Task LoadParticepantList()
        {
            //if (IsBusy) return;
            //StartBusyIndicator();
            await Task.Delay(500);
            try
            {
                slotrequestModel.guid = guid;
                var res = await _eventService.LoadParticepants(slotrequestModel);
                if (!string.IsNullOrEmpty(res.Status) && res.Status == "success")
                {
                    particepants = res.Data;
                }
                else
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoSignupMessage, "OK");
                }
            }
            catch (System.Exception ex)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
            }
            //StopBusyIndicator();
        }
        #endregion

        #region |Helpers|
        private EventDetailModel MapToEventDetailModel(EventDetailModel x)
        {
            return new EventDetailModel
            {
                Guid = x.Guid,
                Title = x.Title,
                SessionDay = x.SessionDay,
                Time = $"{x.FromTime} {x.ToTime} daily",
                Location = x.Location,
                DistanceCalucate = x.DistanceCalucate,
                TotalPatients = x.EventParticipants?.Count(d => d.UserTypeId == 2 && d.StatusId == 2) ?? 0
            };
        }
        #endregion
    }
}
