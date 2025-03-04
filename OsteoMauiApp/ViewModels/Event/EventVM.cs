using OsteoMAUIApp.Helpers;
using OsteoMAUIApp.Models.Common;
using OsteoMAUIApp.Models.Event;
using OsteoMAUIApp.Services.Implementations;
using OsteoMAUIApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.ViewModels.Event
{
    public class EventVM: BaseViewModel
    {
        private readonly IEventService _eventService;
        public EventVM(INavigation navigation)
        {
            _navigation = navigation;
            _eventService = new EventService(new RequestProvider());
            Event = new EventModel();
            EventInvite = new EventInviteModel();
            EventList = new List<DropdownListModel>();
            TreatmentLengthOptions = new List<TreatmentLengthModel>
            {
                new TreatmentLengthModel { Minuts = 15, Text = "15 Minutes" },
                new TreatmentLengthModel { Minuts = 30, Text = "30 Minutes" }
            };
            PatientGroupList = new List<DropdownListModel>();
            this.ReminderOptions = new List<ReminderOptions>();
            this.ReminderOptions.Add(new ReminderOptions { Text = "12 hours", Value = 720.00 });
            this.ReminderOptions.Add(new ReminderOptions { Text = "24 hours", Value = 1440.00 });
            this.ReminderOptions.Add(new ReminderOptions { Text = "36 hours", Value = 2160.00 });
            this.ReminderOptions.Add(new ReminderOptions { Text = "48 hours", Value = 2880.00 });
            this.ReminderOptions.Add(new ReminderOptions { Text = "60 hours", Value = 3600.00 });
            //Event.treatmentLength = TreatmentLengthOptions.FirstOrDefault();
            CreateCommand = new Command(CreateEventClicked);
            EventInviteCommand = new Command(EventInviteClicked);
            LoadEventsDll = new Command(LoadEventsDrodpwnList);
            LoadUserGroupDll = new Command(LoadUserGroupDrodpwnList);
        }
        #region |Private|
        INavigation _navigation;
        EventModel _event;
        EventInviteModel _eventInviteModel;
        public string SessionDateFormate { get; set; }
        #endregion

        #region |Public|
        public EventModel Event
        {
            get { return this._event; }
            set
            {
                if (this._event == value)
                {
                    return;
                }

                SetProperty(ref _event, value);
            }
        }

        public EventInviteModel EventInvite
        {
            get { return this._eventInviteModel; }
            set
            {
                if (this._eventInviteModel == value)
                {
                    return;
                }
                SetProperty(ref _eventInviteModel, value);
            }
        }

        private string _searchPtaint;
        public string SearchPatient
        {
            get => _searchPtaint;
            set
            {
                if (_searchPtaint != value)
                {
                    _searchPtaint = value;
                    OnPropertyChanged();
                    if (_searchPtaint.Length >= 3)
                        FetchFilteredData(_searchPtaint,2);
                }
            }
        }
        private string _searchPractitioner;
        public string SearchPractitioner
        {
            get => _searchPractitioner;
            set
            {
                if (_searchPractitioner != value)
                {
                    _searchPractitioner = value;
                    OnPropertyChanged();
                    if (_searchPractitioner.Length >= 3)
                        FetchFilteredData(_searchPractitioner, 1);
                }
            }
        }
        private async Task FetchFilteredData(string searchText,int type)
        {
            try
            {
                var UserList = PatientGroupList.Where(x => x.Name.Trim().ToLower().Contains(searchText.Trim().ToLower())).ToList();
                
                if (type == 2)
                {
                    PatientList = UserList;
                    OnPropertyChanged(nameof(PatientList));
                }
                else {
                    PractitionerList = UserList;
                    OnPropertyChanged(nameof(PractitionerList));
                }
  
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error fetching data: " + ex.Message);
            }
        }
        public List<TreatmentLengthModel> TreatmentLengthOptions { get; set; }
        public List<DropdownListModel> PatientList { get; set; }
        public List<DropdownListModel> PractitionerList { get; set; }
        public List<ReminderOptions> ReminderOptions { get; set; }
        public List<DropdownListModel> _PatientGroupList;
        public List<DropdownListModel> PatientGroupList
        {
            get => _PatientGroupList;
            set
            {
                if (this._PatientGroupList == value)
                {
                    return;
                }
                SetProperty(ref _PatientGroupList, value);
            }
        }

        public List<DropdownListModel> _EventList;
        public List<DropdownListModel> EventList
        {
            get => _EventList;
            set
            {
                if (this._EventList == value)
                {
                    return;
                }
                SetProperty(ref _EventList, value);
            }
        }
        #endregion

        #region |Commands|
        public Command CreateCommand;
        public Command EventInviteCommand;
        public Command AddEmailOrPhonesToListCommand;
        public Command LoadEventsDll;
        public Command LoadUserGroupDll;
        #endregion

        #region |Methods|
        private async void CreateEventClicked(object obj)
        {
            if (IsBusy) return;

            if (await Event.ValidateModelForAddEvent())
            {
                StartBusyIndicator();
                await Task.Delay(500);
                try
                {
                    if (!string.IsNullOrEmpty(SessionDateFormate) && SessionDateFormate.Contains(" - "))
                    {
                        var splitdDate = SessionDateFormate.Split(" - ");
                        Event.fromDateStr = splitdDate[0];
                        Event.toDateStr = splitdDate[1];
                    }
                    else {
                        Event.fromDateStr = SessionDateFormate;
                        Event.toDateStr = SessionDateFormate;
                    }
                    var res = await _eventService.CreateAsync(Event);
                    if (res != null)
                    {
                        if (res.status == "success")
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Success", res.statusMessage, "OK");
                            Event = new EventModel();
                            OnPropertyChanged(nameof(Event));
                            Application.Current.MainPage = new AppShell();
                            await Shell.Current.GoToAsync("//MyEvents");
                        }
                        else if (res.status =="error")
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

        private async void EventInviteClicked(object obj)
        {
            if (IsBusy) return;

            if (await EventInvite.ValidateModelForInvite())
            {
                StartBusyIndicator();
                await Task.Delay(500);
                try
                {
                    var res = await _eventService.EventInviteAsync(EventInvite);
                    if (res != null)
                    {
                        if (res.status == "success")
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Success", res.statusMessage, "OK");
                            EventInvite.emailOrPhones = new ObservableCollection<string>();
                            EventInvite.eventInvite = new DropdownListModel();
                            EventInvite.emailOrPhone = string.Empty;
                            EventInvite.eventGuidError = string.Empty;
                            EventInvite.emailOrPhoneError = string.Empty;
                        }
                        else if (res.status =="error")
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

        private async void LoadEventsDrodpwnList(object ojc)
        {
            try
            {
                EventList = await _eventService.EventDropdownList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async void LoadUserGroupDrodpwnList(object ojc)
        {
            try
            {
                PatientGroupList = await _eventService.UserGroupDropdownList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
