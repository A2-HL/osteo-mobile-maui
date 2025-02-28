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

    }

    private void OnFrameTappedPractitioner(object sender, EventArgs e)
    {
        PractitionerTextBox.IsVisible = true;
        PractitionerFrame.Stroke = Color.FromArgb("#00d9bc");
        PatientTextBox.IsVisible = false;
        phoneinput.Text = "";
        PatientFrame.Stroke = Color.FromArgb("#00FFFFFF");
        _loginVM.SelectedUserType = 2;
    }
    private void OnFrameTappedPatient(object sender, EventArgs e)
    {
        PractitionerTextBox.IsVisible = false;
        emailinput.Text = "";
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
   

}