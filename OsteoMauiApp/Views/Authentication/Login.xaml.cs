using OsteoMAUIApp.ViewModels.Authentication;

namespace OsteoMAUIApp.Views.Authentication;

public partial class Login : ContentPage
{
    LoginVM _loginVM;
    public Login()
    {

        try
        {
            InitializeComponent();
        }
        catch (Exception e)
        {

        }

        _loginVM = new LoginVM(Navigation);
        BindingContext = _loginVM;

        Device.BeginInvokeOnMainThread(() => { _loginVM.InitializeCommand.Execute(null); });
    }
    private async void OnSignupClicked(object sender, EventArgs e)
    {
        // Navigate to the SignUp.xaml page
        await Navigation.PushAsync(new SignUpStepOne());
    }
    private void OnLoginClicked(object sender, EventArgs e)
    {
        _loginVM.LoginCommand.Execute(null);
    }
}