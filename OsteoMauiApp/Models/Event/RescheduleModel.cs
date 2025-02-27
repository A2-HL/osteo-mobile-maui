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
        public int _patientType = 1;
        public int patientType
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
            get { return this._sessionDay; }
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
            get
            {
                return this._sessionDayError;
            }

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
            get { return this._isScheduleEnable; }
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
            get { return this._treatmentLength; }
            set
            {
                if (this._treatmentLength != value)
                {
                    SetProperty(ref _treatmentLength, value);
                }
            }
        }

        public TimeSpan? _fTime;
        public TimeSpan? fTime
        {
            get { return this._fTime; }
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
            get
            {
                return this._fTimeError;
            }

            set
            {
                if (this._fTimeError != value)
                {
                    SetProperty(ref _fTimeError, value);
                }
            }
        }

        public TimeSpan? _tTime;
        public TimeSpan? tTime
        {
            get { return this._tTime; }
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
            get
            {
                return this._tTimeError;
            }

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
            });
            if (string.IsNullOrEmpty(sessionDayError) || string.IsNullOrEmpty(fTimeError)
                || string.IsNullOrEmpty(tTimeError))
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
        #endregion
    }
}
