
using Newtonsoft.Json;
using OsteoMAUIApp.ViewModels;
using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;


namespace OsteoMAUIApp.Models.Authentication
{
    public class SignUpModel : BaseViewModel, INotifyPropertyChanged
    {
        // Fields and properties for each input
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phoneNumber;
        private string _homeAddress;
        private string _password;
        private string _confirmPassword;

        // Error messages for each input
        private string _firstNameError;
        private string _lastNameError;
        private string _emailError;
        private string _phoneNumberError;
        private string _homeAddressError;
        private string _passwordError;
        private string _confirmPasswordError;

        // Properties for inputs
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public string HomeAddress
        {
            get => _homeAddress;
            set => SetProperty(ref _homeAddress, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        // Properties for error messages
        public string FirstNameError
        {
            get => _firstNameError;
            set => SetProperty(ref _firstNameError, value);
        }

        public string LastNameError
        {
            get => _lastNameError;
            set => SetProperty(ref _lastNameError, value);
        }

        public string EmailError
        {
            get => _emailError;
            set => SetProperty(ref _emailError, value);
        }

        public string PhoneNumberError
        {
            get => _phoneNumberError;
            set => SetProperty(ref _phoneNumberError, value);
        }

        public string HomeAddressError
        {
            get => _homeAddressError;
            set => SetProperty(ref _homeAddressError, value);
        }

        public string PasswordError
        {
            get => _passwordError;
            set => SetProperty(ref _passwordError, value);
        }

        public string ConfirmPasswordError
        {
            get => _confirmPasswordError;
            set => SetProperty(ref _confirmPasswordError, value);
        }

        // Validation logic
        public async Task<bool> Validate()
        {
            bool isValid = true;

            // Reset all error messages
            FirstNameError = string.IsNullOrWhiteSpace(FirstName) ? "First name is required" : "";
            LastNameError = string.IsNullOrWhiteSpace(LastName) ? "Last name is required" : "";
            EmailError = string.IsNullOrWhiteSpace(Email) ? "Email is required" : !IsValidEmail(Email) ? "Invalid email format" : "";
            PhoneNumberError = string.IsNullOrWhiteSpace(PhoneNumber) ? "Phone number is required" : "";
            HomeAddressError = string.IsNullOrWhiteSpace(HomeAddress) ? "Home address is required" : "";
            PasswordError = string.IsNullOrWhiteSpace(Password) ? "Password is required" : "";
            ConfirmPasswordError = string.IsNullOrWhiteSpace(ConfirmPassword) ? "Confirm password is required" : "";

            // Check if passwords match
            if (!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword) && Password != ConfirmPassword)
            {
                PasswordError = "Passwords do not match";
                ConfirmPasswordError = "Passwords do not match";
                isValid = false;
            }

            // Check if any error messages are set
            if (!string.IsNullOrEmpty(FirstNameError) || !string.IsNullOrEmpty(LastNameError) ||
                !string.IsNullOrEmpty(EmailError) || !string.IsNullOrEmpty(PhoneNumberError) ||
                !string.IsNullOrEmpty(HomeAddressError) || !string.IsNullOrEmpty(PasswordError) ||
                !string.IsNullOrEmpty(ConfirmPasswordError))
            {
                isValid = false;
            }

            return isValid;
        }

        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        #region |Serialization for submission|
       
        public string SerializeSignupFields()
        {
            var data = new Dictionary<string, object>
            {
                { "email", Email },
                { "password", Password },
                { "confirmPassword", ConfirmPassword }
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
        #endregion
    }

}