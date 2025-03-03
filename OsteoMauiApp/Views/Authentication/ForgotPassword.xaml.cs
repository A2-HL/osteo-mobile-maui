using OsteoMAUIApp.ViewModels;

namespace OsteoMAUIApp.Views.Authentication;

public partial class ForgotPassword : ContentPage
{
    AuthenticationVM _authenticationVM;

    public ForgotPassword()
    {
        InitializeComponent();
        _authenticationVM = new AuthenticationVM(Navigation);
        BindingContext = _authenticationVM;
    }
    private void OnCancelClicked(object sender, EventArgs e)
    {
        CloseDrawer();
    }

    private void OnForgotSubmitClicked(object sender, EventArgs e)
    {
        _authenticationVM.ForgotPasswordSubmitCommand.Execute(null);
    }

    #region |Drawer Swipe|
    private bool _isDrawerOpen = true;
    private double _startY;
    private double _startTranslationY;
    private const double SwipeThreshold = 50;
    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _startY = e.TotalY;
                _startTranslationY = DrawerFrame.TranslationY;
                break;

            case GestureStatus.Running:
                double offset = e.TotalY - _startY;
                double newY = Math.Max(0, DrawerFrame.TranslationY + offset);
                DrawerFrame.TranslationY = newY;
                _startTranslationY = e.TotalY;
                break;
            case GestureStatus.Completed:
                double distance = DrawerFrame.TranslationY - _startTranslationY;
                if (distance > SwipeThreshold && _startTranslationY > 0)
                {
                    CloseDrawer();
                }
                else
                {
                    if (_isDrawerOpen)
                    {
                        OpenDrawer();
                    }
                    else
                    {
                        CloseDrawer();
                    }
                }
                break;
        }
    }

    private async void CloseDrawer()
    {
        await Task.WhenAll(
            DrawerFrame.TranslateTo(0, Height, 250),
            DrawerOverlay.FadeTo(0, 250)
        );
        DrawerOverlay.IsVisible = false;
        _isDrawerOpen = false;
        await Navigation.PopModalAsync();
    }

    private async void OpenDrawer()
    {
        DrawerOverlay.IsVisible = true;
        await Task.WhenAll(
            DrawerFrame.TranslateTo(0, 0, 250),
            DrawerOverlay.FadeTo(0.5, 250)
        );
        _isDrawerOpen = true;
    }

    public async void ShowDrawer()
    {
        OpenDrawer();
        await Navigation.PushModalAsync(this);
    }
    private void OnOverlayClicked(object sender, EventArgs e)
    {
        CloseDrawer();
    }
    private void OnFrameTappedPractitioner(object sender, EventArgs e)
    {
        PractitionerTextBox.IsVisible = true;
        PractitionerFrame.Stroke = Color.FromArgb("#00d9bc");
        PatientTextBox.IsVisible = false;
      
        PatientFrame.Stroke = Color.FromArgb("#00FFFFFF");
        _authenticationVM.SelectedUserType = 2;
    }
    private void OnFrameTappedPatient(object sender, EventArgs e)
    {
        PractitionerTextBox.IsVisible = false;
        PractitionerFrame.Stroke = Color.FromArgb("#00FFFFFF");
        PatientTextBox.IsVisible = true;
        PatientFrame.Stroke = Color.FromArgb("#00d9bc");
        _authenticationVM.SelectedUserType = 1;
    }

    #endregion
}