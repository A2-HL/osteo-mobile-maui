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
}