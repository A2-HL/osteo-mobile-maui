namespace OsteoMAUIApp.Views.ProfileSettings;

public partial class ProfileIndex : ContentPage
{
	public ProfileIndex()
	{
		InitializeComponent();
	}

    private async void OnSelectImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select a Certificate Image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                UploadedImage.Source = ImageSource.FromStream(() => stream);
                UploadedImage.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to pick an image.", "OK");
        }
    }

    private async void OnSaveSettingsClicked(object sender, EventArgs e)
    {
        // Collect values from input fields
        var homeAddress = HomeAddressEntry.Text;
        var provincesWillingToTravel = GetSelectedProvinces(); // Function to get checked provinces
        var bio = BioEditor.Text;
        var osteopathicPractice = OsteopathicPracticeEntry.Text;
        var minLoad = MinPatientLoadEntry.Text;
        var maxLoad = MaxPatientLoadEntry.Text;
        var adultsLoad = AdultsEntry.Text;
        var kidsLoad = KidsEntry.Text;

        // Simple validation check
        if (string.IsNullOrWhiteSpace(homeAddress) || string.IsNullOrWhiteSpace(bio))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please fill all required fields.", "OK");
            return;
        }

        // Construct object for submission
        var profileData = new
        {
            HomeAddress = homeAddress,
            Provinces = provincesWillingToTravel,
            Bio = bio,
            OsteopathicPractice = osteopathicPractice,
            MinLoad = minLoad,
            MaxLoad = maxLoad,
            AdultsLoad = adultsLoad,
            KidsLoad = kidsLoad
        };

        // TODO: Send data to API or save locally
        await Application.Current.MainPage.DisplayAlert("Success", "Settings saved successfully!", "OK");
    }

    private List<string> GetSelectedProvinces()
    {
        var selectedProvinces = new List<string>();

        // Checking each province checkbox manually
        if (AlbertaCheckBox.IsChecked)
            selectedProvinces.Add("Alberta");

        if (BritishColumbiaCheckBox.IsChecked)
            selectedProvinces.Add("British Columbia");

        if (ManitobaCheckBox.IsChecked)
            selectedProvinces.Add("Manitoba");

        if (NewBrunswickCheckBox.IsChecked)
            selectedProvinces.Add("New Brunswick");

        if (NewfoundlandCheckBox.IsChecked)
            selectedProvinces.Add("Newfoundland and Labrador");

        if (NovaScotiaCheckBox.IsChecked)
            selectedProvinces.Add("Nova Scotia");

        if (OntarioCheckBox.IsChecked)
            selectedProvinces.Add("Ontario");

        if (PrinceEdwardIslandCheckBox.IsChecked)
            selectedProvinces.Add("Prince Edward Island");

        if (QuebecCheckBox.IsChecked)
            selectedProvinces.Add("Quebec");

        if (SaskatchewanCheckBox.IsChecked)
            selectedProvinces.Add("Saskatchewan");

        return selectedProvinces;
    }

    //// Function to collect selected provinces
    //private List<string> GetSelectedProvinces()
    //{
    //    var selectedProvinces = new List<string>();

    //    foreach (var checkbox in ProvincesCheckboxes)
    //    {
    //        if (checkbox.IsChecked)
    //        {
    //            selectedProvinces.Add(checkbox.Text);
    //        }
    //    }

    //    return selectedProvinces;
    //}

    private void ValidatePasswordForm(object sender, TextChangedEventArgs e)
    {
        bool isValid = true;

        // Check Current Password
        if (string.IsNullOrWhiteSpace(CurrentPasswordEntry.Text))
        {
            CurrentPasswordError.IsVisible = true;
            isValid = false;
        }
        else
        {
            CurrentPasswordError.IsVisible = false;
        }

        // Check New Password
        if (string.IsNullOrWhiteSpace(NewPasswordEntry.Text))
        {
            NewPasswordError.IsVisible = true;
            isValid = false;
        }
        else
        {
            NewPasswordError.IsVisible = false;
        }

        // Check Confirm Password
        if (string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text) || ConfirmPasswordEntry.Text != NewPasswordEntry.Text)
        {
            ConfirmPasswordError.IsVisible = true;
            isValid = false;
        }
        else
        {
            ConfirmPasswordError.IsVisible = false;
        }

        // Enable the button only if all fields are valid
        PasswordSubmitButton.IsEnabled = isValid;
    }
    private void OnSubmitPasswordChange(object sender, EventArgs e)
    {
        bool isValid = true;

        // Check Current Password
        if (string.IsNullOrWhiteSpace(CurrentPasswordEntry.Text))
        {
            CurrentPasswordError.IsVisible = true;
            isValid = false;
        }
        else
        {
            CurrentPasswordError.IsVisible = false;
        }

        // Check New Password
        if (string.IsNullOrWhiteSpace(NewPasswordEntry.Text))
        {
            NewPasswordError.IsVisible = true;
            isValid = false;
        }
        else
        {
            NewPasswordError.IsVisible = false;
        }

        // Check Confirm Password
        if (string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text) || ConfirmPasswordEntry.Text != NewPasswordEntry.Text)
        {
            ConfirmPasswordError.IsVisible = true;
            isValid = false;
        }
        else
        {
            ConfirmPasswordError.IsVisible = false;
        }

        // Proceed only if all fields are valid
        if (isValid)
        {
            DisplayAlert("Success", "Password changed successfully!", "OK");
        }
    }
}