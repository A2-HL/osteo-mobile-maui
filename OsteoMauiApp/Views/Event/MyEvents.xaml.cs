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
}