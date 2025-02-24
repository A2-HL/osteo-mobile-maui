namespace OsteoMAUIApp.Views.Authentication;

public partial class SignUpStepOne : ContentPage
{
	public SignUpStepOne()
	{
		InitializeComponent();
	}
    private async void OnLoginTapped(object sender, EventArgs e)
    {
        // Navigate to the SignUp.xaml page
        await Navigation.PushAsync(new Login());
    }
    private async void OnSignupClicked(object sender, EventArgs e)
    {
        // Navigate to the SignUp.xaml page
        await Navigation.PushAsync(new VerifyAccount());
    }
}