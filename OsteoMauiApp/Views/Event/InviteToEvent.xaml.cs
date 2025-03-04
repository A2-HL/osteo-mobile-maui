using OsteoMAUIApp.ViewModels.Event;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace OsteoMAUIApp.Views.Event;

public partial class InviteToEvent : ContentPage
{
    EventVM _eventVM;
    public InviteToEvent()
	{
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        _eventVM = new EventVM(Navigation);
        BindingContext = _eventVM;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _eventVM.LoadEventsDll.Execute(null);
    }
    private void EmailEntry_Completed(object sender, EventArgs e)
    {
        var entry = sender as Entry;
        if (entry == null || string.IsNullOrWhiteSpace(entry.Text)) return;
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        SetPattern(emailPattern, entry, "Invalid Email", "Please enter a valid email address.");
    }
    private void PhoneEntry_Completed(object sender, EventArgs e)
    {
        var entry = sender as Entry;
        if (entry == null || string.IsNullOrWhiteSpace(entry.Text)) return;
        string phonePattern = @"^\+?[0-9]{10,15}$";
        SetPattern(phonePattern, entry, "Invalid Phone No.", "Please enter a valid phone no.");
    }
    private void EventInviteClicked(object sender, EventArgs e)
    {
        _eventVM.EventInviteCommand.Execute(null);
    }
    private void Remove_Clicked(object sender, EventArgs e)
    {
        var button = sender as Syncfusion.Maui.Buttons.SfButton;
        var value = button?.CommandParameter as string;
        if (value != null && _eventVM.EventInvite.emailOrPhones.Contains(value))
        {
            _eventVM.EventInvite.emailOrPhones.Remove(value);
        }
    }
    private void SetPattern(string emailPattern, Entry entry,string title,string message)
    {
        if (Regex.IsMatch(entry.Text, emailPattern))
        {
            if (!_eventVM.EventInvite.emailOrPhones.Contains(entry.Text))
            {
                _eventVM.EventInvite.emailOrPhones.Add(entry.Text);
                _eventVM.EventInvite.emailOrPhone = string.Empty;
                _eventVM.EventInvite.emailOrPhoneError = string.Empty;
                entry.Text = "";
            }
            else
            {
                _eventVM.EventInvite.emailOrPhoneError = "Already in the list.";
            }
            
        }
        else
        {
            DisplayAlert(title, message, "OK");
        }
    }
}