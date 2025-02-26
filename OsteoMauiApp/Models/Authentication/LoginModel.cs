using Newtonsoft.Json;
using OsteoMAUIApp.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace OsteoMAUIApp.Models.Authentication
{
    public class LoginModel : BaseViewModel
    {
       

        

   
        #region email
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

        #endregion

        #region phonenumber

        private string _phoneNumber;
        public string phoneNumber
        {
            get { return this._phoneNumber; }
            set
            {
                if (this._phoneNumber != value)
                {
                    SetProperty(ref _phoneNumber, value);
                    ValidatePhoneNumber();
                }
            }
        }

        private string _phoneNumberError;
        public string phoneNumberError
        {
            get
            {
                return this._phoneNumberError;
            }

            set
            {
                if (this._phoneNumberError != value)
                {
                    SetProperty(ref _phoneNumberError, value);
                }
            }
        }

        #endregion

        #region currentPassword
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
        #endregion

        #region userType

        private int _userTypeId;
        public int userTypeId
        {
            get { return this._userTypeId; }
            set
            {
                if (this._userTypeId != value)
                {
                    SetProperty(ref _userTypeId, value);
                }
            }
        }
        #endregion
    
        #region |Login Model Validations|
        public async Task<bool> ValidateModelForLogin()



        {
            await Task.Run(() =>
            {
                ValidateEmail();
                ValidatePhoneNumber();
                ValidateCurrentPassword();

            });
            if (string.IsNullOrEmpty(emailAddressError) && string.IsNullOrEmpty(currentPasswordError) && _userTypeId == 2)
            {
                return true;
            }
            else if (string.IsNullOrEmpty(phoneNumberError) && string.IsNullOrEmpty(currentPasswordError) && _userTypeId == 1)
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
        private void ValidatePhoneNumber()
        {
            phoneNumberError = string.IsNullOrEmpty(phoneNumber) ? "Phone number is required." : "";
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
                { "phoneNumber", phoneNumber },
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

        public string fcmToken { get; set; }
    }
}
