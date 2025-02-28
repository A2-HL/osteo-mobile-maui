using Newtonsoft.Json;
using OsteoMAUIApp.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Models.Event
{
   public class RescheduleModel: BaseViewModel
    {
        public string _patientType;
        public string patientType
        {
            get { return _patientType; }
            set
            {
                _patientType = value;
                SetProperty(ref _patientType, value);
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

        #region|event model validations|

        //User field validations for event
        public async Task<bool> ValidateModelForRescheudleEvent()
        {
            await Task.Run(() =>
            {
                ValidateSessionDay();
                ValidateFromTime();
                ValidateToTime();
                ValidateTreatmentLength();
            });
            if (string.IsNullOrEmpty(sessionDayError) || string.IsNullOrEmpty(fTimeError)
                || string.IsNullOrEmpty(tTimeError) || string.IsNullOrEmpty(treatmentLengthError))
            {
                return true;
            }
            return false;
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
            if (fTime == null)
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
            if (tTime == null)
            {
                tTimeError = "To time is required";
            }
            else
            {
                tTimeError = "";
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
        #region |Serialization for submission|

        public string SerializeRescheduleEventFields()
        {
            var data = new Dictionary<string, object>
            {
                { "patientType", patientType },
                { "sessionDay", sessionDay },
                { "fromDateStr", fromDateStr },
                { "toDateStr", toDateStr },
                { "fTime", fTime },
                { "tTime", tTime},
                { "isScheduleEnable", isScheduleEnable },
                { "treatmentLength", treatmentLength?.Minuts ?? 0}
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
        #endregion
        #endregion
    }
}
