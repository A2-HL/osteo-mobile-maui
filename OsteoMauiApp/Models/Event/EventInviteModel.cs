using Newtonsoft.Json;
using OsteoMAUIApp.Models.Common;
using OsteoMAUIApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Models.Event
{
   public class EventInviteModel: BaseViewModel
    {
        public string _inviteType = "2";
        public string inviteType
        {
            get => _inviteType;
            set
            {
                this._inviteType = value;
                SetProperty(ref _inviteType, value);
                OnInviteTypeChanged(_inviteType);
            }
        }
        public bool _isEmailVisible = true;
        public bool _isPhoneNumberVisible = false;
        public bool IsEmailVisible
        {
            get => _isEmailVisible;
            set
            {
                _isEmailVisible = value;
                OnPropertyChanged(nameof(IsEmailVisible));
            }
        }

        public bool IsPhoneNumberVisible
        {
            get => _isPhoneNumberVisible;
            set
            {
                _isPhoneNumberVisible = value;
                OnPropertyChanged(nameof(IsPhoneNumberVisible));
            }
        }

        public ObservableCollection<string> _emailOrPhones { get; set; } = new();
        public ObservableCollection<string> emailOrPhones
        {
            get => _emailOrPhones;
            set
            {
                _emailOrPhones = value;
                OnPropertyChanged(nameof(emailOrPhones));
            }
        }

        private DropdownListModel _event;
        public DropdownListModel eventInvite
        {
            get { return this._event; }
            set
            {
                if (this._event != value)
                {
                    SetProperty(ref _event, value);
                    ValidateEvent();
                }
            }
        }
        private string _eventGuidError;
        public string eventGuidError
        {
            get
            {
                return _eventGuidError;
            }

            set
            {
                if (_eventGuidError != value)
                {
                    SetProperty(ref _eventGuidError, value);
                }
            }
        }

        private string _emailOrPhone;
        public string emailOrPhone
        {
            get { return _emailOrPhone; }
            set
            {
                if (_emailOrPhone != value)
                {
                    SetProperty(ref _emailOrPhone, value);
                    ValidatePhoneNumberOrEmail();
                }
            }
        }
        private string _emailOrPhoneError;
        public string emailOrPhoneError
        {
            get
            {
                return _emailOrPhoneError;
            }

            set
            {
                if (_emailOrPhoneError != value)
                {
                    SetProperty(ref _emailOrPhoneError, value);
                }
            }
        }

        #region |Model Validations|
        public async Task<bool> ValidateModelForInvite()
        {
            await Task.Run(() =>
            {
                ValidatePhoneNumberOrEmail();
                ValidateEvent();

            });
            if (string.IsNullOrEmpty(emailOrPhoneError) && string.IsNullOrEmpty(eventGuidError))
            {
                return true;
            }
            return true;
        }

        private void ValidatePhoneNumberOrEmail()
        {
            emailOrPhoneError = string.IsNullOrEmpty(emailOrPhone) ? "This Field is required." : "";
        }
        private void ValidateEvent()
        {
            eventGuidError = eventInvite == null ? "Event is required." : "";
        }

        private void OnInviteTypeChanged(string selectedValue)
        {
            if (selectedValue == "2")
            {
                IsEmailVisible = true;
                IsPhoneNumberVisible = false;
                emailOrPhones = new();
            }

            else {
                IsPhoneNumberVisible = true;
                IsEmailVisible = false;
                emailOrPhones = new();
            }
        }

        #endregion

        #region |Serialization for submission|
        public string SerializeInviteFields()
        {
            var data = new Dictionary<string, object>
            {
                { "emailOrPhone", emailOrPhone },
                { "inviteType", inviteType },
                { "guid", eventInvite.Guid },
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
        #endregion
    }
}
