using OsteoMAUIApp.Models.Event;
using OsteoMAUIApp.ViewModels.Event;

namespace OsteoMAUIApp.Views.Event;

public partial class MyEvents : ContentPage
{
    EventDetailVM _eventDetailVM;
    public MyEvents()
	{
		InitializeComponent();
        _eventDetailVM = new EventDetailVM(Navigation);
        BindingContext = _eventDetailVM;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _eventDetailVM.LoadMyEvents.Execute(null);
    }
    private async void SfListView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        if (e.DataItem is EventDetailModel selectedEvent)
        {
            var detailPage = new EventDetail();
            await detailPage.InitializeAsync(selectedEvent.Guid);
            await Navigation.PushAsync(detailPage);//(new EventDetail(selectedEvent.Guid));
        }
    }
}
