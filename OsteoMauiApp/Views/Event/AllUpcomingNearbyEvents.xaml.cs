using OsteoMAUIApp.Models.Event;
using OsteoMAUIApp.ViewModels.Event;

namespace OsteoMAUIApp.Views.Event;

public partial class AllUpcomingNearbyEvents : ContentPage
{
	EventDetailVM _eventDetailVM;
	public AllUpcomingNearbyEvents()
	{
		InitializeComponent();
        _eventDetailVM = new EventDetailVM(Navigation);
        BindingContext = _eventDetailVM;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _eventDetailVM.LoadAllUpcommingEvents.Execute(null);
    }
    private async void SfListView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        //if (e.DataItem is EventDetailModel selectedEvent)
        //{
        //    await Navigation.PushAsync(new EventDetail(selectedEvent.Guid));
        //}
    }
}