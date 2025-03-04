using OsteoMAUIApp.ViewModels.Event;
using System;

namespace OsteoMAUIApp.Views.Event;

public partial class EventDetail : ContentPage
{
    EventDetailVM _eventDetailVM;
    public EventDetail()
	{
		InitializeComponent();
        _eventDetailVM = new EventDetailVM(Navigation);
        BindingContext = _eventDetailVM;
    }
    public async Task InitializeAsync(string guid)
    {
        _eventDetailVM.guid = guid;//"01be4a51-7b5a-4cd0-8be0-404d7ac7002f";//guid;
        await _eventDetailVM.LoadEventDetail();//_eventDetailVM.LoadDetail.ExecuteAsync(null); // Make sure LoadDetail supports async
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        //_eventDetailVM.guid = "01be4a51-7b5a-4cd0-8be0-404d7ac7002f";
        _eventDetailVM.LoadUpcommingEventSlots.Execute(null);
        _eventDetailVM.LoadParticepants.Execute(null);
    }
}