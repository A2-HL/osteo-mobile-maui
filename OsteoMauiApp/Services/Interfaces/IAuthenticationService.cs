using OsteoMAUIApp.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponseModel> LoginAsync(LoginModel credentials);
       // Task<ResponseStatusModel> SignupAsync(SignupModel signupModel);
       // Task<ResponseStatusModel> ForgotPasswordAsync(LoginModel credentials);
        Task<LoginResponseModel> RefreshAccessToken(string refreshToken, string userId);
        Task<bool> LogoutAsync();
        //Task<ResponseStatusModel> ResendVerificationEmail(string emailAddress);
    }
}
