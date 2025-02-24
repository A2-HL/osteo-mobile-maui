using Newtonsoft.Json;
using OsteoMAUIApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Models.Authentication
{
    public class LoginModel : BaseViewModel
    {
        private string _emailAddress;
        public string emailAddress
        {
            get { return this._emailAddress; }
            set
            {
                if (this._emailAddress != value)
                {
                    SetProperty(ref _emailAddress, value);
                    ValidateEmail();
                }
            }
        }
        private string _emailAddressError;
        public string emailAddressError
        {
            get
            {
                return this._emailAddressError;
            }

            set
            {
                if (this._emailAddressError != value)
                {
                    SetProperty(ref _emailAddressError, value);
                }
            }
        }

        private string _currentPassword;
        public string currentPassword
        {
            get { return this._currentPassword; }
            set
            {
                if (this._currentPassword != value)
                {
                    SetProperty(ref _currentPassword, value);
                    ValidateCurrentPassword();
                }
            }
        }
        private string _currentPasswordError;

        private int _userTypeId;
        public int userTypeId
        {
            get { return this._userTypeId; }
            set
            {
                if (this._userTypeId != value)
                {
                    SetProperty(ref _userTypeId, value);
                    ValidateUserTypeId();
                }
            }
        }
        private string _userTypeIdError;
        public string currentPasswordError
        {
            get
            {
                return this._currentPasswordError;
            }

            set
            {
                if (this._currentPasswordError != value)
                {
                    SetProperty(ref _currentPasswordError, value);
                }
            }
        }
        public string fcmToken { get; set; }

        #region |Login Model Validations|
        public async Task<bool> ValidateModelForLogin()
        
        
        
        {
            await Task.Run(() =>
            {
                ValidateEmail();
                ValidateCurrentPassword();
                ValidateUserTypeId();
            });
            if (string.IsNullOrEmpty(emailAddressError) && string.IsNullOrEmpty(currentPasswordError)
                && string.IsNullOrEmpty(_userTypeIdError))
            {
                return true;
            }
            return true;
        }

        public async Task<bool> ValidateModelForForgotPassword()
        {
            await Task.Run(() =>
            {
                ValidateEmail();
            });
            if (string.IsNullOrEmpty(emailAddressError))
            {
                return true;
            }
            return false;
        }

        private void ValidateCurrentPassword()
        {
            currentPasswordError = string.IsNullOrEmpty(currentPassword) ? "Password is required." : "";

        }
        private void ValidateUserTypeId()
        {
            _userTypeIdError = userTypeId <= 0 ? "User type is required." : "";

        }
        private void ValidateEmail()
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                emailAddressError = "Email is required.";
            }
            else if (!IsValidEmail(emailAddress))
            {
                emailAddressError = "Invalid email format.";
            }
            else
            {
                emailAddressError = "";
            }
        }
        bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        #endregion

        #region |Serialization for submission|
        //Serialize fields for login
        public string SerializeLoginFields()
        {
            var data = new Dictionary<string, object>
            {
                { "email", emailAddress },
                { "password", currentPassword },
                 { "userTypeId", userTypeId },
                { "deviceToken", fcmToken }
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
        //Serialize fields for forgot password
        public string SerializeForgotPasswordFields()
        {
            var data = new Dictionary<string, object>
            {
                { "email", emailAddress }
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
        #endregion
    }
}
