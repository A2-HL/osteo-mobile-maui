using Microsoft.Extensions.Logging;
using OsteoMAUIApp.ViewModels.Event;
using Syncfusion.Maui.Calendar;

namespace OsteoMAUIApp.Views.Event;

public partial class CreateEvent : ContentPage
{
	EventVM _eventVM;
    public CreateEvent()
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
        _eventVM.LoadUserGroupDll.Execute(null);
    }
    private void OnOpenDatePickerClicked(object sender, EventArgs e)
    {
        calendar.IsVisible = true;
        calendar.IsOpen = true;
    }
    private void Calendar_FooterOkClicked(object sender, Syncfusion.Maui.Calendar.CalendarSubmittedEventArgs args)
    {
        if (args.Value is CalendarDateRange dateRange)
        {
            DateTime? startDate = dateRange.StartDate;
            DateTime? endDate = dateRange.EndDate != null ? dateRange.EndDate : dateRange.StartDate;
            calendar.IsVisible = false;
            calendar.IsOpen = false;
            dateEntry.Text = dateRange.EndDate != null && dateRange.EndDate.HasValue ? $"{startDate.Value.ToString("MMM dd")}  -  {endDate.Value.ToString("MMM dd")}" : startDate.Value.ToString("MMM dd"); // Display selected range in Entry
            _eventVM.SessionDateFormate = dateRange.EndDate != null && dateRange.EndDate.HasValue ? $"{startDate.Value.ToString("MM-dd-yyyy")}  -  {endDate.Value.ToString("MM-dd-yyyy")}" : startDate.Value.ToString("MM-dd-yyyy");
        }
    }
    private void OnOpenFTimePickerClicked(object sender, EventArgs e)
    {
        fromtimePicker.IsVisible = true;
        fromtimePicker.IsOpen = true;
    }
    private void OnTimeFPickerOkButtonClicked(object sender, EventArgs e)
    {
        ftimeEntry.Text = fromtimePicker.SelectedTime?.ToString(@"hh\:mm");
        fromtimePicker.IsVisible = false;
        fromtimePicker.IsOpen = false;
    }
    private void OnOpenTTimePickerClicked(object sender, EventArgs e)
    {
        totimePicker.IsVisible = true;
        totimePicker.IsOpen = true;
    }
    private void OnTimeTPickerOkButtonClicked(object sender, EventArgs e)
    {
        ttimeEntry.Text = totimePicker.SelectedTime?.ToString(@"hh\:mm");
        totimePicker.IsVisible = false;
        totimePicker.IsOpen = false;
    }
    private void CreateEventClicked(object sender, EventArgs e)
    {
        _eventVM.CreateCommand.Execute(null);
    }
}