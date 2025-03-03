using OsteoMAUIApp.ViewModels;

namespace OsteoMAUIApp.Views.Authentication;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SignUp : ContentPage
{
    AuthenticationVM _authenticationVM;
    public SignUp()
    {
        try
        {
            InitializeComponent();
            _authenticationVM = new AuthenticationVM(Navigation);
            BindingContext = _authenticationVM;
        }
        catch (Exception e)
        {

        }
    }

    private void OnSignupSubmitClicked(object sender, EventArgs e)
    {
        _authenticationVM.SignupSubmitCommand.Execute(null);
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        _authenticationVM.LoginCommand.Execute(null);
    }
    private void OnFrameTappedPractitioner(object sender, EventArgs e)
    {
        //PractitionerTextBox.IsVisible = true;
        PractitionerFrame.Stroke = Color.FromArgb("#00d9bc");
       // PatientTextBox.IsVisible = false;

        PatientFrame.Stroke = Color.FromArgb("#00FFFFFF");
        _authenticationVM.SelectedUserType = 2;
    }
    private void OnFrameTappedPatient(object sender, EventArgs e)
    {
        //PractitionerTextBox.IsVisible = false;
        PractitionerFrame.Stroke = Color.FromArgb("#00FFFFFF");
        //PatientTextBox.IsVisible = true;
        PatientFrame.Stroke = Color.FromArgb("#00d9bc");
        _authenticationVM.SelectedUserType = 1;
    }
}