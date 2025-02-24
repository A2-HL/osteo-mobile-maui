using OsteoMAUIApp.ViewModels;
using Newtonsoft.Json;
using SQLite;
using System.Text.Json.Serialization;
using System.Xml;

namespace OsteoMAUIApp.Models.User
{
    public class UserModel : BaseViewModel
    {
        [PrimaryKey]
        public string id { get; set; }
        public string email { get; set; }
        private string _firstName;
        public string firstName
        {
            get { return this._firstName; }
            set
            {
                if (this._firstName != value)
                {
                    SetProperty(ref _firstName, value);
                    ValidateFirstName();
                }
            }
        }
        private string _firstNameError;
        [Ignore]
        public string firstNameError
        {
            get
            {
                return this._firstNameError;
            }

            set
            {
                if (this._firstNameError != value)
                {
                    SetProperty(ref _firstNameError, value);
                }
            }
        }
        private string _lastName;
        public string lastName
        {
            get { return this._lastName; }
            set
            {
                if (this._lastName != value)
                {
                    SetProperty(ref _lastName, value);
                    ValidateLastName();
                }
            }
        }
        private string _lastNameError;
        [Ignore]
        public string lastNameError
        {
            get
            {
                return this._lastNameError;
            }

            set
            {
                if (this._lastNameError != value)
                {
                    SetProperty(ref _lastNameError, value);
                }
            }
        }
        public string profilePicture { get; set; }
        public string stripeCustomerId { get; set; }
        public string defaultCardGuid { get; set; }
        private bool _isAnyPlanSubscribed;
        public bool isAnyPlanSubscribed
        {
            get { return _isAnyPlanSubscribed; }
            set
            {
                SetProperty(ref _isAnyPlanSubscribed, value);
            }
        }
        public bool isExternalLogin { get; set; }
        public string externalLoginType { get; set; }

        private bool _isUpdatingFromJson;
      
       
        

        #region|User model validations|

        //User field validations for edit profile name
        public async Task<bool> ValidateModelForEditProfile()
        {
            await Task.Run(() =>
            {
                ValidateFirstName();
                ValidateLastName();
            });
            if (string.IsNullOrEmpty(firstNameError) && string.IsNullOrEmpty(lastNameError))
            {
                return true;
            }
            return false;
        }
        private void ValidateFirstName()
        {
            if (string.IsNullOrEmpty(firstName))
            {
                firstNameError = "First name is required";
            }
            else
            {
                firstNameError = "";
            }
        }
        private void ValidateLastName()
        {
            if (string.IsNullOrEmpty(lastName))
            {
                lastNameError = "Last name is required";
            }
            else
            {
                lastNameError = "";
            }
        }

        #endregion

        

        [Ignore]
        public oauthTokenModel oauthToken { get; set; }


        private string _profilePictureBase64;
        [JsonProperty("profilePictureBase64")]
        public string profilePictureBase64
        {
            get => _profilePictureBase64;
            set
            {
                if (_profilePictureBase64 != value)
                {
                    SetProperty(ref _profilePictureBase64, value);
                    UpdateProfilePictureImageSource();
                }
            }
        }
        private ImageSource _profilePictureSource;
        [System.Text.Json.Serialization.JsonIgnore]
        [Ignore]
        public ImageSource profilePictureSource
        {
            get => _profilePictureSource;
            set
            {
                if (_profilePictureSource != value)
                {
                    SetProperty(ref _profilePictureSource, value);
                }
            }
        }

        #region |Profile Picture Conversions|        
        private void UpdateProfilePictureImageSource()
        {
            if (!string.IsNullOrEmpty(profilePictureBase64))
            {
                // Remove the data URL scheme prefix if present
                var base64Data = profilePictureBase64;
                if (base64Data.Contains(","))
                {
                    var dataParts = base64Data.Split(',');
                    if (dataParts.Length > 1)
                    {
                        base64Data = dataParts[1];
                    }
                }

                var imageBytes = Convert.FromBase64String(base64Data);
                profilePictureSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
            else
            {
                profilePictureSource = null;
            }
        }
        #endregion

        [Ignore]
        public string fullName
        {
            get
            {
                var fname = firstName + " " + lastName;
                return fname;
            }
        }

        public class oauthTokenModel
        {
            public accessTokenModel access { get; set; }
            public refreshTokenModel refresh { get; set; }

        }

        public class accessTokenModel
        {
            public string token { get; set; }
            public DateTime expires { get; set; }
            public string type { get; set; }

        }

        public class refreshTokenModel
        {
            public string token { get; set; }
            public DateTime expires { get; set; }

        }
    }
}
