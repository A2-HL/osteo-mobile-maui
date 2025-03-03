using OsteoMAUIApp.ViewModels;
namespace OsteoMAUIApp.Views.Authentication;

public partial class Login : ContentPage
{
    AuthenticationVM _loginVM;
    public Login()
    {

        try
        {
            InitializeComponent();
        }
        catch (Exception e)
        {

        }
        _loginVM = new AuthenticationVM(Navigation);
        BindingContext = _loginVM;

    }

    private void OnFrameTappedPractitioner(object sender, EventArgs e)
    {
        PractitionerTextBox.IsVisible = true;
        PractitionerFrame.Stroke = Color.FromArgb("#00d9bc");
        PatientTextBox.IsVisible = false;
        //phoneinput.Text = "";
        PatientFrame.Stroke = Color.FromArgb("#00FFFFFF");
        _loginVM.SelectedUserType = 2;
    }
    private void OnFrameTappedPatient(object sender, EventArgs e)
    {
        PractitionerTextBox.IsVisible = false;
        //emailinput.Text = "";
        PractitionerFrame.Stroke = Color.FromArgb("#00FFFFFF");
        PatientTextBox.IsVisible = true;
        PatientFrame.Stroke = Color.FromArgb("#00d9bc");
        _loginVM.SelectedUserType = 1;
    }

    private async void OnSignupClicked(object sender, EventArgs e)
    {
        _loginVM.SignupCommand.Execute(null);
    }
    private void OnLoginClicked(object sender, EventArgs e)
    {
        _loginVM.LoginCommand.Execute(null);
    }
    private void OnForgotClicked(object sender, EventArgs e)
    {
        _loginVM.ForgotPasswordCommand.Execute(null);
    }

}