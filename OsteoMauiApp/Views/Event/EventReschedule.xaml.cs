using OsteoMAUIApp.ViewModels.Event;
using System.Globalization;

namespace OsteoMAUIApp.Views.Event;

public partial class EventReschedule : ContentPage
{
    EventVM _eventVM;
    public EventReschedule()
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
    private void OnOpenDatePickerClicked(object sender, EventArgs e)
    {
        sessionDate.IsVisible = true;
        sessionDate.IsOpen = true;
    }
    private void Calendar_FooterOkClicked(object sender, Syncfusion.Maui.Calendar.CalendarSubmittedEventArgs args)
    {
        if (args.Value is Syncfusion.Maui.Calendar.CalendarDateRange dateRange)
        {
            DateTime? startDate = dateRange.StartDate;
            DateTime? endDate = dateRange.EndDate != null ? dateRange.EndDate: dateRange.StartDate;
            sessionDate.IsVisible = false;
            sessionDate.IsOpen = false;
            dateEntry.Text = dateRange.EndDate != null && dateRange.EndDate.HasValue ? $"{startDate.Value.ToString("MM-dd-yyyy")} - {endDate.Value.ToString("MM-dd-yyyy")}" : startDate.Value.ToString("MM-dd-yyyy"); // Display selected range in Entry
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
    private void RescheduleEventClicked(object sender, EventArgs e)
    {
        _eventVM.RescheduleCommand.Execute(null);
    }
}