using Newtonsoft.Json;
using OsteoMAUIApp.Models.Common;
using OsteoMAUIApp.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Models.Event
{
   public class EventModel: BaseViewModel 
    {

        public string _title;
        public string title {
            get => _title;
            set {
                if (this._title != value)
                {
                    SetProperty(ref _title, value);
                    ValidateTitle();
                }
            }
        }

        private string _titleError;
        [Ignore]
        public string titleError
        {
            get => _titleError;

            set
            {
                if (this._titleError != value)
                {
                    SetProperty(ref _titleError, value);
                }
            }
        }

        public string _sessionDay;
        public string sessionDay
        {
            get => _sessionDay;
            set
            {
                if (this._sessionDay != value)
                {
                    SetProperty(ref _sessionDay, value);
                    ValidateSessionDay();
                }
            }
        }
        private string _sessionDayError;
        [Ignore]
        public string sessionDayError
        {
            get => _sessionDayError;

            set
            {
                if (this._sessionDayError != value)
                {
                    SetProperty(ref _sessionDayError, value);
                }
            }
        }

        public string _location;
        public string location {
            get => _location;
            set {
                if (this._location != value)
                {
                    SetProperty(ref _location, value);
                    ValidateLocation();
                }
            }
        }
        private string _locationError;
        [Ignore]
        public string locationError
        {
            get => _locationError;
            set
            {
                if (this._locationError != value)
                {
                    SetProperty(ref _locationError, value);
                }
            }
        }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string fromDateStr { get; set; }
        public string toDateStr { get; set; }
        public bool _isScheduleEnable;
        public bool isScheduleEnable
        {
            get => _isScheduleEnable;
            set
            {
                if (this._isScheduleEnable != value)
                {
                    SetProperty(ref _isScheduleEnable, value);
                }
            }
        }

        public TreatmentLengthModel _treatmentLength;
        public TreatmentLengthModel treatmentLength
        {
            get => _treatmentLength;
            set
            {
                if (this._treatmentLength != value)
                {
                    SetProperty(ref _treatmentLength, value);
                    ValidateTreatmentLength();
                }
            }
        }
        private string _treatmentLengthError;
        [Ignore]
        public string treatmentLengthError
        {
            get => _treatmentLengthError;

            set
            {
                if (this._treatmentLengthError != value)
                {
                    SetProperty(ref _treatmentLengthError, value);
                }
            }
        }
        public string _fTime;
        public string fTime
        {
            get => _fTime;
            set
            {
                if (this._fTime != value)
                {
                    SetProperty(ref _fTime, value);
                    ValidateFromTime();
                }
            }
        }
        private string _fTimeError;
        [Ignore]
        public string fTimeError
        {
            get => _fTimeError;

            set
            {
                if (this._fTimeError != value)
                {
                    SetProperty(ref _fTimeError, value);
                }
            }
        }

        public string _tTime;
        public string tTime
        {
            get => _tTime;
            set
            {
                if (this._tTime != value)
                {
                    SetProperty(ref _tTime, value);
                    ValidateToTime();
                }
            }
        }
        private string _tTimeError;
        [Ignore]
        public string tTimeError
        {
            get => _tTimeError;

            set
            {
                if (this._tTimeError != value)
                {
                    SetProperty(ref _tTimeError, value);
                }
            }
        }

        public ReminderOptions _reminder;
        public ReminderOptions reminder
        {
            get => _reminder;
            set
            {
                if (this._reminder != value)
                {
                    SetProperty(ref _reminder, value);
                    ValidateReminder();
                }
            }
        }
        private string _reminderError;
        [Ignore]
        public string reminderError
        {
            get => _reminderError;

            set
            {
                if (this._reminderError != value)
                {
                    SetProperty(ref _reminderError, value);
                }
            }
        }

        public string _privacy = "1";
        public string privacy
        {
            get => _privacy;
            set
            {
                this._privacy = value;
                SetProperty(ref _privacy, value);
            }
        }

        public string _patientType= "1";
        public string patientType
        {
            get => _patientType;
            set
            {
                _patientType = value;
                SetProperty(ref _patientType, value);
            }
        }
        public int adults { get; set; }
        public int kids { get; set; }

        private List<DropdownListModel> _userGroupIds=new();

        public List<DropdownListModel> userGroupIds
        {
            get => _userGroupIds;
            set {
                _userGroupIds = value;
                SetProperty(ref _userGroupIds, value);
            }
        }

        private ObservableCollection<DropdownListModel> _patientIds = new();

        public ObservableCollection<DropdownListModel> patientIds
        {
            get => _patientIds;
            set => SetProperty(ref _patientIds, value);
        }

        private List<DropdownListModel> _practitionerIds;

        public List<DropdownListModel> practitionerIds
        {
            get => _practitionerIds;
            set => SetProperty(ref _practitionerIds, value);
        }

        #region|event model validations|

        //User field validations for event
        public async Task<bool> ValidateModelForAddEvent()
        {
            await Task.Run(() =>
            {
                ValidateTitle();
                ValidateLocation();
                ValidateSessionDay();
                ValidateFromTime();
                ValidateToTime();
                ValidateReminder();
                ValidateTreatmentLength();
            });
            if (string.IsNullOrEmpty(titleError) || string.IsNullOrEmpty(locationError)
                || string.IsNullOrEmpty(sessionDayError) || string.IsNullOrEmpty(fTimeError)
                || string.IsNullOrEmpty(tTimeError) || string.IsNullOrEmpty(reminderError)
                || string.IsNullOrEmpty(treatmentLengthError))
            {
                return true;
            }
            return false;
        }
        private void ValidateTitle()
        {
            if (string.IsNullOrEmpty(title))
            {
                titleError = "Title is required";
            }
            else
            {
                titleError = "";
            }
        }
        private void ValidateLocation()
        {
            if (string.IsNullOrEmpty(location))
            {
                locationError = "Location is required";
            }
            else
            {
                locationError = "";
            }
        }
        private void ValidateSessionDay()
        {
            if (string.IsNullOrEmpty(sessionDay))
            {
                sessionDayError = "Date is required";
            }
            else
            {
                sessionDayError = "";
            }
        }
        private void ValidateFromTime()
        {
            if (string.IsNullOrEmpty(fTime))
            {
                fTimeError = "From time is required";
            }
            else
            {
                fTimeError = "";
            }
        }
        private void ValidateToTime()
        {
            if (string.IsNullOrEmpty(tTime))
            {
                tTimeError = "To time is required";
            }
            else
            {
                tTimeError = "";
            }
        }
        private void ValidateReminder()
        {
            if (reminder == null)
            {
                reminderError = "Reminder is required";
            }
            else
            {
                reminderError = "";
            }
        }
        private void ValidateTreatmentLength()
        {
            if (treatmentLength == null)
            {
                treatmentLengthError = "Appointment length is required";
            }
            else
            {
                treatmentLengthError = "";
            }
        }
        #endregion
        #region |Serialization for submission|

        public string SerializeCreateEventFields()
        {
            var data = new Dictionary<string, object>
            {
                { "title", title },
                { "patientType", Convert.ToInt32(patientType) },
                { "sessionDay", sessionDay },
                { "fromDateStr", fromDateStr },
                { "toDateStr", toDateStr },
                { "fTime", fTime },
                { "tTime", tTime},
                { "isScheduleEnable", isScheduleEnable },
                { "treatmentLength", treatmentLength?.Minuts ?? 0},
                { "privacy", Convert.ToInt32(privacy) },
                { "location", "Gujranwala, punjab, pakistan" },
                { "latitude", 31.029933 },
                { "longitude", 71.930011 },
                { "practitionerIds",practitionerIds?.Select(x => x.Id).ToArray() ?? Array.Empty<int>()},
                { "adults", adults },
                { "kids", kids },
                { "userGroupIds", userGroupIds?.Select(x => x.Id).ToArray() ?? Array.Empty<int>() },
                { "patientIds", patientIds?.Select(x => x.Id).ToArray() ?? Array.Empty<int>() },
                { "emailInvite", false },
                { "reminder", reminder?.Value ?? 0 },
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
        #endregion
    }

    public class TreatmentLengthModel
    {
        public int Minuts { get; set; }
        public string Text { get; set; }
    }
    public class ReminderOptions
    {
        public string Text { get; set; }
        public double Value { get; set; }
    }
}
