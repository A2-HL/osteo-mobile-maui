using OsteoMAUIApp.ViewModels.Event;

namespace OsteoMAUIApp.Views.Event;

public partial class EventReschedule : ContentPage
{
    public string eventGuid { get; set; }
    EventDetailVM _eventDetailVM;
    public EventReschedule(string guid)
	{
        try
        {
            InitializeComponent();
            eventGuid = guid;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        _eventDetailVM = new EventDetailVM(Navigation);
        BindingContext = _eventDetailVM;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _eventDetailVM.guid = eventGuid; 
        _eventDetailVM.LoadResheduleDetail.Execute(null);
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
            DateTime? endDate = dateRange.EndDate != null ? dateRange.EndDate : dateRange.StartDate;
            sessionDate.IsVisible = false;
            sessionDate.IsOpen = false;
            dateEntry.Text = dateRange.EndDate != null && dateRange.EndDate.HasValue ? $"{startDate.Value.ToString("MMM dd")}  -  {endDate.Value.ToString("MMM dd")}" : startDate.Value.ToString("MMM dd"); // Display selected range in Entry
            _eventDetailVM.RescheduleDateFormate = dateRange.EndDate != null && dateRange.EndDate.HasValue ? $"{startDate.Value.ToString("MM-dd-yyyy")}  -  {endDate.Value.ToString("MM-dd-yyyy")}" : startDate.Value.ToString("MM-dd-yyyy");
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
        _eventDetailVM.RescheduleCommand.Execute(null);
    }
}