using OsteoMAUIApp.ViewModels.Authentication;
namespace OsteoMAUIApp.Views.Authentication;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SignUp : ContentPage
{
    SignUpVM _signupVM;
    public SignUp()
    {
        try
        {
           // InitializeComponent();
            _signupVM = new SignUpVM(Navigation);
            BindingContext = _signupVM;
        }
        catch (Exception e)
        {

        }
    }
   
    private void OnSignupSubmitClicked(object sender, EventArgs e)
    {
        _signupVM.SignupSubmitCommand.Execute(null);
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        _signupVM.LoginCommand.Execute(null);
    }
}