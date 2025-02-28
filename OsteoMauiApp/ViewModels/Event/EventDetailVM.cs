using OsteoMAUIApp.Models.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.ViewModels.Event
{
   public class EventDetailVM
    {
        public EventDetailVM(INavigation navigation)
        {
            _navigation = navigation;
            UpcommingEvents = new List<EventDetailModel>
            {
            new EventDetailModel
            {
                Title = "Session 2025",
                Date = "Nov 20 - Nov 30",
                Time="01:00 - 02:00 daily",
                Location = "Fareed Town, Gujranwala,Punjab, Pakistan",
                TotalPatients = 8,
                Distance = 10
            },
            new EventDetailModel
            {
                Title = "OstaoPathy 2025",
                Date = "Nov 20 - Nov 30",
                Time="01:00 - 02:00 daily",
                Location = "Model Town, Gujranwala,Punjab, Pakistan",
                TotalPatients = 5,
                Distance = 20
            },
            new EventDetailModel
            {
                Title = "Medical 2025",
                Date = "Nov 20 - Nov 30",
                Time="01:00 - 02:00 daily",
                Location = "Wapda Town, Gujranwala,Punjab, Pakistan",
                TotalPatients = 10,
                Distance = 15.01
            },
            };
            CompletedEvents = new List<EventDetailModel>
            {
            new EventDetailModel
            {
                Title = "Session 2024",
                Date = "Nov 20 - Nov 30",
                Time="01:00 - 02:00 daily",
                Location = "Kamoke, Gujranwala,Punjab, Pakistan",
                TotalPatients = 8,
                Distance = 10
            },
            new EventDetailModel
            {
                Title = "OstaoPathy",
                Date = "Nov 20 - Nov 30",
                Time="01:00 - 02:00 daily",
                Location = "Model Town, Gujranwala,Punjab, Pakistan",
                TotalPatients = 5,
                Distance = 20
            },
            new EventDetailModel
            {
                Title = "Medical 2023",
                Date = "Nov 20 - Nov 30",
                Time="01:00 - 02:00 daily",
                Location = "Wapda Town, Gujranwala,Punjab, Pakistan",
                TotalPatients = 10,
                Distance = 15.01
            },
            };
            CancelledEvents = new List<EventDetailModel>
            {
            new EventDetailModel
            {
                Title = "Artist 2025",
                Date = "Nov 20 - Nov 30",
                Time="01:00 - 02:00 daily",
                Location = "Fareed Town, Gujranwala,Punjab, Pakistan",
                TotalPatients = 8,
                Distance = 10
            },
            new EventDetailModel
            {
                Title = "Medical Innovation",
                Date = "Nov 20 - Nov 30",
                Time="01:00 - 02:00 daily",
                Location = "Model Town, Gujranwala,Punjab, Pakistan",
                TotalPatients = 5,
                Distance = 20
            },
            new EventDetailModel
            {
                Title = "Joints",
                Date = "Nov 20 - Nov 30",
                Time="01:00 - 02:00 daily",
                Location = "Wapda Town, Gujranwala,Punjab, Pakistan",
                TotalPatients = 10,
                Distance = 15.01
            },
            };
        }
        #region |Private|
        INavigation _navigation;
        #endregion
        public List<EventDetailModel> UpcommingEvents { get; set; }
        public List<EventDetailModel> CompletedEvents { get; set; }
        public List<EventDetailModel> CancelledEvents { get; set; }
    }
}
