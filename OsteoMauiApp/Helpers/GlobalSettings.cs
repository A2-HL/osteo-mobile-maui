using OsteoMAUIApp.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Helpers
{
    public class GlobalSettings
    {
        public string APIsBaseUrl { get { return BaseUrl + "api/"; } } // APIs base url
        public string APPsBaseUrl { get { return BaseWebUrl; } } // APIs base url

        private string _baseUrl = null;
        public string BaseUrl
        {
            get
            {
                if (_baseUrl == null)
                {
                    var preference = Preferences.Get(nameof(BaseUrl), null);
                    //_baseUrl = preference == null ? StageBaseUrl : preference;
                    _baseUrl = StageBaseUrl;
                }
                return _baseUrl;
            }
            set
            {
                Preferences.Set(nameof(BaseUrl), value);
                _baseUrl = value;
            }
        }
        private string _baseWebUrl = null;
        public string BaseWebUrl
        {
            get
            {
                if (_baseWebUrl == null)
                {
                    var preference = Preferences.Get(nameof(BaseWebUrl), null);
                    //_baseWebUrl = preference == null ? StageWebBaseUrl : preference;
                    _baseWebUrl = StageWebBaseUrl;
                }
                return _baseWebUrl;
            }
            set
            {
                Preferences.Set(nameof(BaseWebUrl), value);
                _baseWebUrl = value;
            }
        }

        //Production URLS
        //public const string ProdBaseUrl = "https://api.tapgrap.com/";
        //public const string ProdWebBaseUrl = "app.tapgrap.com/";

        //Staging URLS
        public const string StageBaseUrl = "https://stagingapi.tapgrap.com/";
        public const string StageWebBaseUrl = "stagingapi.tapgrap.com/";


        //OAuth Credentials for Android & IOS Specefic
        public const string GoogleOAuthClientId_Android = "272600636733-bcu4u2mq9o0cdc5qgancb39i8573tgab.apps.googleusercontent.com";
        public const string GoogleOAuthClientId_Ios = "272600636733-bcu4u2mq9o0cdc5qgancb39i8573tgab.apps.googleusercontent.com";
        public static string GoogleOAuthRedirectUri => "com.alphasquared.tapgrap:/oauth2redirect";
        public static string GoogleOAuthAuthorizationEndpoint => "https://accounts.google.com/o/oauth2/auth";
        public static string GoogleOAuthTokenEndpoint => "https://oauth2.googleapis.com/token";
        public static string GoogleOAuthUserInfoEndpoint => "https://www.googleapis.com/oauth2/v3/userinfo";

        //Menu URLS
        public static string AndroidPlayStoreURL => "https://play.google.com/store/apps/details?id=com.alphaSquared.tapgrap.app&hl=en";
        public static string IOSStoreURL => "https://apps.apple.com/app/App_ID";
        public static string TermsOfServiceURL => "https://www.tapgrap.com/terms-conditions";
        public static string PrivacyPolicyURL => "https://www.tapgrap.com/privacy-policy";
        public static string SocialFacebookURL => "https://www.facebook.com/tapgrap.connect";
        public static string SocialInstagramURL => "https://www.instagram.com/tapgrap.connect/";


        //Static Keys        
        public static int MaxCardLimit = 10;
        public static string DisplayUTCDateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        public static string ReceivedFieldDateFormat = "MMM dd, yyyy";
        public static string DisplayShortDateFormat = "MMM dd, yyyy";
        public static string DisplayLongDateFormat = "MMMM dd, yyyy";
        public static string Display24HourTimeFormat = "HH:mm:ss";
        public static string Display12HourTimeFormat = "hh:mm tt";
        public static string LoginDateTimeKey = "Login_DateTime";
        public static string FcmTokenKey = "FCM_Token";
        public static string AccessTokenKey = "Access_Token";
        public static string RefreshTokenKey = "Refresh_Token";
        public static string GoogleSignInSuccessfulKey = "Google_Signin_Successful";
        public static string UserKey = "User_Info";
        public static string NoInternetMessage = "Oops! It looks like you're offline. Check your internet connection.";
        public static string UnauthorizedMessage = "You are unauthorized to perform this action. Please login again.";
        public static string FailedtoProcessMessage = "We are unable to process your request at the moment. Please try again later.";
        public static string InactiveCardMessage = "This card is currently inactive and cannot be edited. Additionally, inactive cards cannot be scanned or shared. To activate this card and access all features, please subscribe to the Pro plan.";
        public static string FailedtoSignupMessage = "We are unable to sign up a new account for you. Please try again later.";
        public static string UnabletoLogoutMessage = "We are unable to logout at the moment. Please try again later.";
        public static string FirstCardMustBeCreatedMessage = "You must create your first card to proceed.";
        public static string GrantPermissionMessage = "Device access permissions required.";
        public static string EmptyInviteEmailListMessage = "You must add atleast 1 email in the list to send the invite.";
        public static string InvalidTapGrapBarcodeMessage = "Invalid TapGrap QR Code.";
        public static string CannotScanOwnCardMessage = "You can not scan your own card.";
        public static string CardAlreadyScannedMessage = "Card already scanned.";
        public static string LinkCopiedMessage = "Link copied";
        public static string QRSavedMessage = "QR code saved on device";
        public static string ReachedMaxCardLimitMessage = $"You have reached your maximum card limit ({MaxCardLimit}). Delete an existing card to create new card.";
        public static string AwaitingSyncInternetRequiredMessage = "Awaiting sync. Internet connection required.";
        public static string WhyAllowLocationAccessMessage = "Allowing location access helps TapGrap to record the location of your QR scans. This feature makes it easier for you to recognize where each QR code was scanned, adding valuable context to your records. Do you want to enable it in settings?";
        public static string WhyAllowCameraAccessToScanMessage = "TapGrap requires camera access to scan QR codes. Without camera access the app can not scan QR codes. Do you want to enable it in settings?";
        public static string WhyAllowNotificationAccessMessage = "TapGrap requires notification access to send you updates and personalized alerts. Do you want to enable it in settings?";
        public static string LocationServicesDisabledMessage = "Unable to fetch your location. Please ensure location services are enabled in your device settings.";
        public static string RemoveUserPhotoConfirmation = "About to remove photo from your profile. This action is irreversible, continue ?";
        public static string RemoveCardPhotoConfirmation = "About to remove photo from your card. This action is irreversible, continue ?";
        public static string RemoveCardLogoConfirmation = "About to remove logo from your card. This action is irreversible, continue ?";
        public static string RemoveQRLogoConfirmation = "About to remove custom logo from QR code. This action is irreversible, continue ?";
        public static string DeleteAccountConfirmation = "About to permanently delete your account from TapGrap. This will delete all data associated with your account and this action is irreversible, continue ?";
        public static string UnsubscribePlanConfirmation = "About to Unsubscribe professional plan. Until {0}, professional plan features will still be available to you and it will not auto renew, continue ?";
        public static string ShareText = "Let's connect! View my digital business card and save my contact details instantly. #TapGrap #GoGreen #AmplifyYourNetwork";
        public static string ShareTitle = "TapGrap - Amplify Your Network";
        public static string ShareSubject = "Let's connect! View my digital business card and save my contact details instantly.";
        public static string ForceUpdateMessage = "A new version of TapGrap is available. Please update to continue.";
        public static string OptionalUpdateMessage = "A new version of TapGrap is available. Would you like to update?";
        public static string NoProfilePictureMessage = "You do not have any profile photo associated with card. Tap upload and set a photo to make your card look more appealing.";
        public static string UploadedProfilePictureMessage = "Tap upload to change your card profile photo OR remove it from your card.";
        public static string NoLogoMessage = "You do not have any logo associated with card. Tap upload and set a logo to make your card look more appealing.";
        public static string UploadedLogoMessage = "Tap upload to change your card logo OR remove it from your card.";
        public string access_token { get; set; }
        public string refresh_token { get; set; }

        public UserModel user { get; set; }

        public static GlobalSettings Instance { get; } = new GlobalSettings();




        //Error Codes
        //200 or 201 When the request is succeeded
        //401 when the request is unauthorized
        //402 when there is any validation error
        //404 when the request resource is not found
        //503 when internet is not available
        //504 when upgrade plan is required
        //900 Exception
    }
}
